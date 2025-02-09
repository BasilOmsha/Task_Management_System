import { jwtDecode } from 'jwt-decode'

interface TokenPayload {
    nameid: string  // User ID
    email: string   // User Email
    exp: number     // Expiration timestamp
    iss: string     // Issuer
    aud: string     // Audience
}

export function decodeToken(token: string): TokenPayload | null {
    try {
        return jwtDecode<TokenPayload>(token)
    } catch (error) {
        console.error('Error decoding token:', error)
        return null
    }
}

export function getUserInfoFromToken() {
    const token = localStorage.getItem('accessToken')
    if (!token) {return null}

    const decodedToken = decodeToken(token)
  
    if (!decodedToken) {return null}

    return {
        userId: decodedToken.nameid,
        email: decodedToken.email,
        exp: decodedToken.exp
    }
}

export function isTokenExpired(token: string | null): boolean {
    if (!token) {return true}
  
    try {
        const decodedToken = decodeToken(token)
        if (!decodedToken) {return true}
  
        const currentTime = Math.floor(Date.now() / 1000)
        console.log('Token expiration:', decodedToken.exp, currentTime)
        return decodedToken.exp < currentTime
    } catch (error) {
        console.error('Token expiration check failed:', error)
        return true
    }
}