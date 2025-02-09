import { useState } from "react"
import { toast } from "react-hot-toast"
// import { useNavigate } from "react-router-dom"

import { FormInput } from "@/components/form/signup/form-Input.tsx"
import { Button } from "@/components/ui/button.tsx"
import { Spinner } from "@/components/ui/spinner.tsx"
import { useLogin } from "@/features/user-management/useLogin.ts"
import { LoginInput } from "@/schemas/auth.ts"
import { ApiError } from "@/utils/errors.ts"

import { validateField, validateForm } from "./validation.ts"

import styles from "./styles/login-form.module.css"

export function LoginForm() {
    // const navigate = useNavigate()
    const [formData, setFormData] = useState<LoginInput>({
        email: '',
        password: ''
    })
    const [errors, setErrors] = useState<Record<string, string>>({})
    const [authError, setAuthError] = useState<string>('')
    const { isPending, mutate: login } = useLogin()

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        setErrors({})
        setAuthError('')
        
        // Validate form fields
        if (!validateForm(formData, setErrors)) {
            return
        }

        login(formData, {
            onSuccess: () => {
                toast.success('Logged in successfully!')
                console.log('Attempting to navigate to /home now...')
                // navigate('/dashboard/overview')
            },
            onError: (error: ApiError) => {
                if (error.message.includes('Invalid Credentials')) {
                    setAuthError('Invalid email or password. Try again.')
                } else {
                    toast.error(error.message || 'Login failed')
                }
            },
        })
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target
        
        setFormData(prev => ({ ...prev, [name]: value }))
        
        setErrors(prev => ({
            ...prev,
            [name]: '',  // Clear field-specific error
            ...(authError && { authError: '' })  // Clear auth error if it exists
        }))
    }

    const handleValidating = (e: React.FocusEvent<HTMLInputElement>) => {
        const { name } = e.target
        validateField(name as keyof LoginInput, formData, setErrors)
    }

    return (
        <form className={styles.form} onSubmit={handleSubmit}>
            {authError && (
                <div className="w-full p-3 mb-4 text-sm text-red-500 bg-red-50 rounded-md" data-login-error>
                    {authError}
                </div>
            )}

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
    
            <Button 
                className="w-full" 
                disabled={isPending}
                type="submit"
            >
                {isPending ? (
                    <>
                        <Spinner className="mr-2" size="sm" />
                        Logging in...
                    </>
                ) : (
                    'Log in'
                )}
            </Button>
        </form>
    )
}