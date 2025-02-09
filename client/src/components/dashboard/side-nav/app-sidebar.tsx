import { ComponentProps } from "react"

import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarHeader,
    SidebarRail,
} from "@/components/ui/sidebar.tsx"
import { useUser } from "@/features/user-management/useUser.ts"
import { useActiveWorkspace } from "@/features/worksapce-management/provider.tsx"
import { useWorkspaces } from "@/features/worksapce-management/useWorksace.ts"
import {
    BriefcaseBusiness,

} from "lucide-react"

import { NavMain } from "./layout/nav-main.tsx"
import { NavUser } from "./layout/nav-user.tsx"
import { WorkspaceSwitcher } from "./layout/worksapce-switcher.tsx"

export function AppSidebar({ ...props }: ComponentProps<typeof Sidebar>) {
    const { userData } = useUser()
    const { data: workspaces, isLoading } = useWorkspaces()
    const { activeWorkspace } = useActiveWorkspace()

    const data = {
        user: {
            firstname: userData?.firstName ?? "",
            lastname: userData?.lastName ?? "",
            email: userData?.email ?? "",
            avatar: "/avatars/shadcn.jpg",
        }
    }
    
    // const transformedWorkspaces = workspaces?.map(workspace => ({
    //     id: workspace.id,
    //     name: workspace.name,
    //     logo: BriefcaseBusiness,
    //     role: workspace.workspaceUsers?.[0]?.roleName || "Default",
    //     originalData: workspace // Keep the original workspace data
    // })) || []

    const transformedWorkspaces = workspaces?.map(workspace => {
        // If this is the active workspace, use its current data
        const workspaceData = workspace.id === activeWorkspace?.id ? 
            activeWorkspace : workspace

        // Find the role for current user
        const currentUserRole = workspaceData.workspaceUsers?.find(
            wu => wu.userId === userData?.id
        )?.roleName || "Default"

        return {
            id: workspaceData.id,
            name: workspaceData.name,
            logo: BriefcaseBusiness,
            role: currentUserRole,
            originalData: workspaceData
        }
    }) || []

    return (
        <Sidebar collapsible="icon" {...props}>
            <SidebarHeader>
                {isLoading ? (
                    <div>Loading workspaces...</div>
                ) : (
                    <WorkspaceSwitcher workspaces={transformedWorkspaces} />
                )}
            </SidebarHeader>
            <SidebarContent>
                <NavMain />
            </SidebarContent>
            <SidebarFooter>
                <NavUser user={data.user} />
            </SidebarFooter>
            <SidebarRail />
        </Sidebar>
    )
}
