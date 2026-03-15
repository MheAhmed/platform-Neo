export default [
    {
        path: '/superAdmin',
        component: () => import('@/layouts/MainLayout.vue'),
        meta: { requiresAuth: true, role: 'SuperAdmin' },
        children: [
            {
                path: '',
                name: 'superAdmin-dashboard',
                component: () => import('@/views/superAdmin/AdminDashboard.vue'),
            },
            {
                path: 'users',
                name: 'superAdmin-users',
                component: () => import('@/views/superAdmin/AdminUsers.vue'),
            }
        ]
    }
]
