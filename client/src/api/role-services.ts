import { ApiError } from "@/utils/errors"
import { z } from "zod"

import api from "."

// type RoleReq = {
//     name: string
// }

const roleRes = z.object({
    id: z.string(),
    name: z.string(),
    description: z.string()
})

const rolesArraySchema = z.array(roleRes)

export type RoleRes = z.infer<typeof roleRes>

interface RoleService {
    getAll: () => Promise<RoleRes[]>
}

const roleService: RoleService = {
    getAll: async () => {
        const response = await api.get(`/Roles`)
        const validatedData = rolesArraySchema.safeParse(response.data.data)

        if (!validatedData.success) {
            throw new ApiError("Invalid data", 400)
        }

        return validatedData.data
    }
}

export default roleService
export type { RoleService }
