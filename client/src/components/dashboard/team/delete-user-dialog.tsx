import { Button } from "@/components/ui/button"
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog"

type DeleteUserDialogProps = {
    isOpen: boolean
    onClose: () => void
    onConfirm: () => void
    userName: string
}

export function DeleteUserDialog({ isOpen, onClose, onConfirm, userName }: DeleteUserDialogProps) {
    return (
        <Dialog onOpenChange={onClose} open={isOpen}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Confirm Deletion</DialogTitle>
                    <DialogDescription>
            Are you sure you want to remove {userName} from the team?
                    </DialogDescription>
                </DialogHeader>
                <DialogFooter>
                    <Button onClick={onClose} variant="outline">Cancel</Button>
                    <Button onClick={onConfirm} variant="destructive">Delete</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}

