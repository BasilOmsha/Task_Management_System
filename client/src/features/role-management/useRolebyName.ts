import roleService, { RoleRes } from "@/api/role-services"
import { useQuery } from "@tanstack/react-query"

export function useRoleByName(roleName: string | null) {
    const query = useQuery<RoleRes[]>({
        queryKey: ['roles'],
        queryFn: () => roleService.getAll(),
        enabled: !!roleName
    })

    const matchingRole = query.data?.find(role => 
        role.name.toLowerCase() === roleName?.toLowerCase()
    )

    return {
        ...query,
        data: matchingRole,
        isLoading: query.isLoading,
        error: query.error
    }
}