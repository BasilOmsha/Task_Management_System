import { z } from 'zod'

import { taskCardResponseSchema } from './task-card.ts'

export const listSchema = z.object({
    name: z.string()
        .min(1, { message: "Name is required" })
        .max(60, { message: "Name must be 60 characters or less" }),
    projectBoardId: z.string(),
    position: z.number()
})

export const listResponseSchema = z.object({
    id: z.string(),
    name: z.string(),
    projectBoardId: z.string(),
    position: z.number(),
    taskCard: z.array(taskCardResponseSchema).nullable()
})

export type ListData = z.infer<typeof listSchema>
export type ListResponse = z.infer<typeof listResponseSchema>