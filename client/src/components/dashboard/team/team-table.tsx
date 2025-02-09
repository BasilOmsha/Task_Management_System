import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { User as UserType } from '@/pages/dashboard/team/team'
import { Mail, User } from 'lucide-react'

import { SkeletonTable } from "./skeleton-table"
import { UserActions } from "./user-action"


type TeamTableProps = {
    users: UserType[]
    onRemoveUser: (userId: string) => void
    onEditUserRole: (userId: string, newRole: UserType['role']) => void
    isLoading: boolean
}

export function TeamTable({ users, onRemoveUser, onEditUserRole, isLoading }: TeamTableProps) {

    
    if (isLoading) {
        return <SkeletonTable columns={5} rows={users.length || 5} />
    }
  
    return (
        <div className=" rounded-lg shadow-lg overflow-hidden">
            <Table>
                <TableHeader>
                    <TableRow>
                        <TableHead className="w-[50px]"></TableHead>
                        <TableHead className="font-semibold text-gray-600">Name</TableHead>
                        <TableHead className="font-semibold text-gray-600">Username</TableHead>
                        <TableHead className="font-semibold text-gray-600">Email</TableHead>
                        <TableHead className="font-semibold text-gray-600">Role</TableHead>
                        <TableHead className="w-[100px]"></TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {users.map((user) => (
                        <TableRow className=" transition-colors" key={user.id}>
                            <TableCell>
                                <User className="h-5 w-5 text-gray-400" />
                            </TableCell>
                            <TableCell className="font-medium">{user.name}</TableCell>
                            <TableCell className="text-gray-600">{user.username}</TableCell>
                            <TableCell>
                                <div className="flex items-center text-gray-600">
                                    <Mail className="h-4 w-4 mr-2 text-gray-400" />
                                    {user.email}
                                </div>
                            </TableCell>
                            <TableCell>
                                <span className={`px-2 py-1 rounded-full text-xs font-medium ${
                                    user.role === 'Admin' ? 'bg-blue-100 text-blue-800' :
                                        user.role === 'Contributor' ? 'bg-green-100 text-green-800' :
                                            'bg-gray-100 text-gray-800'
                                }`}>
                                    {user.role}
                                </span>
                            </TableCell>
                            <TableCell>
                                <UserActions 
                                    onEditUserRole={onEditUserRole}
                                    onRemoveUser={onRemoveUser}
                                    user={user}
                                />
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </div>
    )
}

