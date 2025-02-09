import { useNavigate } from 'react-router-dom'

import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Button } from '@/components/ui/button'
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuGroup,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
import { CirclePlus, LogOut, ReceiptText, Settings,  UserRound} from 'lucide-react'

import { LogoutButton } from '../form/logout-button.tsx'

export function UserNav({
    user,
}: {
    user: {
        firstname: string
        lastname: string
        email: string
        avatar: string
    }
}) {

    const navigate = useNavigate()

    function goToProfile() {
        // navigate to profile page
        navigate('/dashboard/profile')
    }

    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Button className='relative h-8 w-8 rounded-full' variant='ghost'>
                    <Avatar className='h-8 w-8'>
                        <AvatarImage
                        />
                        <AvatarFallback>
                            {user?.firstname?.charAt(0)?.toUpperCase() ?? ""}
                            {user?.lastname?.charAt(0)?.toUpperCase() ?? ""}
                        </AvatarFallback>
                    </Avatar>
                </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align='end' className='w-56' forceMount>
                <DropdownMenuLabel className='font-normal'>
                    <div className='flex flex-col space-y-1'>
                        <p className='text-sm font-medium leading-none'>
                            {user.firstname} {user.lastname}
                        </p>
                        <p className='text-xs leading-none text-muted-foreground'>
                            {user.email}
                        </p>
                    </div>
                </DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuGroup>
                    <DropdownMenuItem>
                        <button className='flex items-center w-full'
                            onClick={goToProfile}
                        >
                            <UserRound className='size-4 mr-2' />
                            Profile
                        </button>
                    </DropdownMenuItem>
                    <DropdownMenuItem>
                        <ReceiptText />
                             Billing
                    </DropdownMenuItem>
                    <DropdownMenuItem>
                        <Settings />
                            Settings
                    </DropdownMenuItem>
                    <DropdownMenuItem>
                        <CirclePlus />
                        New Team
                    </DropdownMenuItem>
                </DropdownMenuGroup>
                <DropdownMenuSeparator />
                <DropdownMenuItem>
                    <LogOut />
                    <LogoutButton />
                </DropdownMenuItem>
            </DropdownMenuContent>
        </DropdownMenu>
    )
}

