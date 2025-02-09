import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader } from "@/components/ui/card"
import { ListResponse } from "@/schemas/list"
import { SortableContext, useSortable, verticalListSortingStrategy } from "@dnd-kit/sortable"
import { CSS } from "@dnd-kit/utilities"
import { cva } from "class-variance-authority"
import { GripVertical } from "lucide-react"

import ColumnActions from "./column-action"
import SortableTaskCard from "./sortable-task-card"

interface BoardColumnProps {
    list: ListResponse
}

export default function BoardColumn({ list }: BoardColumnProps) {
    const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
        id: list.id,
        data: {
            type: "list",
            list
        }
    })

    const variants = cva("w-80 mx-2 flex flex-col", {
        variants: {
            dragging: {
                default: "border-2 border-transparent",
                over: "ring-2 opacity-30",
                overlay: "ring-2 ring-primary"
            }
        }
    })

    return (
        <Card
            className={variants({
                dragging: isDragging ? "over" : undefined
            })}
            ref={setNodeRef}
            style={{
                transition,
                transform: CSS.Transform.toString(transform),
                opacity: isDragging ? 0.5 : 1
            }}
        >
            <CardHeader className="flex flex-row items-center space-x-2 p-4">
                <Button
                    variant="ghost"
                    {...attributes}
                    {...listeners}
                    className="cursor-grab p-1 text-muted-foreground"
                >
                    <GripVertical size={20} />
                </Button>
                <ColumnActions id={list.id} title={list.name} />
            </CardHeader>
            <CardContent className="flex-grow overflow-y-auto p-2">
                <SortableContext
                    items={list.taskCard?.map((task) => task.id) || []}
                    strategy={verticalListSortingStrategy}
                >
                    {list.taskCard?.map((task) => (
                        <SortableTaskCard key={task.id} task={task} />
                    ))}
                </SortableContext>
            </CardContent>
        </Card>
    )
}
