import * as React from "react"

import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    // DropdownMenuShortcut,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu.tsx"
import {
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
    useSidebar,
} from "@/components/ui/sidebar.tsx"
import { useActiveWorkspace } from "@/features/worksapce-management/provider.tsx"
import { WorkspaceResponse } from "@/schemas/workspace.ts"
import { ChevronsUpDown, Plus } from "lucide-react"

import { AddWorkspaceForm } from "../../worksapce/add-workspace-form.tsx"

interface WorkspaceDisplay {
    id: string
    name: string
    logo: React.ElementType
    role: string
    originalData: WorkspaceResponse
}

export function WorkspaceSwitcher({
    workspaces,
}: {
    workspaces: WorkspaceDisplay[]
}) {
    const { isMobile } = useSidebar()
    const { activeWorkspace, setActiveWorkspace, setIsLoading } = useActiveWorkspace()
    const [dialogOpen, setDialogOpen] = React.useState(false)

    React.useEffect(() => {
        if (!activeWorkspace && workspaces.length > 0) {
            setIsLoading(true) 
            setActiveWorkspace(workspaces[0].originalData)
            setIsLoading(false) 
        }
    }, [workspaces, activeWorkspace, setActiveWorkspace, setIsLoading])

    const handleWorkspaceSwitch = async (workspace: WorkspaceResponse) => {
        setIsLoading(true)
        await new Promise(resolve => setTimeout(resolve, 1000)) 
        setActiveWorkspace(workspace)
        setIsLoading(false)
    }

    function handleDialog() {
        setDialogOpen(true)
    }

    const activeDisplay = workspaces.find(w => w.id === activeWorkspace?.id)

    if (!activeDisplay) {
        return (
            <>
                <SidebarMenu>
                    <SidebarMenuItem>
                        <button 
                            className="w-full p-2 text-left"
                            onClick={handleDialog}
                        >
                            Create Workspace
                        </button>
                    </SidebarMenuItem>
                </SidebarMenu>
                <AddWorkspaceForm 
                    onOpenChange={setDialogOpen} 
                    open={dialogOpen} 
                />
            </>
        )
    }

    return (
        <>
            <SidebarMenu>
                <SidebarMenuItem>
                    <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                            <SidebarMenuButton
                                className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground"
                                size="lg"
                            >
                                <div className="flex aspect-square size-8 items-center justify-center rounded-lg bg-sidebar-primary text-sidebar-primary-foreground">
                                    <activeDisplay.logo className="size-4" />
                                </div>
                                <div className="grid flex-1 text-left text-sm leading-tight">
                                    <span className="truncate font-semibold">
                                        {activeDisplay.name}
                                    </span>
                                    <span className="truncate text-xs">{activeDisplay.role}</span>
                                </div>
                                <ChevronsUpDown className="ml-auto" />
                            </SidebarMenuButton>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent
                            align="start"
                            className="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
                            side={isMobile ? "bottom" : "right"}
                            sideOffset={4}
                        >
                            <DropdownMenuLabel className="text-xs text-muted-foreground">
                                Workspaces
                            </DropdownMenuLabel>
                            {/* Add a scrollable container */}
                            <div className="max-h-[300px] overflow-y-auto">
                                {/* {workspaces.map((workspace, index) => ( */}
                                {workspaces.map((workspace) => (
                                    <DropdownMenuItem
                                        className="gap-2 p-2 cursor-pointer"
                                        key={workspace.id}
                                        // onClick={() => setActiveWorkspace(workspace.originalData)}
                                        onClick={() => handleWorkspaceSwitch(workspace.originalData)}
                                    >
                                        <div className="flex size-6 items-center justify-center rounded-sm border">
                                            <workspace.logo className="size-4 shrink-0" />
                                        </div>
                                        {workspace.name}
                                        {/* <DropdownMenuShortcut>⌘{index + 1}</DropdownMenuShortcut> */}
                                    </DropdownMenuItem>
                                ))}
                            </div>
                            <DropdownMenuSeparator />
                            <DropdownMenuItem 
                                className="gap-2 p-2"
                                onSelect={handleDialog}
                            >
                                <div className="flex size-6 items-center justify-center rounded-md border bg-background">
                                    <Plus className="size-4" />
                                </div>
                                <div className="font-medium text-muted-foreground cursor-pointer">Add a workspace</div>
                            </DropdownMenuItem>
                        </DropdownMenuContent>
                    </DropdownMenu>
                </SidebarMenuItem>
            </SidebarMenu>
            <AddWorkspaceForm 
                onOpenChange={setDialogOpen} 
                open={dialogOpen} 
            />
        </>
    )
}
