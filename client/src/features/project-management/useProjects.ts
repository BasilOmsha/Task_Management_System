import GetProjectsByWorkspce from "@/api/project-services.ts"
import { ProjectResponse } from "@/schemas/project.ts"
import { ApiError } from "@/utils/errors.ts"
import { useQuery } from "@tanstack/react-query"

export function useProjects(workspaceId?: string) {
    return useQuery<ProjectResponse[], ApiError>({
        queryKey: ['projects', workspaceId],
        queryFn: () => {
            if (!workspaceId) {
                throw new Error("workspaceId is required")
            }
            return GetProjectsByWorkspce.getProjectsByWorkspace(workspaceId)
        },
        retry: (failureCount: number, error: ApiError) => {
            const noRetryConditions = [
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
        enabled: !!workspaceId,
        select: (data) => {
            // Ensure we handle both array and object responses
            if (Array.isArray(data)) {
                return data
            }
            // If data is wrapped in a data property
            if (data && 'data' in data) {
                const typedData = data as { data: ProjectResponse[] }
                return Array.isArray(typedData.data) ? typedData.data : []
            }
            return []
        }
    })
}