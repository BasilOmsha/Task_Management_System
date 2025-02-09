import { z } from "zod"

// Matches AuthenticateDTO
export const loginSchema = z.object({
    email: z.string().email().nonempty('Email is required'),
    password: z.string().min(1, 'Password is required')
})

export const tokenResponseSchema = z.object({
    accessToken: z.string(),
    refreshToken: z.string()
}) 

// Use only one TokenResponse definition
export type TokenResponse = z.infer<typeof tokenResponseSchema>

export type LoginInput = z.infer<typeof loginSchema>