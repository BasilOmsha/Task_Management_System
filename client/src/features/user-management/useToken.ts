const AUTH_KEYS = {
    ACCESS_TOKEN: 'accessToken',
    REFRESH_TOKEN: 'refreshToken'
} as const

export const useToken = () => {
    const setTokens = (accessToken: string | null, refreshToken: string | null) => {
        if (accessToken) {
            localStorage.setItem(AUTH_KEYS.ACCESS_TOKEN, accessToken)
        }
        if (refreshToken) {
            localStorage.setItem(AUTH_KEYS.REFRESH_TOKEN, refreshToken)
        }
    }

    const clearTokens = () => {
        localStorage.removeItem(AUTH_KEYS.ACCESS_TOKEN)
        localStorage.removeItem(AUTH_KEYS.REFRESH_TOKEN)
    }

    const getAccessToken = (): string | null => localStorage.getItem(AUTH_KEYS.ACCESS_TOKEN)
    const getRefreshToken = (): string | null => localStorage.getItem(AUTH_KEYS.REFRESH_TOKEN)

    return {
        setTokens,
        clearTokens,
        getAccessToken,
        getRefreshToken
    }
}