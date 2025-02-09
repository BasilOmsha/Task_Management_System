import { useState } from "react"

import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle
} from "@/components/ui/alert-dialog"
import { Button } from "@/components/ui/button"
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger
} from "@/components/ui/dropdown-menu"
import { Input } from "@/components/ui/input"
// import { useRemoveList, useUpdateList } from '@/features/kanban-management/useList'
import { MoreHorizontal } from "lucide-react"

interface ColumnActionsProps {
    id: string
    title: string
}

export default function ColumnActions({ title }: ColumnActionsProps) {
    const [isEditing, setIsEditing] = useState(false)
    const [newTitle, setNewTitle] = useState(title)
    const [showDeleteDialog, setShowDeleteDialog] = useState(false)
    // const updateList = useUpdateList()
    // const removeList = useRemoveList()

    // const handleRename = () => {
    //     if (newTitle.trim() !== title) {
    //         updateList.mutate({ id, name: newTitle.trim() })
    //     }
    //     setIsEditing(false)
    // }

    // const handleDelete = () => {
    //     removeList.mutate(id)
    //     setShowDeleteDialog(false)
    // }

    return (
        <div className="flex items-center">
            {isEditing ? (
                // <form onSubmit={(e) => { e.preventDefault(); handleRename() }}>
                <form
                    onSubmit={(e) => {
                        e.preventDefault()
                    }}
                >
                    <Input
                        autoFocus
                        className="w-40"
                        // onBlur={handleRename}
                        onChange={(e) => setNewTitle(e.target.value)}
                        value={newTitle}
                    />
                </form>
            ) : (
                <h3 className="font-medium text-lg">{title}</h3>
            )}
            <DropdownMenu>
                <DropdownMenuTrigger asChild>
                    <Button className="h-8 w-8 p-0 ml-2" variant="ghost">
                        <MoreHorizontal className="h-4 w-4" />
                    </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                    <DropdownMenuItem onClick={() => setIsEditing(true)}>Rename</DropdownMenuItem>
                    <DropdownMenuItem
                        className="text-red-600"
                        onClick={() => setShowDeleteDialog(true)}
                    >
                        Delete
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
            <AlertDialog onOpenChange={setShowDeleteDialog} open={showDeleteDialog}>
                <AlertDialogContent>
                    <AlertDialogHeader>
                        <AlertDialogTitle>
                            Are you sure you want to delete this list?
                        </AlertDialogTitle>
                        <AlertDialogDescription>
                            This action cannot be undone. All tasks in this list will also be
                            deleted.
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                        {/* <AlertDialogAction className="bg-red-600 hover:bg-red-700" onClick={handleDelete}> */}
                        <AlertDialogAction className="bg-red-600 hover:bg-red-700">
                            Delete
                        </AlertDialogAction>
                    </AlertDialogFooter>
                </AlertDialogContent>
            </AlertDialog>
        </div>
    )
}
