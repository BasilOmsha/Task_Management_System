import { UserRolesChart } from "@/components/dashboard/worksapce/overview/user-roles-chart"
import { UserRolesChartSkeleton } from "@/components/dashboard/worksapce/overview/user-roles-chart-skeleton"
import { WorkspaceInfoCard } from "@/components/dashboard/worksapce/overview/workspace-info-card"
import { WorkspaceInfoCardSkeleton } from "@/components/dashboard/worksapce/overview/workspace-info-card-skeleton"
import { WorkspaceUsersCard } from "@/components/dashboard/worksapce/overview/workspace-user-card"
import { WorkspaceUsersCardSkeleton } from "@/components/dashboard/worksapce/overview/workspace-user-card-skeleton"
import { useActiveWorkspace } from "@/features/worksapce-management/provider"


export function Overview() {
    const { activeWorkspace, isLoading } = useActiveWorkspace()

    if (!activeWorkspace) {
        return <div className="p-6 text-center text-gray-500">No workspace selected</div>
    }

    const roleData = activeWorkspace.workspaceUsers?.reduce((acc, user) => {
        acc[user.roleName] = (acc[user.roleName] || 0) + 1
        return acc
    }, {} as Record<string, number>) || {}

    const pieChartData = Object.entries(roleData).map(([name, value]) => ({ name, value }))

    return (
        <div className="p-6 space-y-6">
            <h1 className="text-3xl font-bold">Overview</h1>
      
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {isLoading ? (
                    <>
                        <WorkspaceInfoCardSkeleton />
                        <UserRolesChartSkeleton />
                        <WorkspaceUsersCardSkeleton />
                    </>
                ) : (
                    <>
                        <WorkspaceInfoCard
                            createdAt={activeWorkspace.createdAt}
                            creatorUsername={activeWorkspace.creatorUsername || "Unknown"}
                            description={activeWorkspace.description}
                            isPublic={activeWorkspace.isPublic}
                            name={activeWorkspace.name}
                        />
                        <UserRolesChart data={pieChartData} />
                        <WorkspaceUsersCard users={activeWorkspace.workspaceUsers || []} />
                    </>
                )}
            </div>
        </div>
    )
}

