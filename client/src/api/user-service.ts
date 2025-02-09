import { type UserData, type UserResponse, userResponseSchema } from "@/schemas/user.ts"
import { ApiError } from "@/utils/errors.ts"

import api from "./index.ts"

interface UserService {
    AddOne: (userData: UserData) => Promise<UserResponse>;
    GetOne: (id: string) => Promise<UserResponse>;
    GetUserByEmail: (email: string) => Promise<UserResponse>;
}

const userService: UserService = {
    AddOne: async (userData) => {
        const response = await api.post('/Users', userData)
        const validatedData = userResponseSchema.safeParse(response.data.data)
        
        if (!validatedData.success) {
            throw new ApiError('Invalid data', 400)
        }
        
        return validatedData.data
    },
    
    GetOne: async (id) => {
        await new Promise(resolve => setTimeout(resolve, 1000)) // Simulate API call
        const response = await api.get(`/Users/${id}`)
        const validatedData = userResponseSchema.safeParse(response.data.data)
        
        if (!validatedData.success) {
            throw new ApiError('Invalid data', 400)
        }
        
        return validatedData.data
    },
    GetUserByEmail: async (email) => {
        const response = await api.get(`/users/search?email=${email}`)
        const validatedData = userResponseSchema.safeParse(response.data.data)
        console.log("validatedData ", validatedData)
        
        if (!validatedData.success) {
            throw new ApiError('Invalid data', 400)
        }
        
        return validatedData.data
    }
    
}

export default userService
export type { UserService }