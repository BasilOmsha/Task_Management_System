import { useState } from "react"
import toast from "react-hot-toast"

import { InviteUserDialog } from "@/components/dashboard/team/invite-user-dialog"
import { TeamTable } from "@/components/dashboard/team/team-table"
import { Button } from "@/components/ui/button"
import { useWorkspaceUsers } from "@/features/user-management/useWorksapceUsers"
import { useActiveWorkspace } from "@/features/worksapce-management/provider"
import { useQueryClient } from "@tanstack/react-query"
import { UserPlus } from "lucide-react"

import "./styles.module.css"

export type User = {
    id: string
    name: string
    username: string
    email: string
    role: "Admin" | "Contributor"
}

export default function Team() {
    const { activeWorkspace } = useActiveWorkspace()
    const [isDialogOpen, setIsDialogOpen] = useState(false)

    const workspaceUsers = activeWorkspace?.workspaceUsers ?? null
    console.log("Workspace Users:", workspaceUsers)

    // Get user details
    const userQueries = useWorkspaceUsers(activeWorkspace?.id, workspaceUsers)

    const queryClient = useQueryClient()

    // Check if any queries are loading
    const isLoading = userQueries.some((query) => query.isLoading)

    const usersInfo =
        workspaceUsers?.map((workspaceUser, index) => {
            const userDetails = userQueries[index]?.data as
                | { firstName: string; lastName: string; email: string }
                | undefined

            return {
                id: workspaceUser.userId,
                name: userDetails
                    ? `${userDetails.firstName} ${userDetails.lastName}`
                    : workspaceUser.username,
                username: workspaceUser.username,
                email: userDetails?.email ?? "",
                role: workspaceUser.roleName as User["role"]
            }
        }) ?? []

    const handleInviteUser = (newUser: User) => {
        // Handle invite logic
        // queryClient.invalidateQueries({queryKey: ['user', newUser.id]})
        // queryClient.invalidateQueries({queryKey: ['workspace-user', activeWorkspace?.id, newUser.id]})
        // queryClient.invalidateQueries({queryKey: ['workspaceUser', activeWorkspace?.id]})
        setIsDialogOpen(false)
    }

    const handleRemoveUser = (userId: string) => {
        try {
            // Add your remove user mutation here
            // await removeWorkspaceUser.mutateAsync({ workspaceId: activeWorkspace?.id, userId })
            toast.success("User removed successfully")
        } catch (error) {
            toast.error("Failed to remove user")
        }
    }

    const handleEditUserRole = (userId: string, newRole: User["role"]) => {
        try {
            // Add your edit role mutation here
            // await updateWorkspaceUserRole.mutateAsync({
            //     workspaceId: activeWorkspace?.id,
            //     userId,
            //     roleName: newRole
            // })
            toast.success("User role updated successfully")
        } catch (error) {
            toast.error("Failed to update user role")
        }
    }

    return (
        // <div className="team-management">
        <div className="container mx-auto py-10 px-4">
            <div className="flex justify-between items-center mb-8">
                <h1>Workspace Team</h1>
                <Button
                    className="bg-blue-600 hover:bg-blue-700 text-white"
                    onClick={() => setIsDialogOpen(true)}
                >
                    Invite User
                    <UserPlus className="ml-2 h-4 w-4" />
                </Button>
            </div>

            <TeamTable
                isLoading={isLoading}
                onEditUserRole={handleEditUserRole}
                onRemoveUser={handleRemoveUser}
                users={usersInfo ?? []}
            />

            <InviteUserDialog
                isOpen={isDialogOpen}
                onClose={() => setIsDialogOpen(false)}
                onInvite={handleInviteUser}
                workspaceId={activeWorkspace?.id ?? ""}
            />
        </div>
        // </div>
    )
}
