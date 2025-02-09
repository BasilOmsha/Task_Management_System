import { ModeToggle } from "@/components/theme/mode-toggle.tsx"
import { Separator } from "@/components/ui/separator.tsx"
import { SidebarTrigger } from "@/components/ui/sidebar.tsx"
import { useUser } from "@/features/user-management/useUser.ts"

import { Breadcrumbs } from "./breadcrumbs.tsx"
import { UserNav } from "./user-nav.tsx"

export function Header() {
    const { userData } = useUser()
        
    return (
        <header className='flex h-16 shrink-0 items-center justify-between gap-2 transition-[width,height] ease-linear group-has-[[data-collapsible=icon]]/sidebar-wrapper:h-12'>
            <div className='flex items-center gap-2 px-4'>
                <SidebarTrigger className='-ml-1' />
                <Separator className='mr-2 h-4' orientation='vertical' />
                <Breadcrumbs />
            </div>
            <div className='flex items-center gap-2 px-4'>
                <div className='hidden md:flex'>
                    {/* <SearchInput /> */}
                </div>
                <UserNav user={{
                    firstname: userData?.firstName || "",
                    lastname: userData?.lastName || "",
                    email: userData?.email || "",
                    avatar: ""
                }} />
                <ModeToggle />
            </div>
        </header>
    )
}