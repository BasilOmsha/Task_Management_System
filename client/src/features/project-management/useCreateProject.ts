import createProject from "@/api/project-services.ts"
import { ProjectData, ProjectResponse } from "@/schemas/project.ts"
import { ApiError } from "@/utils/errors.ts"
import { useMutation } from "@tanstack/react-query"


export function useCreateProject() {
    return useMutation<ProjectResponse, ApiError, ProjectData>({
        mutationKey: ['createProject'],
        mutationFn: createProject.AddOne,
        
        retry: (failureCount: number, error: ApiError) => {
            const noRetryConditions = [
                error.message.includes('already exists'),  
                error.statusCode === 429,  
                error.statusCode === 400, 
                error.statusCode === 422, 
                error.statusCode === 403,
                error.statusCode === 404
            ]

            if (noRetryConditions.some(condition => condition)) {
                return false
            }
            return failureCount < 3
        },
        retryDelay: (attemptIndex) => Math.min(1000 * (2 ** attemptIndex), 30000),
    })
}