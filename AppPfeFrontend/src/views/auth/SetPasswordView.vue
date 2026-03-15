<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { authService } from '../../services/auth.service'

const route = useRoute()
const router = useRouter()
const token = ref('')
const password = ref('')
const confirmPassword = ref('')
const isLoading = ref(false)
const errorMessage = ref('')
const successMessage = ref('')

onMounted(() => {
  const t = route.query.token as string
  if (!t) {
    errorMessage.value = "Le lien d'activation est invalide ou manquant."
  } else {
    token.value = t
  }
})

const handleSubmit = async () => {
  if (password.value !== confirmPassword.value) {
    errorMessage.value = "Les mots de passe ne correspondent pas."
    return
  }

  if (password.value.length < 6) {
    errorMessage.value = "Le mot de passe doit contenir au moins 6 caractères."
    return
  }

  isLoading.value = true
  errorMessage.value = ''

  try {
    await authService.completeActivation(token.value, password.value)
    successMessage.value = "Votre compte a été activé avec succès !"
    setTimeout(() => {
      router.push('/auth')
    }, 3000)
  } catch (err: any) {
    errorMessage.value = err.response?.data?.message || err.response?.data || "Une erreur est survenue lors de l'activation."
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex items-center justify-center p-6 font-sans">
    <div class="bg-white rounded-[32px] shadow-2xl p-10 max-w-sm w-full border border-gray-100 text-center animate-in fade-in zoom-in duration-500">
      
      <!-- Icon (Key) -->
      <div class="flex justify-center mb-6">
        <div class="w-16 h-16 bg-emerald-600 rounded-2xl flex items-center justify-center shadow-lg shadow-emerald-200">
          <svg class="w-8 h-8 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z" />
          </svg>
        </div>
      </div>

      <!-- Title & Subtitle -->
      <div class="mb-8">
        <h2 class="text-[20px] font-black text-gray-800 tracking-tight leading-tight">Activation de votre compte</h2>
        <p class="text-[11px] font-medium text-gray-400 mt-2 leading-relaxed max-w-[200px] mx-auto">
          Veuillez définir votre mot de passe pour accéder à EvaluaTech
        </p>
      </div>

      <!-- Messages -->
      <div v-if="errorMessage" class="mb-6 p-3 bg-red-50 border border-red-100 rounded-xl">
        <p class="text-[10px] font-bold text-red-600 uppercase">{{ errorMessage }}</p>
      </div>
      <div v-if="successMessage" class="mb-6 p-3 bg-emerald-50 border border-emerald-100 rounded-xl">
        <p class="text-[10px] font-bold text-emerald-600 uppercase">{{ successMessage }}</p>
      </div>

      <form @submit.prevent="handleSubmit" class="space-y-4">
        <!-- New Password -->
        <div class="text-left space-y-1.5 text-xs text-gray-400 font-bold ml-1">
          <label class="block">Nouveau mot de passe</label>
          <input 
            v-model="password"
            type="password" 
            placeholder="••••••••"
            class="w-full px-5 py-4 bg-gray-50 border border-gray-100 rounded-2xl text-xs font-bold focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 outline-none transition-all"
            required
          />
        </div>

        <!-- Confirm Password -->
        <div class="text-left space-y-1.5 text-xs text-gray-400 font-bold ml-1">
          <label class="block">Confirmer</label>
          <input 
            v-model="confirmPassword"
            type="password" 
            placeholder="••••••••"
            class="w-full px-5 py-4 bg-gray-50 border border-gray-100 rounded-2xl text-xs font-bold focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 outline-none transition-all"
            required
          />
        </div>

        <button 
          type="submit"
          :disabled="isLoading || !token"
          class="w-full mt-6 py-4 bg-emerald-600 text-white rounded-2xl text-[12px] font-black tracking-wide shadow-xl shadow-emerald-200 hover:bg-emerald-700 transition-all active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          <span v-if="!isLoading">Activer mon espace</span>
          <span v-else class="flex items-center justify-center gap-2">
            <div class="w-3 h-3 border-2 border-white/20 border-t-white rounded-full animate-spin"></div>
            Activation...
          </span>
        </button>
      </form>
    </div>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&display=swap');
.font-sans { font-family: 'Plus Jakarta Sans', sans-serif; }
</style>
