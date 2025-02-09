import toast from "react-hot-toast"
import { useNavigate } from "react-router-dom"

import authService from "@/api/auth-service.ts"
import { ApiError } from "@/utils/errors.ts"
import { useMutation, useQueryClient } from "@tanstack/react-query"

import { useToken } from "./useToken.ts"

export function useLogout() {
    const navigate = useNavigate()
    const { clearTokens } = useToken()
    const queryClient = useQueryClient()
 
    return useMutation({
        mutationKey: ['logout'],
        mutationFn: authService.logout,
        retry: (failureCount: number, error: ApiError) => {

            const noRetryConditions = [
                error.statusCode === 401,                 // Unauthorized
                error.statusCode === 429,                 // Rate limiting
                error.statusCode === 400,                 // Bad request
                error.statusCode === 422 

            ]
            if (noRetryConditions.some(condition => condition)) {
                return false
            }
            return failureCount < 3
        },
        onSuccess: () => {
            clearTokens()
            // Clear auth state immediately
            queryClient.setQueryData(['auth'], {
                isAuthenticated: false,
                userInfo: null
            })
            navigate('/login', { replace: true })
            toast.success('Logged out successfully!')
        },
        onError: (error: ApiError) => {
            toast.error(error.message || 'Logout failed')
            // Force logout on error anyway
            clearTokens()
            queryClient.setQueryData(['auth'], {
                isAuthenticated: false,
                userInfo: null
            })
            navigate('/login', { replace: true })
        }
    })
}