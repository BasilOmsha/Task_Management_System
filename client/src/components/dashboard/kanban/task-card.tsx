import { Card, CardContent } from '@/components/ui/card'
import { TaskCardResponse } from '@/schemas/task-card'

interface TaskCardProps {
    task: TaskCardResponse
}

export default function TaskCard({ task }: TaskCardProps) {
    return (

        <Card className="mb-5">
            <CardContent className="p-4">
                <h3 className="font-medium">{task.title}</h3>
                {task.description && <p className="text-sm text-muted-foreground mt-1">{task.description}</p>}
            </CardContent>
        </Card>
    )
}

