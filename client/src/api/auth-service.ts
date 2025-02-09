import { LoginInput, TokenResponse, tokenResponseSchema } from "@/schemas/auth.ts"
import { ApiError } from "@/utils/errors.ts"

import api from "./index.ts"

export default {
    login: async (credentials: LoginInput): Promise<TokenResponse> => {
        const response = await api.post('/Authenticate/login', credentials)
        console.log("response data ", response.data) 

        const validatedData = tokenResponseSchema.safeParse(response.data)
        console.log("validatedData ", validatedData)

        if (!validatedData.success) {
            console.log("validatedData.error ", validatedData.error)
            throw new ApiError('Invalid data', 400)
        }

        return validatedData.data
    },

    refresh: async (tokens: TokenResponse): Promise<TokenResponse> => {
        const response = await api.post('/Authenticate/refresh', tokens)
        return response.data
    },

    logout: async (): Promise<boolean> => {
        console.log("logging out")
        const response = await api.delete('/Authenticate/logout')
        console.log("response data ", response.data)
        return response.data
    }
}