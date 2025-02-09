/* eslint-disable indent */
import { ApiError } from "@/utils/errors.ts"
import axios from "axios"

const isDevelopment = import.meta.env.MODE === 'development'

// Get the current hostname
const hostname = window.location.hostname

// Define base URLs based on the hostname
let baseURL = hostname === 'localhost' 
    ? import.meta.env.VITE_API_URL_LOCAL
    : import.meta.env.VITE_API_URL_NETWORK

if (!isDevelopment) {
    // Update this later when you have a working backend server
    baseURL = import.meta.env.VITE_API_URL
}

const api = axios.create(
    {
        baseURL,
        headers: {
            'Content-Type': 'application/json'
        }, 
        timeout: 5000
    }
)

// authorization interceptor
api.interceptors.request.use((config) => {
    const accessToken = localStorage.getItem('accessToken')
    if (accessToken) {
        config.headers.Authorization = `Bearer ${accessToken}`
    }
    return config
}, (error) => {
    return Promise.reject(error)
})

// Enhanced error handling
api.interceptors.response.use(
    (response) => response,
    (error) => {
        // Get the most specific error message available
        const message = error.response?.data?.message 
            || error.response?.data 
            || error.message 
            || 'An error occurred'

        const statusCode = error.response?.status

        // Create custom error with additional info
        const apiError = new ApiError(message, statusCode)

        return Promise.reject(apiError)
    }
)

export default api
