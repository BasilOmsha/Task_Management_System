import React, { createContext, useContext, useState } from 'react'

import { WorkspaceResponse } from '@/schemas/workspace.ts'

interface WorkspaceContextType {
    activeWorkspace: WorkspaceResponse | null
    setActiveWorkspace: (workspace: WorkspaceResponse | null) => void,
    isLoading: boolean,
    setIsLoading: (isLoading: boolean) => void
}

const WorkspaceContext = createContext<WorkspaceContextType | undefined>(undefined)

export function WorkspaceProvider({ children }: { children: React.ReactNode }) {
    const [activeWorkspace, setActiveWorkspace] = useState<WorkspaceResponse | null>(null)
    const [isLoading, setIsLoading] = useState(false)  // Add this line

    const updateActiveWorkspace = async (workspaceData: WorkspaceResponse | null) => {
        setIsLoading(true)
        try {
            setActiveWorkspace(workspaceData)
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <WorkspaceContext.Provider value={{ 
            activeWorkspace, 
            setActiveWorkspace: updateActiveWorkspace, 
            isLoading,        // Now it's dynamic
            setIsLoading     // Add this to allow updating loading state
        }}>
            {children}
        </WorkspaceContext.Provider>
    )
}


export function useActiveWorkspace() {
    const context = useContext(WorkspaceContext)
    if (context === undefined) {
        throw new Error('useActiveWorkspace must be used within a WorkspaceProvider')
    }
    return context
}