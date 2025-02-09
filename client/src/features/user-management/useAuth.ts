import { useCallback, useState } from 'react'

import authService from "@/api/auth-service.ts"
import { isTokenExpired } from "@/utils/jwt-decode.ts"
import { useQuery, useQueryClient } from "@tanstack/react-query"

import { useLogout } from "./useLogout.ts"
import { useToken } from "./useToken.ts"

interface AuthState {
    isAuthenticated: boolean;
    // userInfo: { userId: string; email: string; } | null;
}

const TOKEN_CHECK_INTERVAL = 3000000 // 50 minutes

export function useAuth() {
    const queryClient = useQueryClient()
    const { getAccessToken, getRefreshToken, setTokens } = useToken()
    const { mutate: logout } = useLogout()
    const [showRefreshPrompt, setShowRefreshPrompt] = useState(false)
    
    const checkAuthState = useCallback(async (): Promise<AuthState> => {
        
        console.log('Token check')
        const token = getAccessToken()
        console.log('Token expired:', isTokenExpired(token))
        
        if (!token) {
            return { isAuthenticated: false }
        }

        if (isTokenExpired(token)) {
            const refreshToken = getRefreshToken()
            if (!refreshToken) {
                logout()
                return { isAuthenticated: false }
            }
            setShowRefreshPrompt(true)
            return { isAuthenticated: false }
        }

        return {
            isAuthenticated: true,
            // userInfo: getUserInfoFromToken(),
        }
    }, [getAccessToken, getRefreshToken, logout])

    const { data: authState } = useQuery<AuthState, Error>({
        queryKey: ['auth'],
        queryFn: checkAuthState,
        refetchInterval: TOKEN_CHECK_INTERVAL,
        refetchIntervalInBackground: true,
        refetchOnWindowFocus: true,
        refetchOnMount: true
    })

    const refetch = useCallback(() => {
        queryClient.invalidateQueries({ queryKey: ['auth'] })
    }, [queryClient])

    const handleRefreshResponse = useCallback(async (stayLoggedIn: boolean) => {
        setShowRefreshPrompt(false)
        if (stayLoggedIn) {
            try {
                const refreshToken = getRefreshToken()
                const accessToken = getAccessToken()
                if (!refreshToken || !accessToken) {
                    throw new Error("Missing tokens for refresh")
                }
                const newTokens = await authService.refresh({
                    refreshToken,
                    accessToken
                })
                setTokens(newTokens.accessToken, newTokens.refreshToken)
                refetch()
            } catch (error) {
                console.error("Failed to refresh token:", error)
                logout()
            }
        } else {
            logout()
        }
    }, [getRefreshToken, getAccessToken, setTokens, refetch, logout])

    return {
        ...(authState ?? { isAuthenticated: false }),
        showRefreshPrompt,
        handleRefreshResponse
    }
}

