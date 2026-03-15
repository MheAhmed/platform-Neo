<template>
  <div class="max-w-sm mx-auto space-y-6">
    
    <div class="text-center">
      <h2 class="text-2xl font-bold text-gray-900 tracking-tight">Créer un compte entreprise</h2>
      <p class="text-sm text-gray-500 mt-2">Rejoignez <span class="text-emerald-600 font-semibold italic">AppTest</span></p>
    </div>

    <transition name="toast">
      <div v-if="successMessage" class="fixed top-5 left-1/2 -translate-x-1/2 z-50 flex items-center gap-3 px-6 py-3 bg-emerald-600 text-white rounded-full shadow-xl min-w-[260px]">
        <svg class="w-5 h-5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7"></path></svg>
        <span class="text-sm font-bold">{{ successMessage }}</span>
      </div>
    </transition>

    <div v-if="errorMessage" class="p-3 text-sm text-red-700 bg-red-50 border border-red-100 rounded-xl" role="alert">
      {{ errorMessage }}
    </div>

    <div class="bg-white p-6 rounded-3xl border border-gray-100 shadow-sm space-y-6">
      <form @submit.prevent="handleSubmit" class="space-y-4">
        
        <!-- Nom Entreprise -->
        <div>
          <label class="block text-xs font-bold text-gray-400 uppercase tracking-wider mb-2 ml-1">Nom Entreprise</label>
          <input 
            v-model="form.nomEntreprise" 
            type="text" 
            placeholder="ex: Société Générale"
            class="w-full px-4 py-3 rounded-2xl border border-gray-100 bg-gray-50 focus:bg-white focus:border-emerald-500 outline-none transition-all text-sm"
            :class="{ 'border-red-500 bg-red-50': errors.nomEntreprise }"
          />
          <p v-if="errors.nomEntreprise" class="text-[10px] text-red-500 font-bold mt-1 ml-1">{{ errors.nomEntreprise }}</p>
        </div>

        <!-- Matricule Fiscale -->
        <div>
          <label class="block text-xs font-bold text-gray-400 uppercase tracking-wider mb-2 ml-1">Matricule Fiscale</label>
          <input 
            v-model="form.matriculeFiscale" 
            type="text" 
            placeholder="ex: 1234567A/B/C/D"
            class="w-full px-4 py-3 rounded-2xl border border-gray-100 bg-gray-50 focus:bg-white focus:border-emerald-500 outline-none transition-all text-sm"
            :class="{ 'border-red-500 bg-red-50': errors.matriculeFiscale }"
          />
          <p v-if="errors.matriculeFiscale" class="text-[10px] text-red-500 font-bold mt-1 ml-1">{{ errors.matriculeFiscale }}</p>
        </div>

        <!-- Nom Responsable -->
        <div>
          <label class="block text-xs font-bold text-gray-400 uppercase tracking-wider mb-2 ml-1">Nom Responsable</label>
          <input 
            v-model="form.nomResponsable" 
            type="text" 
            placeholder="ex: Ahmed Benali"
            class="w-full px-4 py-3 rounded-2xl border border-gray-100 bg-gray-50 focus:bg-white focus:border-emerald-500 outline-none transition-all text-sm"
            :class="{ 'border-red-500 bg-red-50': errors.nomResponsable }"
          />
          <p v-if="errors.nomResponsable" class="text-[10px] text-red-500 font-bold mt-1 ml-1">{{ errors.nomResponsable }}</p>
        </div>

        <!-- Email Responsable -->
        <div>
          <label class="block text-xs font-bold text-gray-400 uppercase tracking-wider mb-2 ml-1">Email Responsable</label>
          <input 
            v-model="form.emailResponsable" 
            type="email" 
            placeholder="nom@exemple.com"
            class="w-full px-4 py-3 rounded-2xl border border-gray-100 bg-gray-50 focus:bg-white focus:border-emerald-500 outline-none transition-all text-sm"
            :class="{ 'border-red-500 bg-red-50': errors.emailResponsable }"
          />
          <p v-if="errors.emailResponsable" class="text-[10px] text-red-500 font-bold mt-1 ml-1">{{ errors.emailResponsable }}</p>
        </div>

        <button 
          type="submit" 
          :disabled="isLoading" 
          class="w-full py-4 bg-gray-900 border border-gray-900 text-white rounded-2xl text-sm font-bold uppercase tracking-widest hover:bg-emerald-600 transition-all disabled:opacity-50 active:scale-95 flex items-center justify-center gap-3 shadow-lg"
        >
          <span v-if="!isLoading">Créer mon compte</span>
          <svg v-else class="animate-spin h-5 w-5" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path></svg>
        </button>
      </form>
    </div>

    <p class="text-center text-sm text-gray-500 font-medium pt-2">
      Déjà un compte ? 
      <router-link to="/auth/login" class="text-emerald-600 font-bold hover:underline underline-offset-4">
        Se connecter
      </router-link>
    </p>

    <div class="text-center">
      <router-link to="/" class="text-xs font-bold text-gray-300 uppercase tracking-widest hover:text-gray-900 transition-colors">
        ← Retour à l'accueil
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { authService } from '../../services/auth.service';

const router = useRouter();

interface RegistrationForm {
  nomEntreprise: string;
  matriculeFiscale: string;
  nomResponsable: string;
  emailResponsable: string;
}

const form = reactive<RegistrationForm>({
  nomEntreprise: '',
  matriculeFiscale: '',
  nomResponsable: '',
  emailResponsable: ''
});

const errors = reactive<Partial<RegistrationForm>>({});
const errorMessage = ref('');
const successMessage = ref('');
const isLoading = ref(false);

const handleSubmit = async () => {
  errorMessage.value = "";
  Object.keys(errors).forEach(key => delete (errors as any)[key]);

  let isInvalid = false;

  if (!form.nomEntreprise.trim()) {
    errors.nomEntreprise = "Le nom de l'entreprise est requis.";
    isInvalid = true;
  }

  if (!form.matriculeFiscale.trim()) {
    errors.matriculeFiscale = "La matricule fiscale est requise.";
    isInvalid = true;
  }

  if (!form.nomResponsable.trim()) {
    errors.nomResponsable = "Le nom du responsable est requis.";
    isInvalid = true;
  }

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(form.emailResponsable)) {
    errors.emailResponsable = "Email invalide.";
    isInvalid = true;
  }

  if (isInvalid) {
    errorMessage.value = "Veuillez remplir correctement les champs.";
    return;
  }

  isLoading.value = true;
  
  try {
    const response = await authService.registerCompany({
      nomEntreprise: form.nomEntreprise,
      matriculeFiscale: form.matriculeFiscale,
      nomResponsable: form.nomResponsable,
      emailResponsable: form.emailResponsable
    });

    successMessage.value = "Inscription réussie ! Redirection...";
    
    setTimeout(() => {
      router.push('/auth/login');
    }, 2000);

  } catch (err: any) {
    errorMessage.value = err.response?.data?.Message || "Une erreur est survenue lors de l'inscription.";
  } finally {
    isLoading.value = false;
  }
};
</script>

<style scoped>
.toast-enter-active { transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275); }
.toast-enter-from { transform: translate(-50%, -100px); opacity: 0; }
.toast-leave-active { transition: all 0.3s ease-in; }
.toast-leave-to { opacity: 0; transform: translate(-50%, -20px) scale(0.9); }
</style>