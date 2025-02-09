import { z } from "zod"

// Validation patterns matching backend requirements
const VALIDATION = {
    MIN_LENGTHS: {
        firstname: 2,
        lastname: 2,
        password: 8,
        username: 4
    },
    PASSWORD_PATTERN: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$/,
    USERNAME_PATTERN: /^[a-zA-Z0-9]*$/
}

// Base user fields that are common to both input and response
const userBaseSchema = z.object({
    firstname: z.string().min(VALIDATION.MIN_LENGTHS.firstname, 'First name must be at least 2 characters long').max(100),
    lastname: z.string().min(VALIDATION.MIN_LENGTHS.lastname, 'Last name must contain at least 2 characters').max(100),
    username: z.string()
        .min(VALIDATION.MIN_LENGTHS.username, 'Username must contain at least 4 characters)')
        .max(100)
        .regex(VALIDATION.USERNAME_PATTERN, 'Username can only contain letters and numbers'),
    email: z.string().email().max(100),
})

// Input schema extends base and adds password fields
export const userSchema = userBaseSchema.extend({
    confirmPassword: z.string(),
    password: z.string()
        .min(VALIDATION.MIN_LENGTHS.password, 'Password must contain at least 8 characters')
        .regex(VALIDATION.PASSWORD_PATTERN, 'Password must include uppercase, lowercase, number and special character')
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"]
})

// Response schema matches GetUserInfoDTO
export const userResponseSchema = z.object({
    id: z.string(),
    firstName: z.string(), // Note the capital N to match backend
    lastName: z.string(),  // Note the capital N to match backend
    username: z.string(),
    email: z.string(),
    createdAt: z.string(),
    updatedAt: z.string()
})
export type UserData = z.infer<typeof userSchema>
export type UserResponse = z.infer<typeof userResponseSchema>