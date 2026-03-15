import { createRouter, createWebHistory } from 'vue-router'

import authRoutes from './modules/auth'
import superAdminRoutes from './modules/superAdmin'
import adminEntrepriseRoutes from './modules/adminEntreprise'
import evaluatorRoutes from './modules/evaluator'
import candidatRoutes from './modules/candidat'

import HomeView from '../views/HomeView.vue'
import { authService } from '../services/auth.service'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    ...authRoutes,
    ...superAdminRoutes,
    ...adminEntrepriseRoutes,
    ...evaluatorRoutes,
    ...candidatRoutes,
    {
      path: '/:pathMatch(.*)*',
      redirect: '/'
    }
  ],
})

router.beforeEach((to) => {
  // BYPASS TEMPORAIRE POUR LE DÉVELOPPEMENT : Permet d'accéder à toutes les routes directement
  return true;

  const user = authService.getCurrentUser()
  const isAuthenticated = !!user
  const userRole = user?.role?.toUpperCase()

  console.log(`[Router Guard] Path: ${to.path} | Auth: ${isAuthenticated} | Role: ${userRole}`)

  // 1. Redirection automatique depuis HOME si déjà connecté
  if (to.path === '/' && isAuthenticated) {
    if (userRole === 'ADMIN' || userRole === 'SUPERADMIN') return '/superAdmin'
    if (userRole === 'ENTREPRISE' || userRole === 'ADMIN_ENTREPRISE' || userRole === 'ADMINENTREPRISE') return '/adminEntreprise'
    if (userRole === 'EVALUATOR') return '/evaluator'
    if (userRole === 'CANDIDAT') return '/candidat'
  }

  // 2. Gestion des routes protégées
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!isAuthenticated) {
      return '/auth'
    }

    // Vérification du rôle spécifique
    const requiredRole = (to.meta.role as string)?.toUpperCase()

    if (requiredRole) {
      // SuperAdmin can access EVERYTHING
      if (userRole === 'SUPERADMIN') return true

      // Entreprise flexibility
      if (requiredRole === 'ENTREPRISE' && (userRole === 'ENTREPRISE' || userRole === 'ADMIN_ENTREPRISE' || userRole === 'ADMINENTREPRISE')) return true

      // Admin flexibility
      if (requiredRole === 'ADMIN' && (userRole === 'ADMIN' || userRole === 'SUPERADMIN')) return true

      // Exact match
      if (userRole === requiredRole) return true

      // Otherwise, access denied
      console.warn(`[Router Guard] Access Denied for ${to.path}`)
      return '/'
    }
  }

  // 3. Gestion des routes "GUEST ONLY"
  if (to.matched.some(record => record.meta.guestOnly)) {
    if (isAuthenticated) {
      if (userRole === 'ADMIN' || userRole === 'SUPERADMIN') return '/superAdmin'
      if (userRole === 'ENTREPRISE' || userRole === 'ADMIN_ENTREPRISE' || userRole === 'ADMINENTREPRISE') return '/adminEntreprise'
      if (userRole === 'EVALUATOR') return '/evaluator'
      if (userRole === 'CANDIDAT') return '/candidat'
      return '/'
    }
  }

  return true
})

export default router
