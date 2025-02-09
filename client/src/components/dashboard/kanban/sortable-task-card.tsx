import React from 'react'

import { TaskCardResponse } from '@/schemas/task-card'
import { useSortable } from '@dnd-kit/sortable'
import { CSS } from '@dnd-kit/utilities'
import { cva } from 'class-variance-authority'

import TaskCard from './task-card'

interface SortableTaskCardProps {
    task: TaskCardResponse
}

function SortableTaskCard({ task }: SortableTaskCardProps) {
    const {
        attributes,
        listeners,
        setNodeRef,
        transform,
        transition,
        isDragging,
    } = useSortable({
        id: task.id,
        data: {
            type: 'task',
        },
    })

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
        opacity: isDragging ? 0.5 : 1,
    }

    const variants = cva('mb-2 cursor-grab', {
        variants: {
            dragging: {
                over: 'ring-2 opacity-30',
                overlay: 'ring-2 ring-primary'
            }
        }
    })

    return (
        <div 
            className={variants({
                dragging: isDragging ? 'over' : undefined
            })} 
            ref={setNodeRef}
            style={style}
            {...attributes} 
            {...listeners}
        >
            <TaskCard task={task} />
        </div>
    )
}

export default React.memo(SortableTaskCard)

