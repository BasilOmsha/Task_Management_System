import { z } from 'zod'

export const createTaskCardSchema = z.object({
    title: z.string()
        .min(1, { message: "Title is required" }),
    description: z.string(),
    listId: z.string().uuid(),
    position: z.number().int()
})

export const taskCardResponseSchema = z.object({
    id: z.string(),
    title: z.string(),
    description: z.string(),
    listId: z.string(),
    position: z.number(),
    createdAt: z.string(),
    updatedAt: z.string(),
    priorityId: z.string(),
    statusId: z.string(),
    dueDate: z.string().optional(),
    status: z.object({
        id: z.string().uuid(),
        name: z.string()
    }).nullable(),
    activities: z.array(z.object({
        id: z.string().uuid().optional(),
        description: z.string()
    })).nullable(),
    labels: z.array(z.object({
        id: z.string().uuid(),
        name: z.string()
    })).nullable()
})

export type CreateTaskCardData = z.infer<typeof createTaskCardSchema>
export type TaskCardResponse = z.infer<typeof taskCardResponseSchema>

