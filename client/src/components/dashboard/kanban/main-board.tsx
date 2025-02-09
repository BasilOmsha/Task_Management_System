/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { useState } from 'react'

import {  useLists, useUpdateListPositions } from '@/features/kanban-management/useList'
import { useUpdateTaskCardPositions } from '@/features/kanban-management/useTaskCard'
import { ListResponse } from '@/schemas/list'
import { TaskCardResponse } from '@/schemas/task-card'
import { DndContext, DragEndEvent, DragOverEvent, DragOverlay, DragStartEvent, closestCorners, defaultDropAnimationSideEffects } from '@dnd-kit/core'
import { SortableContext, arrayMove, horizontalListSortingStrategy } from '@dnd-kit/sortable'
import { useQueryClient } from "@tanstack/react-query"

import BoardColumn from './list'
import NewListDialog from './new-section-dialog'
import NewTaskDialog from './new-task-dialog'
import TaskCard from './task-card'

interface KanbanBoardProps {
    projectBoardId: string
}

export default function KanbanBoard({ projectBoardId }: KanbanBoardProps) {
    const { data: lists, isLoading } = useLists(projectBoardId)
    const [activeId, setActiveId] = useState<string | null>(null)
    const [activeType, setActiveType] = useState<'list' | 'task' | null>(null)
    const updateListPositions = useUpdateListPositions()
    const updateTaskPositions = useUpdateTaskCardPositions()
    const queryClient = useQueryClient()

    const handleDragStart = (event: DragStartEvent) => {
        const { active } = event
        setActiveId(active.id as string)
        setActiveType(active.data.current?.type || null)
    }

    const handleDragOver = (event: DragOverEvent) => {
        const { active, over } = event
        if (!over) {return}
    
        const activeId = active.id
        const overId = over.id
    
        if (activeType === 'task') {
            const activeList = lists!.find(list => list.taskCard?.some(task => task.id === activeId))
            const overList = lists!.find(list => list.id === overId || list.taskCard?.some(task => task.id === overId))
    
            if (!activeList || !overList) {return}
    
            // Handle dragging to a new position in the same list or a different list
            if (activeList !== overList || active.id !== over.id) {
                const activeIndex = activeList.taskCard!.findIndex(task => task.id === activeId)
                let overIndex: number
    
                if (over.id === overList.id) {
                    // Dropped on list - place at end
                    overIndex = overList.taskCard?.length || 0
                } else {
                    // Dropped on a task - get its position
                    overIndex = overList.taskCard!.findIndex(task => task.id === overId)
                }
    
                const newLists = lists!.map(list => {
                    if (list.id === activeList.id) {
                        return {
                            ...list,
                            taskCard: list.taskCard!.filter(task => task.id !== activeId)
                        }
                    }
                    if (list.id === overList.id) {
                        const newTaskCards = [...list.taskCard!]
                        if (overIndex >= 0) {
                            newTaskCards.splice(overIndex, 0, activeList.taskCard![activeIndex])
                        } else {
                            newTaskCards.push(activeList.taskCard![activeIndex])
                        }
                        return {
                            ...list,
                            taskCard: newTaskCards
                        }
                    }
                    return list
                })
    
                // Optimistic update for smoother UI
                queryClient.setQueryData(['lists'], newLists)
            }
        }
    }

    const handleDragEnd = (event: DragEndEvent) => {
        const { active, over } = event
    
        if (!over) {return}

        if (active.id !== over.id) {
            if (activeType === 'list') {
            // Handle column movement
                const oldIndex = lists!.findIndex((list) => list.id === active.id)
                const newIndex = lists!.findIndex((list) => list.id === over.id)
    
                const updates = arrayMove(lists!, oldIndex, newIndex).map((list, index) => ({
                    listId: list.id,
                    newPosition: index,
                }))
    
                updateListPositions.mutate(updates)
            } else if (activeType === 'task') {
            // Handle task movement (your existing working task movement code)
                const activeList = lists?.find(list => list.taskCard?.some(task => task.id === active.id))
                const overList = lists?.find(list => list.id === over.id || list.taskCard?.some(task => task.id === over.id))

                if (activeList && overList) {
                    const listTasks = [...overList.taskCard!]
                    const activeTask = activeList.taskCard!.find(task => task.id === active.id)
                    const activeIndex = activeList.taskCard!.findIndex(task => task.id === active.id)
                    const overIndex = over.id === overList.id 
                        ? listTasks.length 
                        : listTasks.findIndex(task => task.id === over.id)

                    if (activeList.id === overList.id) {
                        listTasks.splice(activeIndex, 1)
                    }

                    listTasks.splice(overIndex, 0, activeTask!)

                    const updates = listTasks.map((task, index) => ({
                        taskCardId: task.id,
                        listId: overList.id,
                        position: index
                    }))

                    updateTaskPositions.mutate(updates)
                }
            }
        }
    
        setActiveId(null)
        setActiveType(null)
    }

    if (isLoading) {return <div>Loading...</div>}

    return (
        <>
            <DndContext
                collisionDetection={closestCorners}
                // modifiers={[restrictToHorizontalAxis]}
                onDragEnd={handleDragEnd}
                onDragOver={handleDragOver}
                onDragStart={handleDragStart}
            >
                <div className="flex overflow-x-auto p-4 transition-transform duration-200 ease-in-out">
                    <SortableContext 
                        items={lists?.map(list => list.id) || []} 
                        strategy={horizontalListSortingStrategy}
                    >
                        {lists?.map((list) => (
                            <BoardColumn key={list.id} list={list} />
                        ))}
                        <NewListDialog projectBoardId={projectBoardId} />
                    </SortableContext>
                </div>
            
                <DragOverlay dropAnimation={{
                    duration: 0.01,
                    easing: 'cubic-bezier(0.18, 0.67, 0.6, 1.22)',
                    sideEffects: defaultDropAnimationSideEffects({
                        styles: {
                            active: {
                                opacity: '0.5'
                            }
                        }
                    })
                }}>
                    {activeId && activeType === 'task' && (
                        <TaskCard 
                            task={lists?.flatMap(list => list.taskCard || [])
                                .find(task => task.id === activeId) as TaskCardResponse} 
                        />
                    )}
                    {activeId && activeType === 'list' && (
                        <BoardColumn 
                            list={lists?.find(list => list.id === activeId) as ListResponse} 
                        />
                    )}
                </DragOverlay>
                {projectBoardId && <NewTaskDialog lists={lists || []} projectBoardId={projectBoardId} />}
            </DndContext>
        </>
    )
}

