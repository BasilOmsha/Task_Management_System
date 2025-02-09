import GetAllWorkspacesByUser from "@/api/workspace-services.ts"
import workspace from "@/api/workspace-services.ts"
import { WorkspaceResponse } from "@/schemas/workspace.ts"
import { ApiError } from "@/utils/errors.ts"
import { useQuery } from "@tanstack/react-query"

import { useActiveWorkspace } from "./provider.tsx"

import { useUser } from "../user-management/useUser.ts"

export function useWorkspaces() {
    const { userData } = useUser()
    const { activeWorkspace, setActiveWorkspace } = useActiveWorkspace()

    // Get single workspace details
    const { data: refreshedWorkspace } = useQuery({
        queryKey: ['workspace', activeWorkspace?.id],
        queryFn: () => activeWorkspace ? workspace.GetOne(activeWorkspace.id) : null,
        enabled: !!activeWorkspace,
    })

    // Main workspaces query
    return useQuery<WorkspaceResponse[], ApiError>({
        queryKey: ['getAllWorkspacesByAuthUser', userData?.id],
        queryFn: workspace.GetAllByUser,
        enabled: !!userData?.id,
        select: (data) => {
            // If we have fresh single workspace data, use that instead
            if (activeWorkspace && refreshedWorkspace) {
                setActiveWorkspace(refreshedWorkspace)
            }
            return data
        },
    })
}