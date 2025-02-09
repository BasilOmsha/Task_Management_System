import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

interface WorkspaceUser {
    id: string
    username: string
    roleName: string
}

interface WorkspaceUsersCardProps {
    users: WorkspaceUser[]
}

export function WorkspaceUsersCard({ users }: WorkspaceUsersCardProps) {
    return (
        <Card>
            <CardHeader>
                <CardTitle>Workspace Users</CardTitle>
            </CardHeader>
            <CardContent>
                <ul className="list-disc pl-5 space-y-1">
                    {users.map(user => (
                        <li key={user.username}>
                            <span className="font-medium">{user.username}</span> - {user.roleName}
                        </li>
                    ))}
                </ul>
            </CardContent>
        </Card>
    )
}

