import userService from "@/api/user-service"
import { UserResponse } from "@/schemas/user"
import { ApiError } from "@/utils/errors"
import { useQuery } from "@tanstack/react-query"

export const useFindUserByEmail = (email: string) => {
    const { data, isLoading, error } = useQuery<UserResponse, Error>({
        queryKey: ["user", email],
        queryFn: () => userService.GetUserByEmail(email),
        enabled: !!email,
        retry: (failureCount: number, error: ApiError) => {
        
            const noRetryConditions = [
                error.statusCode === 429,
                error.statusCode === 400,                 
                error.statusCode === 422,
                error.statusCode === 404,
                error.statusCode === 401,
                error.statusCode === 403, 
                error.statusCode === 500,
        
            ]
            if (noRetryConditions.some(condition => condition)) {
                return false
            }
            return failureCount < 3
        },        
    })
    return { data, isLoading, error }
}