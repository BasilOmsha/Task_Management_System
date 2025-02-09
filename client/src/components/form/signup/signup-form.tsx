import { useState } from "react"
import { toast } from "react-hot-toast"
import { useNavigate } from "react-router-dom"

import { useRegister } from "@/features/user-management/useRegister.ts"
import { userSchema } from "@/schemas/user.ts"
import { ApiError } from "@/utils/errors.ts"
import { fieldSanitizers, sanitizeInput } from "@/utils/sanitization.ts"
import { z } from "zod"

import { FormInput } from "./form-Input.tsx"
import { validateField, validateForm } from "./validations.ts"

import { Button } from "../../ui/button.tsx"
import { Spinner } from "../../ui/spinner.tsx"

import styles from "./styles/signup-form.module.css"

export type FormData = z.infer<typeof userSchema>


export type FormErrors = {
    [key: string]: string
}

export function SignupForm() {
    const navigate = useNavigate()
    const [formData, setFormData] = useState<FormData>({
        confirmPassword: '',
        email: '',
        firstname: '',
        lastname: '',
        password: '',
        username: ''
    })

    const [errors, setErrors] = useState<FormErrors>({})
    const { isPending, mutate: registerUser } = useRegister()

    

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        
        if (!validateForm(formData, setErrors)) {return}
    
        registerUser(formData, {
            onSuccess: () => {
                toast.success('Account created successfully!')
                navigate('/')
            },
            onError: (error: ApiError) => {
                // Check if the error message is from the backend
                if (error.message) {
                    // Handle specific field errors
                    if (error.message.includes('Email already exists')) {
                        setErrors(prev => ({...prev, email: 'Email already exists'}))
                    } else if (error.message.includes('Username already exists')) {
                        setErrors(prev => ({...prev, username: 'Username already exists'}))
                    } else {
                        // Show generic error as toast
                        toast.error(error.message)
                    }
                } else {
                    // Fallback error message
                    toast.error('An error occurred during registration')
                }
            },
        })
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target
        
        const sanitizer = fieldSanitizers[name] || sanitizeInput
        const sanitizedValue = sanitizer(value)

        setFormData(prev => ({
            ...prev,
            [name]: sanitizedValue
        }))

        // Clear error when user starts typing
        if (errors[name]) {
            setErrors(prev => ({
                ...prev,
                [name]: ''
            }))
        }
    }

    const handleValidating= (e: React.FocusEvent<HTMLInputElement>) => {
        const { name } = e.target
        validateField(name as keyof FormData, formData, setErrors)
    }

    return (
        <form className={styles.form} onSubmit={handleSubmit}>
            <div className={styles.nameFields}>
                <FormInput
                    error={errors.firstname}
                    label="First Name"
                    name="firstname"
                    onBlur={handleValidating}
                    onChange={handleChange}
                    placeholder="John"
                    value={formData.firstname}
                />
                <FormInput
                    error={errors.lastname}
                    label="Last Name"
                    name="lastname"
                    onBlur={handleValidating}
                    onChange={handleChange}
                    placeholder="Doe"
                    value={formData.lastname}
                />
            </div>

            <FormInput
                error={errors.username}
                label="Username"
                name="username"
                onBlur={handleValidating}
                onChange={handleChange}
                placeholder="johndoe"
                value={formData.username}
            />

            <FormInput
                error={errors.email}
                label="Email"
                name="email"
                onBlur={handleValidating}
                onChange={handleChange}
                placeholder="john@example.com"
                type="email"
                value={formData.email}
            />

            <FormInput
                error={errors.password}
                label="Password"
                name="password"
                onBlur={handleValidating}
                onChange={handleChange}
                type="password"
                value={formData.password}
            />

            <FormInput
                error={errors.confirmPassword}
                label="Confirm Password"
                name="confirmPassword"
                onBlur={handleValidating}
                onChange={handleChange}
                type="password"
                value={formData.confirmPassword}
            />

            <Button 
                className="w-full" 
                disabled={isPending}
                type="submit"
            >
                {isPending ? (
                    <>
                        <Spinner className="mr-2" size="sm" />
                        Creating Account...
                    </>
                ) : (
                    'Create Account'
                )}
            </Button>
        </form>
    )
}