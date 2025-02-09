
import { WorkspaceData, WorkspaceResponse, WorkspaceUser, WorkspaceUserData, workspaceResponseSchema, workspaceUserDataSchema, workspaceUserSchema } from "@/schemas/workspace.ts"
import { ApiError } from "@/utils/errors.ts"
import { z } from "zod"

import api from "./index.ts"

interface WorkspaceServices {
    AddOne: (workspaceData: WorkspaceData) => Promise<WorkspaceResponse>;
    GetOne: (id: string) => Promise<WorkspaceResponse>;
    GetAllByUser: () => Promise<WorkspaceResponse[]>;
    GetWorkspaceMember: (id: string) => Promise<WorkspaceUser>;
    AddWorkspaceMember: (workspaceUserData: WorkspaceUserData) => Promise<WorkspaceUserData>;
    GetAllUsersByWorkSpace: (id: string) => Promise<WorkspaceUserData[]>;
}

const workspace: WorkspaceServices = {

    AddOne: async (workspaceData) => {

        const response = await api.post('/Workspaces', workspaceData)

        const validatedData = workspaceResponseSchema.safeParse(response.data)
        if (!validatedData.success) {
            console.error('Validation errors:', validatedData.error.errors)
            throw new ApiError('Invalid data', 400)
        }
        
        return validatedData.data
    },
    
    GetOne: async (id) => {
        await new Promise(resolve => setTimeout(resolve, 1000))
        const response = await api.get(`/Workspaces/${id}`)
        const validatedData = workspaceResponseSchema.safeParse(response.data.data)
        
        if (!validatedData.success) {
            throw new ApiError('Invalid data', 400)
        }
        
        return validatedData.data
    },

    GetAllByUser: async () => {
        await new Promise(resolve => setTimeout(resolve, 1000))
        const response = await api.get('/Workspaces')

        const validatedData = z.array(workspaceResponseSchema).safeParse(response.data.data)
        
        if (!validatedData.success) {
            throw new ApiError('Invalid data', 400)
        }

        return validatedData.data
    },
    GetWorkspaceMember: async (id) => {
        const response = await api.get(`/Workspaces/${id}/members`)
        const validatedData = workspaceUserSchema.safeParse(response.data.data)
        
        if (!validatedData.success) {
            throw new ApiError('Invalid data', 400)
        }
        
        return validatedData.data
    },
    AddWorkspaceMember: async (WorkspaceUserData) => {
        const response = await api.post(`/workspaceUsers`, WorkspaceUserData)
        const validatedData = workspaceUserDataSchema.safeParse(response.data.data)
        
        if (!validatedData.success) {
            throw new ApiError(response.data, 400)
        }
        
        return validatedData.data
    },
    GetAllUsersByWorkSpace: async (id) => {
        const response = await api.get(`/workspaceUsers/${id}/members`)

        const validatedData = z.array(workspaceUserDataSchema).safeParse(response.data.data)

        if (!validatedData.success) {
            throw new ApiError('Invalid data', 400)
        }
        return validatedData.data
    }
}

export default workspace
export type { WorkspaceServices }