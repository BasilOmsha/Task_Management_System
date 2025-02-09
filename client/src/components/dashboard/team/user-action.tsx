import { useState } from 'react'

import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { User } from '@/pages/dashboard/team/team'
import { MoreHorizontal } from 'lucide-react'

import { DeleteUserDialog } from './delete-user-dialog'
import { EditUserRoleDialog } from './edit-user-role'





type UserActionsProps = {
    user: User
    onRemoveUser: (userId: string) => void
    onEditUserRole: (userId: string, newRole: User['role']) => void
}

export function UserActions({ user, onRemoveUser, onEditUserRole }: UserActionsProps) {
    const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false)
    const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)

    return (
        <>
            <DropdownMenu>
                <DropdownMenuTrigger asChild>
                    <Button className="h-8 w-8 p-0" variant="ghost">
                        <MoreHorizontal className="h-4 w-4" />
                    </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                    <DropdownMenuItem onSelect={() => setIsEditDialogOpen(true)}>
            Edit Role
                    </DropdownMenuItem>
                    <DropdownMenuItem className="text-red-600" onSelect={() => setIsDeleteDialogOpen(true)}>
            Delete
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>

            <DeleteUserDialog
                isOpen={isDeleteDialogOpen}
                onClose={() => setIsDeleteDialogOpen(false)}
                onConfirm={() => {
                    onRemoveUser(user.id)
                    setIsDeleteDialogOpen(false)
                }}
                userName={user.name}
            />

            <EditUserRoleDialog
                currentRole={user.role}
                isOpen={isEditDialogOpen}
                onClose={() => setIsEditDialogOpen(false)}
                onConfirm={(newRole) => {
                    onEditUserRole(user.id, newRole)
                    setIsEditDialogOpen(false)
                }}
                userName={user.name}
            />
        </>
    )
}

