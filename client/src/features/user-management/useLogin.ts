import { useNavigate } from "react-router-dom"

import authService from "@/api/auth-service.ts"
import getUserInfo from "@/api/user-service.ts"
import { LoginInput, TokenResponse } from "@/schemas/auth.ts"
import { ApiError } from "@/utils/errors.ts"
import { getUserInfoFromToken } from "@/utils/jwt-decode.ts"
import { useMutation, useQueryClient } from "@tanstack/react-query"

import { useToken } from "./useToken.ts"

export function useLogin() {
    const { setTokens } = useToken()
    const navigate = useNavigate()
    const queryClient = useQueryClient()

    return useMutation<TokenResponse, ApiError, LoginInput>({
        mutationKey: ['login'],
        mutationFn: authService.login,
        //never retry for specific error conditions
        retry: (failureCount: number, error: ApiError) => {

            const noRetryConditions = [
                error.message.includes('Invalid Credentials'),
                error.statusCode === 429,                 // Rate limiting
                error.statusCode === 400,                 // Bad request
                error.statusCode === 422 

            ]
            if (noRetryConditions.some(condition => condition)) {
                return false
            }
            return failureCount < 3
        },
        onSuccess: async (data) => {
            setTokens(data.accessToken, data.refreshToken)

            // Pre-fetch user data to ensure it's in the cache
            try {
                const userInfo = getUserInfoFromToken()
                if (userInfo?.userId) {
                    // Pre-fetch and cache user data
                    const userData = await getUserInfo.GetOne(userInfo.userId)
                    queryClient.setQueryData(['user', userInfo.userId], userData)
                }
            } catch (error) {
                console.error('Failed to pre-fetch user data:', error)
            }
            // Invalidate auth state to reflect new login
            queryClient.invalidateQueries({ queryKey: ['auth'] })
            const location = history.state?.from || '/dashboard/overview'
            navigate(location)
        },
        onError: (error: ApiError) => {
            console.error('Login failed:', error)
        }
    })
}