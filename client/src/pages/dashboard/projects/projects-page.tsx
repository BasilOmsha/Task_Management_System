// src/pages/projects.tsx
import { useState } from "react"

import { CreateProjectDialog } from "@/components/dashboard/project/create-project-dialog.tsx"
import { ProjectCard } from "@/components/dashboard/project/project-card.tsx"
import { Button } from "@/components/ui/button.tsx"
import { useProjects } from "@/features/project-management/useProjects.ts"
import { useActiveWorkspace } from "@/features/worksapce-management/provider.tsx"
import { Plus } from "lucide-react"

export default function ProjectsPage() {
    const { activeWorkspace } = useActiveWorkspace()
    const [createDialogOpen, setCreateDialogOpen] = useState(false)
    
    // Get projects for the active workspace
    const { data: projects, isLoading, error } = useProjects(activeWorkspace?.id)

    // Debug logging
    console.log('Active Workspace:', activeWorkspace?.id)
    console.log('Projects Data:', projects)

    if (!activeWorkspace) {
        return (
            <div className="p-6 text-center text-muted-foreground">
                Select a workspace to view projects
            </div>
        )
    }

    if (isLoading) {
        return (
            <div className="p-6 text-center text-muted-foreground">
                Loading projects...
            </div>
        )
    }

    // if (error) {
    //     return (
    //         <div className="p-6 text-center text-destructive">
    //             Error loading projects: {error.message}
    //         </div>
    //     )
    // }

    // Ensure projects is an array
    const projectsList = Array.isArray(projects) ? projects : []

    return (
        <div className="p-6 space-y-6">
            <div className="flex items-center justify-between">
                <h1 className="text-3xl font-bold">Projects</h1>
                <Button onClick={() => setCreateDialogOpen(true)}>
                    <Plus className="mr-2 h-4 w-4" />
                    New Project
                </Button>
            </div>

            {projectsList.length > 0 ? (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {projectsList.map((project) => (
                        <ProjectCard key={project.id} project={project} />
                    ))}
                </div>
            ) : (
                <div className="text-center text-muted-foreground py-12">
                    No projects found. Create your first project!
                </div>
            )}

            <CreateProjectDialog
                onOpenChange={setCreateDialogOpen}
                open={createDialogOpen}
            />
        </div>
    )
}