import { Button } from '@/components/ui/button.tsx'
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog.tsx"

interface RefreshPromptProps {
    open: boolean;
    onResponse: (stayLoggedIn: boolean) => void;
}

export function RefreshPrompt({ open, onResponse }: RefreshPromptProps) {
    return (
        <Dialog onOpenChange={(open) => !open && onResponse(false)} open={open}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Session Expired</DialogTitle>
                    <DialogDescription>
                        Are you still here?
                    </DialogDescription>
                </DialogHeader>
                <DialogFooter>
                    <Button onClick={() => onResponse(false)} variant="outline">
                        Log out
                    </Button>
                    <Button onClick={() => onResponse(true)}>
                        Stay logged in
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}

