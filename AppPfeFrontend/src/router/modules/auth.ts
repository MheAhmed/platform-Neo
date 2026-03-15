export default [
    {
        path: '/auth',
        component: () => import('@/layouts/AuthLayout.vue'),
        meta: { guestOnly: true },
        children: [
            {
                path: '',
                name: 'login',
                component: () => import('@/views/auth/LoginView.vue'),
            },
            {
                path: 'register',
                name: 'register',
                component: () => import('@/views/auth/RegisterView.vue'),
            }
        ]
    },
    {
        path: '/definir-mot-de-passe',
        name: 'set-password',
        component: () => import('@/views/auth/SetPasswordView.vue')
    }
]
