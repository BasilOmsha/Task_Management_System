import { ProjectData, ProjectResponse, projectResponseSchema } from "@/schemas/project.ts"
import { ApiError } from "@/utils/errors.ts"
import { z } from "zod"

import api from "./index.ts"

interface ProjectServices {
    AddOne: (projectData: ProjectData) => Promise<ProjectResponse>
    getProjectsByWorkspace: (workspaceId: string) => Promise<ProjectResponse[]>
}

const project: ProjectServices = {
    AddOne: async (projectData) => {
        const response = await api.post('/ProjectBoards', projectData)
        console.table('response: ', response.data)
        const validatedData = projectResponseSchema.safeParse(response.data)
        if (!validatedData.success) {
            throw new ApiError('Invalid data', 400)
        }
        
        return validatedData.data
    },

    getProjectsByWorkspace: async (workspaceId) => {
        const response = await api.get(`/ProjectBoards/workspace/${workspaceId}`)
        console.log('Raw API response:', response.data)
        
        // Handle the case where response.data is already an array
        const dataToValidate = Array.isArray(response.data) ? response.data : response.data.data
        
        if (!Array.isArray(dataToValidate)) {
            console.error('Expected array response:', dataToValidate)
            throw new ApiError('Invalid response format', 400)
        }

        const validatedData = z.array(projectResponseSchema).safeParse(dataToValidate)
        
        if (!validatedData.success) {
            console.error('Validation errors:', validatedData.error.errors)
            throw new ApiError('Invalid data', 400)
        }

        return validatedData.data
    }

}

export default project
export type { ProjectServices }