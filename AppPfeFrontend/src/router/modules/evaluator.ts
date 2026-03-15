import QuestionView from '@/views/evaluator/question.vue'
import RapportView from '@/views/evaluator/RapportView.vue'

export default [
    {
        path: '/evaluator',
        component: () => import('@/layouts/MainLayout.vue'),
        meta: { requiresAuth: true, role: 'Evaluator' },
        children: [
            {
                path: '',
                name: 'evaluator-dashboard',
                component: () => import('@/views/evaluator/EvaluatorView.vue'),
            },
            {
                path: 'gestionView',
                name: 'evaluator-gestion',
                component: () => import('@/views/evaluator/gestionView.vue'),
            },
            {
                path: 'candidatsView',
                name: 'evaluator-candidats',
                component: () => import('@/views/evaluator/candidatsView.vue'),
            },
            {
                path: 'questionnaire',
                name: 'evaluator-questionnaire',
                component: () => import('@/views/evaluator/questionnaire.vue'),
            },
            {
                path: 'question',
                name: 'evaluator-question',
                component: QuestionView,
            },
            {
                path: 'rapportView/:id',
                name: 'evaluator-rapport',
                component: RapportView,
            }

        ]
    }
]