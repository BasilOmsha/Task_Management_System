import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"

export function WorkspaceUsersCardSkeleton() {
    return (
        <Card>
            <CardHeader>
                <CardTitle>Workspace Users</CardTitle>
            </CardHeader>
            <CardContent>
                <ul className="space-y-3">
                    {[1, 2, 3, 4].map((i) => (
                        <li className="flex items-center space-x-2" key={i}>
                            <Skeleton className="h-4 w-[140px]" />
                            <Skeleton className="h-4 w-[80px]" />
                        </li>
                    ))}
                </ul>
            </CardContent>
        </Card>
    )
}