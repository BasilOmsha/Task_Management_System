import getUserInfo from "@/api/user-service"
import { WorkspaceResponse } from "@/schemas/workspace"
import { useQueries } from "@tanstack/react-query"

export function useWorkspaceUsers(
    workspaceId: string | undefined,
    workspaceUsers: WorkspaceResponse["workspaceUsers"]
) {
    return useQueries({
        queries: (workspaceUsers ?? []).map((workspaceUser) => ({
            queryKey: ["workspace-user", workspaceId, workspaceUser.userId],
            queryFn: () => getUserInfo.GetOne(workspaceUser.userId),
            enabled: !!workspaceUser.userId,
            refetchOnMount: true,
            refetchOnWindowFocus: false,
            staleTime: 0
            // staleTime: 0,
            // cacheTime: 0,
        }))
    })
}

// export function useWorkspaceUsers(workspaceId?: string | undefined) {

//     return useQuery<WorkspaceUserData[], ApiError>({
//         queryKey: ['all-workspace-users', workspaceId],
//         queryFn: () => {
//             if (!workspaceId) {
//                 throw new Error("workspaceId is required")
//             }
//             return workspace.GetAllUsersByWorkSpace(workspaceId)
//         },
//         retry: (failureCount: number, error: ApiError) => {
//             const noRetryConditions = [
//                 error.statusCode === 429,
//                 error.statusCode === 400,
//                 error.statusCode === 422,
//                 error.statusCode === 403,
//                 error.statusCode === 404
//             ]

//             if (noRetryConditions.some(condition => condition)) {
//                 return false
//             }
//             return failureCount < 3
//         },
//         enabled: !!workspaceId,
//     })
// }
