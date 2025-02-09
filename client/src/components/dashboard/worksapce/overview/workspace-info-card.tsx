import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card.tsx"

interface WorkspaceInfoProps {
    name: string
    description: string
    creatorUsername: string
    createdAt: string
    isPublic: boolean
}

export function WorkspaceInfoCard({ name, description, creatorUsername, createdAt, isPublic }: WorkspaceInfoProps) {
    return (
        <Card>
            <CardHeader>
                <CardTitle>Workspace Info</CardTitle>
            </CardHeader>
            <CardContent className="space-y-2">
                <p><span className="font-semibold">Name:</span> {name}</p>
                <p><span className="font-semibold">Description:</span> {description || 'No description'}</p>
                <p><span className="font-semibold">Created by:</span> {creatorUsername}</p>
                <p><span className="font-semibold">Created at:</span> {new Date(createdAt).toLocaleString()}</p>
                <p><span className="font-semibold">Public:</span> {isPublic ? 'Yes' : 'No'}</p>
            </CardContent>
        </Card>
    )
}

