import { useEffect, useState } from "react"

import { LogoutButton } from "@/components/form/logout-button.tsx"
import { Button } from "@/components/ui/button.tsx"
import { useLogout } from "@/features/user-management/useLogout.ts"
import { useUser } from "@/features/user-management/useUser.ts"
import { isTokenExpired } from "@/utils/jwt-decode.ts"

export function Home() {
    const [message, setMessage] = useState("")
    const { userData } = useUser()
    const token = localStorage.getItem('accessToken')
    const { mutate: logout } = useLogout()

    useEffect(() => {
        if (token && isTokenExpired(token)) {
            logout()
        }
    }, [token])

    const handleWelcome = () => {
        setMessage("Why did you?")
    }
    const handleCleanState = () => {
        setMessage("")
    }

    return (
        <div className="flex flex-col justify-center items-center gap-10 h-screen">
            <h1 className="text-2xl">Welcome!</h1>
            {message && <p>{message}</p>}
            {!message ? (
                <Button onClick={handleWelcome}>Do not click me</Button>
            ) : (
                <Button onClick={handleCleanState}>Undo the damage</Button>
            )}
            {userData && (
                <div>
                    <p>User ID: {userData.id}</p>
                    <p>Email: {userData.email}</p>
                </div>
            )}
            <LogoutButton />
        </div>
    )
}
