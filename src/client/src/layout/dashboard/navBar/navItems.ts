import {
    AlertCircle as AlertCircleIcon,
    BarChart as BarChartIcon,
    Lock as LockIcon,
    Settings as SettingsIcon,
    ShoppingBag as ShoppingBagIcon,
    User as UserIcon,
    UserPlus as UserPlusIcon,
    Users as UsersIcon
} from 'react-feather';

const navItems = [
    {
        href: '/admin/dashboard',
        icon: BarChartIcon,
        title: 'Dashboard'
    },
    {
        href: '/admin/customers',
        icon: UsersIcon,
        title: 'Customers'
    },
    {
        href: '/admin/products',
        icon: ShoppingBagIcon,
        title: 'Products'
    },
    {
        href: '/admin/account',
        icon: UserIcon,
        title: 'Account'
    },
    {
        href: '/admin/settings',
        icon: SettingsIcon,
        title: 'Settings'
    },
    {
        href: '/login',
        icon: LockIcon,
        title: 'Login'
    },
    {
        href: '/register',
        icon: UserPlusIcon,
        title: 'Register'
    },
    {
        href: '/404',
        icon: AlertCircleIcon,
        title: 'Error'
    }
];

export default navItems;