// useUser.ts
import type { UserResponse } from '@/schemas/user.ts'

import getUserInfo from '@/api/user-service.ts'
import { getUserInfoFromToken } from '@/utils/jwt-decode.ts'
import { useQuery } from '@tanstack/react-query'

import { useAuth } from './useAuth.ts'

export function useUser() {
    const { isAuthenticated } = useAuth()
    const basicUserInfo = getUserInfoFromToken()

    const { data: userData, isLoading, error } = useQuery<UserResponse, Error>({
        queryKey: ['user', basicUserInfo?.userId],
        queryFn: () => {
            if (!basicUserInfo?.userId) {
                throw new Error('No user ID available')
            }
            return getUserInfo.GetOne(basicUserInfo.userId)
        },
        enabled: isAuthenticated && !!basicUserInfo?.userId,
        staleTime: 5 * 60 * 1000,
    })

    return {
        ...basicUserInfo,
        userData,
        isLoading,
        error,
    }
}