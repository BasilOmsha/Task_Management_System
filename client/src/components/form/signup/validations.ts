import { userSchema } from "@/schemas/user.ts"
import { z } from "zod"

import { FormData, FormErrors } from "./signup-form.tsx"

export const validateField = (name: keyof FormData, formData: FormData, setErrors: React.Dispatch<React.SetStateAction<FormErrors>>) => {
    try {
        if (name === 'confirmPassword') {
            if (formData.password !== formData.confirmPassword) {
                setErrors(prev => ({
                    ...prev,
                    confirmPassword: "Passwords don't match"
                }))
                return
            }
        }

        // Create a subset of the schema for the specific field
        const fieldSchema = (userSchema._def.schema).shape[name]
        // Validate just this field
        fieldSchema.parse(formData[name])
        
        // Clear error for this field if validation passes
        setErrors(prev => ({
            ...prev,
            [name]: ''
        }))
    } catch (error) {
        if (error instanceof z.ZodError) {
            // Set error just for this field
            setErrors(prev => ({
                ...prev,
                [name]: error.errors[0]?.message || ''
            }))
        }
    }
}

export const validateForm = (formData: FormData, setErrors: React.Dispatch<React.SetStateAction<FormErrors>>) => {
    try {
        // Validate main fields with Zod
        userSchema.parse(formData)
        
        setErrors({})
        return true
    } catch (error) {
        if (error instanceof z.ZodError) {
            const newErrors: FormErrors = {}
            error.errors.forEach((err) => {
                if (err.path[0]) {
                    newErrors[err.path[0] as keyof FormData] = err.message
                }
            })
            setErrors(newErrors)
        }
        return false
    }
}