'use client'

import React, { useState } from 'react'
import toast from 'react-hot-toast'

import { Button } from '@/components/ui/button'
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from '@/components/ui/dialog'
import { Input } from '@/components/ui/input'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Textarea } from '@/components/ui/textarea'
import { useCreateTaskCard } from '@/features/kanban-management/useTaskCard'
import { ListResponse } from '@/schemas/list'
import { CreateTaskCardData } from '@/schemas/task-card'

interface NewTaskDialogProps {
    projectBoardId: string
    lists: ListResponse[]
}

export default function NewTaskDialog({ lists }: NewTaskDialogProps) {
    const [open, setOpen] = useState(false)
    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')
    const [listId, setListId] = useState('')
    const createTask = useCreateTaskCard()

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault()
        if (title.trim() && listId) {
            const newTask: CreateTaskCardData = {
                title: title.trim(),
                description: description.trim(),
                listId,
                position: lists.find(list => list.id === listId)?.taskCard?.length || 0,
            }
            createTask.mutate(newTask, {
                onSuccess: () => {
                    setTitle('')
                    setDescription('')
                    setListId('')
                    setOpen(false)
                },
                onError: (error) => {
                    console.error('Error creating task:', error)
                    toast.error('Failed to create task. Please try again.')
                }
            })
        }
    }

    return (
        <Dialog onOpenChange={setOpen} open={open}>
            <DialogTrigger asChild>
                <Button size="sm" variant="secondary">+ Add New Task</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Add New Task</DialogTitle>
                    <DialogDescription>
            What do you want to get done today?
                    </DialogDescription>
                </DialogHeader>
                <form id="new-task-form" onSubmit={handleSubmit}>
                    <div className="grid gap-4 py-4">
                        <Input
                            id="title"
                            onChange={(e) => setTitle(e.target.value)}
                            placeholder="Task title..."
                            value={title}
                        />
                        <Textarea
                            id="description"
                            onChange={(e) => setDescription(e.target.value)}
                            placeholder="Description..."
                            value={description}
                        />
                        <Select onValueChange={setListId} value={listId}>
                            <SelectTrigger>
                                <SelectValue placeholder="Select a section" />
                            </SelectTrigger>
                            <SelectContent>
                                {lists.map((list) => (
                                    <SelectItem key={list.id} value={list.id}>
                                        {list.name}
                                    </SelectItem>
                                ))}
                            </SelectContent>
                        </Select>
                    </div>
                </form>
                <DialogFooter>
                    <Button form="new-task-form" type="submit">Add Task</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}

