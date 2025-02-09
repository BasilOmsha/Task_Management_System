import { Spinner } from "@/components/ui/spinner.tsx"
import { useLogout } from "@/features/user-management/useLogout.ts"

export function LogoutButton() {

    const { mutate: logout, isPending } = useLogout()

    const handleLogout = async (e: React.FormEvent) => {
        e.preventDefault()

        logout()

    }
    return (
        <button 
            disabled={isPending}
            onClick={handleLogout}
        >
            {isPending ? (
                <>
                    <Spinner className="mr-2 text-white" size="sm" />
                    Logging you out...
                </>
            ) : (
                'Log out'
            )}
        </button>
    )
}