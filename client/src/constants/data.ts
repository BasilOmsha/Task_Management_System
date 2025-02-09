import { NavItem } from "@/types/index.ts"

export const navItems: NavItem[] = [
    {
        title: 'Dashboard',
        url: '/dashboard/overview',
        icon: 'dashboard',
        isActive: false,
        shortcut: ['d', 'd'],
        items: [] // Empty array as there are no child items for Dashboard
    },
    {
        title: 'Projects',
        url: '/dashboard/projects',
        icon: 'projects',
        shortcut: ['p', 'p'],
        isActive: false,
        items: [] // No child items
    },
    {
        title: 'Team',
        url: '/dashboard/team',
        icon: 'team',
        shortcut: ['t', 't'],
        isActive: false,
        items: [] // No child items
    },
    {
        title: 'Kanban',
        url: '/dashboard/kanban',
        icon: 'kanban',
        shortcut: ['k', 'k'],
        isActive: false,
        items: [] // No child items
    }
]
