import { Header } from "@/components/dashboard/header.tsx"
import PageContainer from "@/components/dashboard/page-container.tsx"
import { AppSidebar } from "@/components/dashboard/side-nav/app-sidebar.tsx"
import { SidebarInset, SidebarProvider } from "@/components/ui/sidebar.tsx"
import { WorkspaceProvider } from "@/features/worksapce-management/provider.tsx"

export function Layout({ children }: { children: React.ReactNode }) {
    return (
        <WorkspaceProvider>
            <SidebarProvider defaultOpen={true}>
                <AppSidebar />
                <SidebarInset>
                    <Header />
                    <PageContainer scrollable>
                        {children}
                    </PageContainer>
                </SidebarInset>
            </SidebarProvider>
        </WorkspaceProvider>
    )
}