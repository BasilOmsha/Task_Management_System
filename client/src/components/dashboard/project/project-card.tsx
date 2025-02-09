import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card.tsx"
import { ProjectResponse } from "@/schemas/project.ts"
import { Globe, Lock } from "lucide-react"

interface ProjectCardProps {
    project: ProjectResponse
}

export function ProjectCard({ project }: ProjectCardProps) {
    return (
        <Card className="hover:bg-accent/50 transition-colors cursor-pointer">
            <CardHeader>
                <div className="flex items-center justify-between">
                    <CardTitle className="text-xl">{project.name}</CardTitle>
                    {project.isPublic ? (
                        <Globe className="h-4 w-4 text-muted-foreground" />
                    ) : (
                        <Lock className="h-4 w-4 text-muted-foreground" />
                    )}
                </div>
                <CardDescription>
                    Created {new Date(project.createdAt).toLocaleDateString()}
                </CardDescription>
            </CardHeader>
            <CardContent>
                <p className="text-sm text-muted-foreground">
                    {project.description || 'No description provided'}
                </p>
            </CardContent>
        </Card>
    )
}