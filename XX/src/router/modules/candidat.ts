export default [
    {
        path: '/candidat',
        component: () => import('@/layouts/MainLayout.vue'),
        meta: { requiresAuth: true, role: 'Candidat' },
        children: [
            {
                path: '',
                name: 'candidat-dashboard',
                component: () => import('@/views/candidat/CandidatView.vue'),
            },
            {
                path: 'profile',
                name: 'candidat-profile',
                component: () => import('@/views/candidat/ProfileView.vue'),
            }
        ]
    }
]
