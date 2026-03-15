<script setup lang="ts">
import { RouterLink, RouterView, useRouter } from 'vue-router'
import { authService, type User } from '../services/auth.service'
import { onMounted, ref, computed } from 'vue'

const router = useRouter()
const user = ref<User | null>(null)

onMounted(() => {
  user.value = authService.getCurrentUser()
})

const role = computed(() => user.value?.role?.toUpperCase())

const handleLogout = () => {
  authService.logout()
  router.push('/')
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 font-sans italic text-gray-900">
    <header class="bg-white border-b border-gray-100 px-6 py-4 flex items-center justify-between sticky top-0 z-50">
      <router-link to="/" class="flex items-center gap-2 group">
        <div class="w-8 h-8 bg-emerald-600 rounded-lg flex items-center justify-center text-white font-black text-xs rotate-3 group-hover:rotate-0 transition-transform shadow-lg shadow-emerald-200">AT</div>
        <span class="text-lg font-black tracking-tighter uppercase italic text-gray-900">App<span class="text-emerald-600">Test</span></span>
      </router-link>

      <nav class="hidden md:flex items-center gap-6">
        <template v-if="role === 'ADMIN' || role === 'SUPERADMIN'">
          <router-link to="/superAdmin" class="text-[10px] font-black uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Console</router-link>
          <router-link to="/superAdmin/users" class="text-[10px] font-black uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Utilisateurs</router-link>
        </template>
        
        <template v-if="role === 'ENTREPRISE' || role === 'ADMIN_ENTREPRISE'">
          <router-link to="/adminEntreprise" class="text-[10px] font-black uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Dashboard</router-link>
          <router-link to="/adminEntreprise/jobs" class="text-[10px] font-black uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Recrutement</router-link>
        </template>

        <template v-if="role === 'EVALUATOR'">
          <router-link to="/evaluator" class="text-[10px] font-black uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Évaluations</router-link>
          <router-link to="/evaluator/gestionView" class="text-[10px] font-black uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Banque Questions</router-link>
        </template>

        <template v-if="role === 'CANDIDAT'">
          <router-link to="/candidat" class="text-[10px] font-black uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Mes Tests</router-link>
        </template>
      </nav>

      <div class="flex items-center gap-4">
        <div class="hidden sm:block text-right">
          <p class="text-[9px] font-black text-gray-900 uppercase leading-none">{{ user?.nom || 'Utilisateur' }}</p>
          <p class="text-[8px] font-bold text-emerald-600 uppercase mt-0.5 tracking-tighter">{{ user?.role || 'Rôle' }}</p>
        </div>
        <button @click="handleLogout" class="px-5 py-2 bg-gray-900 text-white rounded-xl text-[10px] font-black uppercase tracking-[0.2em] shadow-lg hover:bg-red-600 transition-all active:scale-95">
          Quitter
        </button>
      </div>
    </header>

    <main class="max-w-7xl mx-auto p-6 md:p-10">
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
.router-link-active {
  color: #059669; /* emerald-600 */
  border-bottom: 2px solid #059669;
}
</style>
