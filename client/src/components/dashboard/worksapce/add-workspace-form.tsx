import { useState } from "react"
import { toast } from "react-hot-toast"
import { useNavigate } from "react-router-dom"

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
import { useCreateWorkspace } from "@/features/worksapce-management/useCreateWorksapce.ts"
import { WorkspaceData } from "@/schemas/workspace.ts"
import { ApiError } from "@/utils/errors.ts"

type FormErrors = {
    [key: string]: string
}

interface AddWorkspaceFormProps {
    open: boolean
    onOpenChange: (open: boolean) => void
}

export function AddWorkspaceForm({ open, onOpenChange }: AddWorkspaceFormProps) {
    const navigate = useNavigate()
    
    const [formData, setFormData] = useState<WorkspaceData>({
        name: "",
        description: "",
        isPublic: false
    })

    const [errors, setErrors] = useState<FormErrors>({})
    const [workspaceError, setWorkspaceError] = useState<string>('')

    const { isPending, mutate: createWorkspace } = useCreateWorkspace()

    const resetForm = () => {
        setFormData({
            name: "",
            description: "",
            isPublic: false
        })
        setErrors({})
        setWorkspaceError('')
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

    const validateField = (name: keyof WorkspaceData) => {
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
                if (formData.description.length > 800) {
                    fieldError = "Description must be 800 characters or less"
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
        data: WorkspaceData,
        setErrors: React.Dispatch<React.SetStateAction<FormErrors>>
    ) => {

        const newErrors: FormErrors = {}
        let isValid = true

        if (!formData.name) {
            newErrors.name = "Name is required"
            isValid = false
        } else if (formData.name.length > 100) {
            newErrors.name = "Name must be 100 characters or less"
            isValid = false
        }

        if (formData.description.length > 800) {
            newErrors.description = "Description must be 800 characters or less"
            isValid = false
        }

        setErrors(newErrors)
        return isValid
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        setErrors({})
        setWorkspaceError('') 
        
        if (!validateForm(formData, setErrors)) {
            return
        }

        createWorkspace(formData, {
            onSuccess: (response) => {
                toast.success('Workspace created successfully!')
                resetForm()
                onOpenChange(false)
                navigate(`/workspaces/${response.id}`)
            },
            onError: (error: ApiError) => {
                if (error.message) {
                    if (error.message.includes('Name already exists')) {
                        setErrors(prev => ({
                            ...prev, 
                            name: 'A workspace with this name already exists'
                        }))
                    } else if (error.message.includes('Permission denied')) {
                        setWorkspaceError('You do not have permission to create workspaces')
                    } else {
                        setWorkspaceError(error.message)
                    }
                } else {
                    setWorkspaceError('Failed to create workspace')
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
        setWorkspaceError('')
    }

    return (
        <Dialog onOpenChange={handleOpenChange} open={open}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Create Workspace</DialogTitle>
                    <DialogDescription>
                        Add a new workspace to organize your projects and collaborate with others.
                    </DialogDescription>
                </DialogHeader>

                <form className="space-y-4" onSubmit={handleSubmit}>
                    {workspaceError && (
                        <div className="p-3 text-sm text-red-500 bg-red-50 rounded-md">
                            {workspaceError}
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
                            placeholder="My Workspace"
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
                            placeholder="Describe your workspace..."
                            value={formData.description}
                        />
                        {errors.description && (
                            <p className="text-sm text-destructive">{errors.description}</p>
                        )}
                    </div>

                    <div className="flex items-center justify-between">
                        <div className="space-y-0.5">
                            <label className="text-sm font-medium">
                                Public Workspace
                            </label>
                            <p className="text-sm text-muted-foreground">
                                Allow anyone to view this workspace
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
                                Creating Workspace...
                            </>
                        ) : (
                            'Create Workspace'
                        )}
                    </Button>
                </form>
            </DialogContent>
        </Dialog>
    )
}