import { Toaster } from "react-hot-toast"
import { Navigate, Route, Routes, useLocation } from "react-router-dom"
// import {  } from "./utils/dumyDelay.tsx"

import { Suspense, lazy } from "react"

import { ProtectedRoute } from "@/components/form/login/protected-route.tsx"
import { ModeToggle } from "@/components/theme/mode-toggle.tsx"
import { ThemeProvider } from "@/components/theme/theme-provider.tsx"
import { Layout } from "@/pages/dashboard/layout.tsx"
import { Overview } from "@/pages/dashboard/overview/overview.tsx"
import { Login } from "@/pages/login/login.tsx"

import { RefreshPrompt } from "./components/auth/RefreshPrompt.tsx"
import { useAuth } from "./features/user-management/useAuth.ts"

const ProjectBoardPage = lazy(() => import("./pages/dashboard/kanban/projectboard.tsx"))
const Profile = lazy(() => import("./pages/dashboard/profile/profile.tsx"))
const Team = lazy(() => import("./pages/dashboard/team/team.tsx"))
const ProjectsPage = lazy(() => import("./pages/dashboard/projects/projects-page.tsx"))
const SignupOptions = lazy(() => import("./pages/signup/signupOptions/signup-options.tsx"))
const Signup = lazy(() => import("./pages/signup/signup.tsx"))

function App() {
    const { isAuthenticated, showRefreshPrompt, handleRefreshResponse } = useAuth()
    const location = useLocation()

    const isPublicRoute = ["/email-registration", "/login", "/signup-options"].includes(
        location.pathname
    )

    return (
        <div className="App">
            <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
                {isPublicRoute && (
                    <div className="fixed top-4 right-4 z-50">
                        <ModeToggle />
                    </div>
                )}
                <Toaster
                    position="top-right"
                    toastOptions={{
                        className: "",
                        duration: 1000,
                        removeDelay: 1000,
                        style: {
                            background: "#363636",
                            color: "#fff"
                        }
                    }}
                />
                {showRefreshPrompt && (
                    <RefreshPrompt onResponse={handleRefreshResponse} open={showRefreshPrompt} />
                )}

                <Routes>
                    {/* Public routes */}
                    <Route
                        element={
                            isAuthenticated ? (
                                <Navigate replace to="/dashboard/overview" />
                            ) : (
                                <Login />
                            )
                        }
                        path="/login"
                    />

                    <Route
                        element={
                            isAuthenticated ? (
                                <Navigate replace to="/dashboard/overview" />
                            ) : (
                                <Suspense fallback={<div>Loading...</div>}>
                                    <SignupOptions />
                                </Suspense>
                            )
                        }
                        path="/signup-options"
                    />

                    <Route
                        element={
                            isAuthenticated ? (
                                <Navigate replace to="/dashboard/overview" />
                            ) : (
                                <Suspense fallback={<div>Loading...</div>}>
                                    <Signup />
                                </Suspense>
                            )
                        }
                        path="/email-registration"
                    />

                    {/* Protected routes */}
                    <Route element={<ProtectedRoute />}>
                        <Route path="/dashboard">
                            <Route element={<Navigate replace to="/dashboard/overview" />} index />

                            <Route
                                element={
                                    <Layout>
                                        <Suspense fallback={<div>Loading...</div>}>
                                            <Overview />
                                        </Suspense>
                                    </Layout>
                                }
                                path="overview"
                            />

                            <Route
                                element={
                                    <Layout>
                                        <Suspense fallback={<div>Loading...</div>}>
                                            <ProjectsPage />
                                        </Suspense>
                                    </Layout>
                                }
                                path="projects"
                            />

                            <Route
                                element={
                                    <Layout>
                                        <Suspense>
                                            <Team />
                                        </Suspense>
                                    </Layout>
                                }
                                path="team"
                            />

                            <Route
                                element={
                                    <Layout>
                                        <Suspense fallback={<div>Loading...</div>}>
                                            <ProjectBoardPage />
                                        </Suspense>
                                    </Layout>
                                }
                                path="kanban"
                            />

                            <Route
                                element={
                                    <Layout>
                                        <Suspense fallback={<div>Loading...</div>}>
                                            <Profile />
                                        </Suspense>
                                    </Layout>
                                }
                                path="profile"
                            />
                        </Route>
                    </Route>

                    {/* Catch all - redirect based on authentication and current path */}
                    <Route
                        element={
                            isAuthenticated ? (
                                location.pathname.startsWith("/dashboard") ? (
                                    <Navigate replace to={location.pathname} />
                                ) : (
                                    <Navigate replace to="/dashboard/overview" />
                                )
                            ) : (
                                <Navigate replace to="/login" />
                            )
                        }
                        path="*"
                    />
                </Routes>
            </ThemeProvider>
        </div>
    )
}

export default App
