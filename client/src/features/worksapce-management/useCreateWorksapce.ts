import createWorkspace from "@/api/workspace-services.ts"
import { WorkspaceData, WorkspaceResponse } from "@/schemas/workspace.ts"
import { ApiError } from "@/utils/errors.ts"
import { useMutation } from "@tanstack/react-query"

export function useCreateWorkspace() {
    return useMutation<WorkspaceResponse, ApiError, WorkspaceData>({
        mutationKey: ['createWorkspace'],
        mutationFn: createWorkspace.AddOne,
        
        retry: (failureCount: number, error: ApiError) => {
            const noRetryConditions = [
                error.message.includes('already exists'),  
                error.statusCode === 429,  
                error.statusCode === 400, 
                error.statusCode === 422, 
                error.statusCode === 403 
            ]

            if (noRetryConditions.some(condition => condition)) {
                return false
            }
            return failureCount < 3
        },

        // exponential backoff for retries, capped at 30 seconds
        retryDelay: (attemptIndex) => Math.min(1000 * (2 ** attemptIndex), 30000),
    })
}