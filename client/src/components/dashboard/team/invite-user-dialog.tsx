import { useEffect, useState } from "react"
import { useForm } from "react-hook-form"
import toast from "react-hot-toast"

import { Button } from "@/components/ui/button"
import {
    Dialog,
    DialogContent,
    DialogFooter,
    DialogHeader,
    DialogTitle
} from "@/components/ui/dialog"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage
} from "@/components/ui/form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod" // Important: import these

import { Input } from "@/components/ui/input"
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue
} from "@/components/ui/select"
import { Spinner } from "@/components/ui/spinner"
import { useRoleByName } from "@/features/role-management/useRolebyName"
import { useFindUserByEmail } from "@/features/user-management/useFindUserByEmail"
import { useAddUser } from "@/features/worksapce-management/useAddUser"
import { User } from "@/pages/dashboard/team/team"
import { useQueryClient } from "@tanstack/react-query"

const inviteFormSchema = z.object({
    email: z.string().email("Invalid email address"),
    role: z.enum(["Admin", "Contributor"])
})

type InviteFormValues = z.infer<typeof inviteFormSchema>

type InviteUserDialogProps = {
    isOpen: boolean
    onClose: () => void
    onInvite: (user: User) => void
    workspaceId: string
}

export function InviteUserDialog({
    isOpen,
    onClose,
    onInvite,
    workspaceId
}: InviteUserDialogProps) {
    const form = useForm<InviteFormValues>({
        resolver: zodResolver(inviteFormSchema),
        defaultValues: {
            email: "",
            role: "Contributor"
        }
    })

    const { data: user } = useFindUserByEmail(form.watch("email"))
    const { data: roleData } = useRoleByName(form.watch("role"))
    const { isPending, mutate: addUser } = useAddUser()

    const [showSuggestion, setShowSuggestion] = useState(false)

    const queryClient = useQueryClient()

    useEffect(() => {
        // If user is found (and has an email), show it again
        if (user && user.email) {
            setShowSuggestion(true)
        } else {
            setShowSuggestion(false)
        }
    }, [user])

    const onSubmit = async (values: InviteFormValues) => {
        if (!user || !roleData) {
            toast.error(`User with email ${values.email} doesn't exist!`)
            return
        }

        addUser(
            {
                workspaceId,
                userId: user.id,
                roleId: roleData.id
            },
            {
                onSuccess: async () => {
                    // toast.success('User invited successfully')
                    onClose()
                    form.reset()
                }
                // onError: () => {
                //     toast.error('Failed to invite user')
                // }
            }
        )
    }

    return (
        <Dialog onOpenChange={onClose} open={isOpen}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle className="text-center text-2xl font-semibold text-gray-800">
                        Invite User
                    </DialogTitle>
                </DialogHeader>
                <Form {...form}>
                    <form className="grid gap-6 py-4" onSubmit={form.handleSubmit(onSubmit)}>
                        <FormField
                            control={form.control}
                            name="email"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Email</FormLabel>
                                    <FormControl>
                                        <Input
                                            placeholder="user@example.com"
                                            type="email"
                                            {...field}
                                        />
                                    </FormControl>
                                    {/* {user && user.id && (
                                        <p className="mt-1 text-sm text-green-600">
                                        Found user: {user.email}
                                        </p>
                                    )} */}
                                    <FormMessage />
                                    {user && user.email && showSuggestion && (
                                        <div
                                            className="
                                            mt-2 
                                            rounded-md 
                                            border border-gray-300 
                                            bg-gray 
                                            p-2
                                            text-sm 
                                            cursor-pointer 
                                            hover:bg-gray-100
                                            dark:hover:text-black
                                        "
                                            onClick={() => {
                                                // Fill the input with the user's email
                                                form.setValue("email", user.email)
                                                // Hide the suggestion afterwards
                                                setShowSuggestion(false)
                                            }}
                                        >
                                            Found: {user.email}
                                        </div>
                                    )}
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="role"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Role</FormLabel>
                                    <Select
                                        defaultValue={field.value}
                                        onValueChange={field.onChange}
                                    >
                                        <FormControl>
                                            <SelectTrigger>
                                                <SelectValue placeholder="Select a role" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            <SelectItem value="Admin">Admin</SelectItem>
                                            <SelectItem value="Contributor">Contributor</SelectItem>
                                        </SelectContent>
                                    </Select>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <DialogFooter>
                            <Button className="w-full" disabled={isPending} type="submit">
                                {isPending ? (
                                    <>
                                        <Spinner className="mr-2" size="sm" />
                                        Inviting...
                                    </>
                                ) : (
                                    "Invite uesr"
                                )}
                            </Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    )
}
