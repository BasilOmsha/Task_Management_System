import { useState } from "react"
import { toast } from "react-hot-toast"

import { Button } from "@/components/ui/button.tsx"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog.tsx"
import { Input } from "@/components/ui/input.tsx"
import { Spinner } from "@/components/ui/spinner.tsx"
import { Switch } from "@/components/ui/switch.tsx"
import { Textarea } from "@/components/ui/textarea.tsx"
import { useCreateProject } from "@/features/project-management/useCreateProject.ts"
import { useActiveWorkspace } from "@/features/worksapce-management/provider.tsx"
import { ProjectData } from "@/schemas/project.ts"
import { ApiError } from "@/utils/errors.ts"
import { useQueryClient } from "@tanstack/react-query"

type FormErrors = {
    [key: string]: string
}

interface CreateProjectDialogProps {
    open: boolean
    onOpenChange: (open: boolean) => void
}

export function CreateProjectDialog({ open, onOpenChange }: CreateProjectDialogProps) {
    const { activeWorkspace } = useActiveWorkspace()
    const [formData, setFormData] = useState<Omit<ProjectData, 'workspaceId'>>({
        name: "",
        description: "",
        isPublic: false
    })

    const [errors, setErrors] = useState<FormErrors>({})
    const [projectError, setProjectError] = useState<string>('')

    const { isPending, mutate: createProject } = useCreateProject()

    const queryClient = useQueryClient()

    const resetForm = () => {
        setFormData({
            name: "",
            description: "",
            isPublic: false
        })
        setErrors({})
        setProjectError('')
    }

    const handleOpenChange = (open: boolean) => {
        if (!open) {
            resetForm()
        }
        onOpenChange(open)
    }

    const handleTogglePublic = (checked: boolean) => {
        setFormData(prev => ({
            ...prev,
            isPublic: checked
        }))
    }

    const validateField = (name: keyof Omit<ProjectData, 'workspaceId'>) => {
        let fieldError = ""

        switch (name) {
            case "name":
                if (!formData.name) {
                    fieldError = "Name is required"
                } else if (formData.name.length > 100) {
                    fieldError = "Name must be 100 characters or less"
                }
                break
            case "description":
                if (formData.description.length > 500) {
                    fieldError = "Description must be 500 characters or less"
                }
                break
        }

        setErrors(prev => ({
            ...prev,
            [name]: fieldError
        }))

        return !fieldError
    }

    const validateForm = (
        data: Omit<ProjectData, 'workspaceId'>,
        setErrors: React.Dispatch<React.SetStateAction<FormErrors>>
    ) => {
        const newErrors: FormErrors = {}
        let isValid = true

        if (!data.name) {
            newErrors.name = "Name is required"
            isValid = false
        } else if (data.name.length > 100) {
            newErrors.name = "Name must be 100 characters or less"
            isValid = false
        }

        if (data.description.length > 500) {
            newErrors.description = "Description must be 500 characters or less"
            isValid = false
        }

        setErrors(newErrors)
        return isValid
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        setErrors({})
        setProjectError('')

        if (!activeWorkspace) {
            setProjectError("No workspace selected")
            return
        }

        if (!validateForm(formData, setErrors)) {
            return
        }

        createProject({
            ...formData,
            workspaceId: activeWorkspace.id
        }, {
            onSuccess: () => {
                toast.success('Project created successfully!')
                queryClient.invalidateQueries({
                    queryKey: ['projects', activeWorkspace.id],
                })
                resetForm()
                onOpenChange(false)
            },
            onError: (error: ApiError) => {
                if (error.message) {
                    if (error.message.includes('Name already exists')) {
                        setErrors(prev => ({
                            ...prev,
                            name: 'A project with this name already exists'
                        }))
                    } else if (error.message.includes('Permission denied')) {
                        setProjectError('You do not have permission to create projects')
                    } else {
                        setProjectError(error.message)
                    }
                } else {
                    setProjectError('Failed to create project')
                }
            },
        })
    }

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    ) => {
        const { name, value } = e.target

        setFormData(prev => ({
            ...prev,
            [name]: value
        }))

        // Clear errors when user starts typing
        if (errors[name]) {
            setErrors(prev => ({
                ...prev,
                [name]: ''
            }))
        }
        setProjectError('')
    }

    return (
        <Dialog onOpenChange={handleOpenChange} open={open}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Create Project</DialogTitle>
                    <DialogDescription>
                        Create a new project in the current workspace.
                    </DialogDescription>
                </DialogHeader>

                <form className="space-y-4" onSubmit={handleSubmit}>
                    {projectError && (
                        <div className="p-3 text-sm text-red-500 bg-red-50 rounded-md">
                            {projectError}
                        </div>
                    )}
                    <div className="space-y-2">
                        <label className="text-sm font-medium">
                            Name
                        </label>
                        <Input
                            name="name"
                            onBlur={() => validateField("name")}
                            onChange={handleChange}
                            placeholder="My Project"
                            value={formData.name}
                        />
                        {errors.name && (
                            <p className="text-sm text-destructive">{errors.name}</p>
                        )}
                    </div>

                    <div className="space-y-2">
                        <label className="text-sm font-medium">
                            Description
                        </label>
                        <Textarea
                            name="description"
                            onBlur={() => validateField("description")}
                            onChange={handleChange}
                            placeholder="Describe your project..."
                            value={formData.description}
                        />
                        {errors.description && (
                            <p className="text-sm text-destructive">{errors.description}</p>
                        )}
                    </div>

                    <div className="flex items-center justify-between">
                        <div className="space-y-0.5">
                            <label className="text-sm font-medium">
                                Public Project
                            </label>
                            <p className="text-sm text-muted-foreground">
                                Allow anyone to view this project
                            </p>
                        </div>
                        <Switch
                            checked={formData.isPublic}
                            onCheckedChange={handleTogglePublic}
                        />
                    </div>

                    <Button 
                        className="w-full" 
                        disabled={isPending}
                        type="submit"
                    >
                        {isPending ? (
                            <>
                                <Spinner className="mr-2" size="sm" />
                                Creating Project...
                            </>
                        ) : (
                            'Create Project'
                        )}
                    </Button>
                </form>
            </DialogContent>
        </Dialog>
    )
}