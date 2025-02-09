import React, { useState } from "react"

import { Button } from "@/components/ui/button"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { useCreateList, useLists } from "@/features/kanban-management/useList"
import { ListData } from "@/schemas/list"
import { useQueryClient } from "@tanstack/react-query"

interface NewListDialogProps {
    projectBoardId: string
}

export default function NewListDialog({ projectBoardId }: NewListDialogProps) {
    const [open, setOpen] = useState(false)
    const [name, setName] = useState("")
    const createList = useCreateList()
    const queryClient = useQueryClient()
    const { data: lists } = useLists(projectBoardId)

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        if (name.trim()) {
            const newList: ListData = {
                name: name.trim(),
                projectBoardId,
                position: lists?.length || 0
            }
            try {
                const result = await createList.mutateAsync(newList)
                console.log("List created successfully:", result)
                setName("")
                queryClient.invalidateQueries({ queryKey: ["lists", projectBoardId] })
            } catch (error) {
                console.error("Error creating list:", error)
            }
        }
    }

    return (
        <Dialog onOpenChange={setOpen} open={open}>
            <DialogTrigger asChild>
                <Button className="w-80 mx-2" variant="outline">
                    + Add New Section
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Add New Section</DialogTitle>
                    <DialogDescription>
                        Create a new section for your Kanban board.
                    </DialogDescription>
                </DialogHeader>
                <form id="new-section-form" onSubmit={handleSubmit}>
                    <div className="grid gap-4 py-4">
                        <Input
                            id="name"
                            onChange={(e) => setName(e.target.value)}
                            placeholder="Section name"
                            value={name}
                        />
                    </div>
                </form>
                <DialogFooter>
                    <Button form="new-section-form" type="submit">
                        Add Section
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}
