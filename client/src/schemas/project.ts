import { z } from 'zod'

export const projectSchema = z.object({
    workspaceId: z.string(),
    name: z.string()
        .min(1, { message: "Name is required" })
        .max(100, { message: "Name must be 100 characters or less" }),
    description: z.string()
        .max(500, { message: "Description must be 500 characters or less" })
        .nullish()
        .transform(val => val || ''), // Transform null/undefined to empty string
    isPublic: z.boolean(),
})

export const projectResponseSchema = z.object({
    id: z.string(),
    workspaceId: z.string(),
    name: z.string(),
    description: z.string().nullish().transform(val => val || ''),
    isPublic: z.boolean(),
    createdAt: z.string(),
    updatedAt: z.string(),
    creatorUserId: z.string(),
})

export type ProjectData = z.infer<typeof projectSchema>
export type ProjectResponse = z.infer<typeof projectResponseSchema>