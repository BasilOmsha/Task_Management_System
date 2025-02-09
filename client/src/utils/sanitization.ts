// Common patterns for malicious content
const SCRIPT_PATTERN = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi
const HTML_TAGS = /<[^>]*>/g
const DANGEROUS_ENTITIES = /javascript:|data:|vbscript:|onload=|onerror=|onclick=/gi
const MULTIPLE_SPACES = /\s+/g
const UNICODE_SPACES = /[\u200B-\u200D\uFEFF]/g

export function sanitizeInput(value: string): string {
    if (!value) {return value}

    return value
        // Remove HTML tags
        .replace(HTML_TAGS, '')
        // Remove script tags and their contents
        .replace(SCRIPT_PATTERN, '')
        // Remove dangerous entities/protocols
        .replace(DANGEROUS_ENTITIES, '')
        // Normalize spaces (remove multiple spaces and special unicode spaces)
        .replace(MULTIPLE_SPACES, ' ')
        .replace(UNICODE_SPACES, '')
        // Trim whitespace
        .trim()
}

// Special sanitization for usernames
export function sanitizeUsername(username: string): string {
    return username
        // Remove anything that's not alphanumeric
        .replace(/[^a-zA-Z0-9]/g, '')
        // Limit length
        .slice(0, 30)
}

// Special sanitization for names
export function sanitizeName(name: string): string {
    return name
        // Allow letters, spaces, hyphens, and apostrophes
        .replace(/[^a-zA-Z\s\-']/g, '')
        // Normalize spaces
        .replace(MULTIPLE_SPACES, ' ')
        .trim()
        // Limit length
        .slice(0, 50)
}

// Special sanitization for email
export function sanitizeEmail(email: string): string {
    return email
        // Basic email sanitization (remove spaces and limit length)
        .replace(/\s/g, '')
        .toLowerCase()
        .slice(0, 254)
}

// Add the new types and constant
export type SanitizationFunction = (value: string) => string

export interface SanitizationRules {
    [key: string]: SanitizationFunction
}

export const fieldSanitizers: SanitizationRules = {
    confirmPassword: (value: string) => value, // Don't sanitize passwords - they can contain special characters
    email: sanitizeEmail,
    firstname: sanitizeName,
    lastname: sanitizeName,
    password: (value: string) => value, // Don't sanitize passwords - they can contain special characters
    username: sanitizeUsername
} 