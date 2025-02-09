import { LoginInput, loginSchema } from "@/schemas/auth.ts"
import { z } from "zod"

interface FormErrors {
    [key: string]: string
}

export const validateField = (
    name: keyof LoginInput,
    formData: LoginInput,
    setErrors: React.Dispatch<React.SetStateAction<FormErrors>>
) => {
    try {
        const fieldSchema = loginSchema.shape[name]
        fieldSchema.parse(formData[name])
        setErrors(prev => ({ ...prev, [name]: '' }))
    } catch (error) {
        if (error instanceof z.ZodError) {
            setErrors(prev => ({
                ...prev,
                [name]: error.errors[0]?.message || ''
            }))
        }
    }
}

export const validateForm = (
    formData: LoginInput,
    setErrors: React.Dispatch<React.SetStateAction<FormErrors>>
): boolean => {
    let isValid = true
    const newErrors: FormErrors = {}

    try {
        loginSchema.parse(formData)
    } catch (error) {
        if (error instanceof z.ZodError) {
            error.errors.forEach(err => {
                const path = err.path[0] as keyof LoginInput
                newErrors[path] = err.message
                isValid = false
            })
        }
    }

    setErrors(newErrors)
    return isValid
}