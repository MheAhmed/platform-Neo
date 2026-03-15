export default [
    {
        path: '/adminEntreprise',
        component: () => import('@/layouts/MainLayout.vue'),
        meta: { requiresAuth: true, role: 'Entreprise' },
        children: [
            {
                path: '',
                name: 'adminEntreprise-dashboard',
                component: () => import('@/views/adminEntreprise/EntrepriseView.vue'),
            },
            {
                path: 'stats',
                name: 'adminEntreprise-stats',
                component: () => import('@/views/adminEntreprise/EntrepriseStats.vue'),
            },
            {
                path: 'jobs',
                name: 'adminEntreprise-jobs',
                component: () => import('@/views/adminEntreprise/JobOfferList.vue'),
            }
        ]
    }
]
