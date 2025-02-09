import { useState } from "react"

import KanbanBoard from "@/components/dashboard/kanban/main-board.tsx"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useProjects } from "@/features/project-management/useProjects"
import { useActiveWorkspace } from "@/features/worksapce-management/provider"


function ProjectBoardPage() {
    // const projectBoardId = 'defbeb80-1ab3-41f4-ad55-71e7469761d0' // Get from route params or state
    const { activeWorkspace } = useActiveWorkspace()
    const [projectBoardId, setProjectBoardId] = useState<string | undefined>(undefined)

    const { data: projects, isLoading } = useProjects(activeWorkspace?.id)

    if (isLoading) {
        return <div>Loading projects...</div>
    }

    return (
        <div className="min-h-screen">
            <div className="container mx-auto py-6">
                <h1 className="text-2xl font-bold mb-6">Project Board</h1>
                <div className="w-64">
                    <Select onValueChange={setProjectBoardId} value={projectBoardId}>
                        <SelectTrigger>
                            <SelectValue placeholder="Select a Project" />
                        </SelectTrigger>
                        <SelectContent>
                            {projects?.map((project) => (
                                <SelectItem key={project.id} value={project.id}>
                                    {project.name}
                                </SelectItem>
                            ))}
                        </SelectContent>
                    </Select>
                </div>
                {projectBoardId && <KanbanBoard projectBoardId={projectBoardId} />}
            </div>
        </div>
    )
}

export default ProjectBoardPage