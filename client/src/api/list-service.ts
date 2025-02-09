import { ListData, ListResponse, listResponseSchema } from "@/schemas/list"
import { ApiError } from "@/utils/errors"
import { z } from "zod"

import api from "./index.ts"

interface ListServices {
    getByProjectBoard: (projectBoardId: string) => Promise<ListResponse[]>;
    addOne: (listData: ListData) => Promise<ListResponse>;
    updatePosition: (listId: string, newPosition: number) => Promise<void>;
    updateMultiplePositions: (updates: { listId: string; newPosition: number }[]) => Promise<void>;
}

const list: ListServices = {
    getByProjectBoard: async (projectBoardId) => {
        try {
            const response = await api.get(`/AppLists/projectBoard/${projectBoardId}`)
            console.log('Get Lists Response:', response.data) 
            
            const validatedData = z.array(listResponseSchema).safeParse(response.data.data)
            if (!validatedData.success) {
                console.error('Validation errors:', validatedData.error.errors)
                throw new ApiError('Invalid data', 400)
            }
            
            return validatedData.data
        } catch (error) {
            console.error('Get lists error:', error)
            throw error
        }
    },

    addOne: async (listData) => {
        try {
            const response = await api.post('/AppLists', listData)
            console.log('API Response:', response.data) 
            
            const validatedData = listResponseSchema.safeParse(response.data.data)
            if (!validatedData.success) {
                console.error('Validation errors:', validatedData.error.errors) 
                throw new ApiError('Invalid data', 400)
            }
            
            return validatedData.data
        } catch (error) {
            console.error('Service error:', error)
            throw error
        }
    },

    updatePosition: async (listId, newPosition) => {
        await api.put(`/AppLists/${listId}/position`, newPosition)
    },

    updateMultiplePositions: async (updates) => {
        await api.put('/AppLists/updatePositions', { listPositions: updates })
    }
}

export default list
export type { ListServices }