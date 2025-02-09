import { Button } from "@/components/ui/button.tsx"
import { Input } from "@/components/ui/input.tsx"
import { Label } from "@/components/ui/label.tsx"
import { useUser } from "@/features/user-management/useUser.ts"

export default function Profile() {
    const { userData } = useUser()

    const stringDate = userData?.createdAt
    const dateObject = stringDate ? new Date(stringDate) : null
    // Extract "YYYY-MM-DD" (the first part of the ISO string)
    const formattedDate = dateObject ? dateObject.toISOString().split("T")[0] : ""

    const stringDate2 = userData?.updatedAt
    const dateObject2 = stringDate2 ? new Date(stringDate2) : null
    const formattedDate2 = dateObject2 ? dateObject2.toISOString().split("T")[0] : ""

    const f1 = userData?.firstName?.charAt(0)?.toUpperCase() ?? ""
    const f2 = userData?.lastName?.charAt(0)?.toUpperCase() ?? ""
    const initials = f1 + f2

    return (
        <div>
            <div className="px-4 space-y-6 md:px-6">
                <header className="space-y-1.5">
                    <div className="flex items-center space-x-4">
                        <img
                            alt= {initials}
                            className="border rounded-full"
                            height="96"
                            src="/profile.svg"
                            style={{ aspectRatio: "96/96", objectFit: "cover" }}
                            width="96"
                        />
                        <div className="space-y-1.5">
                            <h1 className="text-2xl font-bold">{userData?.firstName} {userData?.lastName}</h1>
                            {/* <p className="text-gray-500 dark:text-gray-400">Product Designer</p> */}
                        </div>
                    </div>
                </header>
                <div className="space-y-6">
                    <div className="space-y-2">
                        <h2 className="text-lg font-semibold">Personal Information</h2>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                            <div>
                                <Label htmlFor="name">First name</Label>
                                <Input defaultValue={userData?.firstName} id="firstName" placeholder="Enter your first name"/>
                            </div>
                            <div>
                                <Label htmlFor="name">Last name</Label>
                                <Input defaultValue={userData?.lastName} id="lastName"  placeholder="Enter your last name"/>
                            </div>
                            <div>
                                <Label htmlFor="name">Username</Label>
                                <Input defaultValue={userData?.username} id="Username"  placeholder="Enter your user name"/>
                            </div>
                            <div>
                                <Label htmlFor="email">Email</Label>
                                <Input defaultValue={userData?.email} id="email" placeholder="Enter your email" type="email" />
                            </div>
                            <div>
                                <Label htmlFor="phone">Created at</Label>
                                <Input defaultValue={formattedDate}  id="CreatedAt" type="date"/>
                            </div>
                            <div>
                                <Label htmlFor="phone">Last updated at</Label>
                                <Input defaultValue={formattedDate2}  id="UpdatedAt" type="date"/>
                            </div>
                        </div>
                    </div>
                    <div className="space-y-2">
                        <h2 className="text-lg font-semibold">Change Password</h2>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                            <div>
                                <Label htmlFor="current-password">Current Password</Label>
                                <Input id="current-password" placeholder="Enter your current password" type="password" />
                            </div>
                            <div>
                                <Label htmlFor="new-password">New Password</Label>
                                <Input id="new-password" placeholder="Enter your new password" type="password" />
                            </div>
                            <div>
                                <Label htmlFor="confirm-password">Confirm Password</Label>
                                <Input id="confirm-password" placeholder="Confirm your new password" type="password" />
                            </div>
                        </div>
                    </div>
                </div>
                <div className="mt-8">
                    <Button disabled={true} size="lg">Save</Button>
                </div>
            </div>
        </div>
    )
}