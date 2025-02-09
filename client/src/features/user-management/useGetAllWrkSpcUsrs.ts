import workspace from '@/api/workspace-services'
import { ApiError } from '@/utils/errors'
import { useQuery } from '@tanstack/react-query'

type WorkSpceUsersResponse = {
    workspaceId: string;
    userId: string;
    roleId: string;
}

export function useGetAllWorkspaceUsers(id: string, p0: never[]) {

    const { data, isLoading, error } = useQuery<WorkSpceUsersResponse[]>({
        queryKey: ['all-workspace-users', id],
        queryFn: async () => {
            const users = await workspace.GetAllUsersByWorkSpace(id)
            return users.map(user => ({
                workspaceId: user.workspaceId,
                userId: user.userId,
                roleId: user.roleId
            }))
        },
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