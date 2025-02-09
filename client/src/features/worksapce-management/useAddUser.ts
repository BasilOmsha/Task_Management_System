import toast from "react-hot-toast"

import workspace from "@/api/workspace-services"
import { WorkspaceUserData } from "@/schemas/workspace"
import { ApiError } from "@/utils/errors"
import { useMutation , useQuery, useQueryClient } from "@tanstack/react-query"

import { useActiveWorkspace } from "./provider"


export function useAddUser() {
    const queryClient = useQueryClient()
    const { activeWorkspace } = useActiveWorkspace()
    return useMutation<WorkspaceUserData, ApiError, WorkspaceUserData>({
        mutationKey: ['addUserToWorkspace'],
        mutationFn:  workspace.AddWorkspaceMember,
        
        onSuccess: (data, variables) => {
            // Invalidate all-workspace-users query for this specific workspace
            queryClient.invalidateQueries({ 
                queryKey: ['all-workspace-users', variables.workspaceId]
            })

            // Invalidate workspace data
            queryClient.invalidateQueries({
                queryKey: ['workspace', activeWorkspace?.id]
            })
            
            toast.success('User invited successfully')
        },
        onError: () => {
            toast.error('Failed to invite user')
        },

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