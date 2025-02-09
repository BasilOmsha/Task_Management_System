import registerUser from "@/api/user-service.ts"
import { UserResponse, userSchema } from "@/schemas/user.ts"
import { ApiError } from "@/utils/errors.ts"
import { useMutation } from "@tanstack/react-query"
import { z } from "zod"

export function useRegister() {
    return useMutation<UserResponse, ApiError, z.infer<typeof userSchema>>({
        mutationKey: ['registerUser'],
        mutationFn: registerUser.AddOne,
        
        // Don't retry for specific error conditions
        retry: (failureCount: number, error: ApiError) => {
            // Never retry for these cases
            const noRetryConditions = [
                error.message.includes('already exists'),  // Duplicate entries
                error.statusCode === 429,                 // Rate limiting
                error.statusCode === 400,                 // Bad request
                error.statusCode === 422                  // Validation error
            ]

            if (noRetryConditions.some(condition => condition)) {
                return false
            }

            return failureCount < 3
        },
        retryDelay: (attemptIndex) => Math.min(1000 * (2 ** attemptIndex), 30000),
        
    })
}