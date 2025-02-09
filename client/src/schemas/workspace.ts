import { z } from 'zod'

export const workspaceSchema = z.object({
    name: z.string()
        .min(1, { message: "Name is required" })
        .max(100, { message: "Name must be 100 characters or less" }),
    description: z.string().max(800, { message: "Description must be 800 characters or less" }),
    isPublic: z.boolean(),
})

export const workspaceUserSchema = z.object({
    id: z.string(),
    roleId: z.string(),
    roleName: z.string(),
    userId: z.string(),
    username: z.string()
})

export const workspaceUserDataSchema = z.object({
    workspaceId: z.string(),
    userId: z.string(),
    roleId: z.string()
})

export const workspaceResponseSchema = z.object({
    id: z.string(), 
    name: z.string(),
    creatorUserId: z.string(), 
    description: z.string(),
    isPublic: z.boolean(),
    createdAt: z.string(),
    updatedAt: z.string(),
    creatorUsername: z.string().nullable(),
    workspaceUsers: z.array(workspaceUserSchema).nullable()
})

export type WorkspaceData = z.infer<typeof workspaceSchema>
export type WorkspaceResponse = z.infer<typeof workspaceResponseSchema>
export type WorkspaceUser = z.infer<typeof workspaceUserSchema>
export type WorkspaceUserData = z.infer<typeof workspaceUserDataSchema>