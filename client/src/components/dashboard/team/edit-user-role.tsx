import { useState } from 'react'

import { Button } from "@/components/ui/button"
import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { User } from '@/pages/dashboard/team/team'



type EditUserRoleDialogProps = {
    isOpen: boolean
    onClose: () => void
    onConfirm: (newRole: User['role']) => void
    currentRole: User['role']
    userName: string
}

export function EditUserRoleDialog({ isOpen, onClose, onConfirm, currentRole, userName }: EditUserRoleDialogProps) {
    const [role, setRole] = useState<User['role']>(currentRole)

    return (
        <Dialog onOpenChange={onClose} open={isOpen}>
            <DialogContent className="sm:max-w-[425px] flex flex-col items-center">
                <DialogHeader>
                    <DialogTitle>Edit User Role</DialogTitle>
                </DialogHeader>
                <div className="flex flex-col items-center gap-4 py-4 w-full">
                    <Label htmlFor="role">
            Role for {userName}
                    </Label>
                    <Select onValueChange={(value: User['role']) => setRole(value)} value={role}>
                        <SelectTrigger className="w-full max-w-[250px]">
                            <SelectValue placeholder="Select a role" />
                        </SelectTrigger>
                        <SelectContent>
                            <SelectItem value="Admin">Admin</SelectItem>
                            <SelectItem value="Editor">Editor</SelectItem>
                            <SelectItem value="Viewer">Viewer</SelectItem>
                        </SelectContent>
                    </Select>
                </div>
                <DialogFooter className="w-full flex justify-center pt-4">
                    <Button className="w-full max-w-[250px]" onClick={() => onConfirm(role)}>
            Save Changes
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}

