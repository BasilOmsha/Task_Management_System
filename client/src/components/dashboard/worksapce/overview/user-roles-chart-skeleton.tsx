import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"

export function UserRolesChartSkeleton() {
    return (
        <Card>
            <CardHeader>
                <CardTitle>User Roles Distribution</CardTitle>
            </CardHeader>
            <CardContent>
                <div className="flex items-center justify-center h-[250px]">
                    <Skeleton className="h-[160px] w-[160px] rounded-full" />
                </div>
            </CardContent>
        </Card>
    )
}