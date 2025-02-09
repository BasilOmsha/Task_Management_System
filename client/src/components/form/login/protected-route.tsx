import { Navigate, Outlet, useLocation } from 'react-router-dom'

import { useAuth } from '@/features/user-management/useAuth.ts'

export function ProtectedRoute() {
    const { isAuthenticated } = useAuth()
    const location = useLocation()
    
    return isAuthenticated ? 
        <Outlet /> : 
        <Navigate replace state={{ from: location }} to="/login" />
}