import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"

export function WorkspaceInfoCardSkeleton() {
    return (
        <Card>
            <CardHeader>
                <CardTitle>Workspace Info</CardTitle>
            </CardHeader>
            <CardContent className="space-y-2">
                <div className="flex items-center">
                    <Skeleton className="font-semibold w-24" />
                    <Skeleton className="h-4 w-[200px]" />
                </div>
                <div className="flex items-center">
                    <Skeleton className="font-semibold w-24" />
                    <Skeleton className="h-4 w-[300px]" />
                </div>
                <div className="flex items-center">
                    <Skeleton className="font-semibold w-24" />
                    <Skeleton className="h-4 w-[150px]" />
                </div>
                <div className="flex items-center">
                    <Skeleton className="font-semibold w-24" />
                    <Skeleton className="h-4 w-[180px]" />
                </div>
                <div className="flex items-center">
                    <Skeleton className="font-semibold w-24" />
                    <Skeleton className="h-4 w-[40px]" />
                </div>
            </CardContent>
        </Card>
    )
}