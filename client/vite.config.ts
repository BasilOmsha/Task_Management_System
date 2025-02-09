import react from "@vitejs/plugin-react"
import path from "path"
import { defineConfig } from "vite"
import restart from 'vite-plugin-restart'

export default defineConfig(async () => {
    const glsl = (await import('vite-plugin-glsl')).default

    return {
        plugins: [
            react(),
            restart(),
            glsl()
        ],
        resolve: {
            alias: {
                "@": path.resolve(__dirname, "./src")
            },
            extensions: ['.js', '.jsx', '.ts', '.tsx', '.glsl']
        },
        optimizeDeps: {
            include: [
                'react',
                'react-dom',
                'react-router-dom',
                '@tanstack/react-query',
                '@tanstack/react-query-devtools'
            ],
            exclude: [] // Add any problematic dependencies here
        },
        server: {
            host: true,
            open: !('SANDBOX_URL' in process.env || 'CODESANDBOX_HOST' in process.env),
        },
        build: {
            sourcemap: true,
            commonjsOptions: {
                include: [/node_modules/],
                transformMixedEsModules: true
            }
        }
    }
})