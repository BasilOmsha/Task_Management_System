import React from "react"
import { BrowserRouter } from "react-router-dom"

import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"
import { Analytics } from "@vercel/analytics/react"
import ReactDOM from "react-dom/client"

import App from "./App.tsx"

import "./index.css"

const queryClient = new QueryClient({
    defaultOptions: {
        mutations: {
            retry: 1,
            retryDelay: 1000
        }
    }
})

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <BrowserRouter
                future={{
                    v7_relativeSplatPath: true, // Better catch-all route handling
                    v7_startTransition: true // Smoother route transitions
                }}
            >
                <App />
                <ReactQueryDevtools initialIsOpen={false} />
                <Analytics />
            </BrowserRouter>
        </QueryClientProvider>
    </React.StrictMode>
)
