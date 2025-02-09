import {
    CreateTaskCardData,
    TaskCardResponse,
    taskCardResponseSchema
} from "@/schemas/task-card.ts"
import { ApiError } from "@/utils/errors.ts"
import { z } from "zod"

import api from "./index"

interface TaskCardServices {
    getByList: (listId: string) => Promise<TaskCardResponse[]>
    addOne: (taskCardData: CreateTaskCardData) => Promise<TaskCardResponse>
    updatePosition: (
        taskCardId: string,
        update: { listId: string; position: number }
    ) => Promise<void>
    updateMultiplePositions: (
        updates: { taskCardId: string; listId: string; position: number }[]
    ) => Promise<void>
}

const taskCard: TaskCardServices = {
    getByList: async (listId) => {
        const response = await api.get(`/TaskCard/lists/${listId}`)
        const validatedData = z.array(taskCardResponseSchema).safeParse(response.data.data)

        if (!validatedData.success) {
            throw new ApiError("Invalid data", 400)
        }

        return validatedData.data
    },

    addOne: async (taskCardData) => {
        const response = await api.post("/TaskCards", taskCardData)
        const validatedData = taskCardResponseSchema.safeParse(response.data.data)

        if (!validatedData.success) {
            throw new ApiError("Invalid data", 400)
        }

        return validatedData.data
    },

    updatePosition: async (taskCardId, update) => {
        await api.put(`/TaskCards/${taskCardId}/position`, update)
    },

    updateMultiplePositions: async (updates) => {
        await api.put("/TaskCards/updatePositions", updates)
    }
}

export default taskCard
export type { TaskCardServices }
