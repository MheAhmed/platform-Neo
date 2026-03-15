<template>
  <div class="max-w-2xl mx-auto space-y-8 pb-10 relative">
    
    <div class="text-center">
      <h1 class="text-2xl font-black text-gray-900 uppercase tracking-tight">Nouveau Questionnaire</h1>
      <p class="text-sm text-gray-500 mt-1 font-medium">Configurez votre évaluation</p>
    </div>

    <transition name="toast">
      <div v-if="successMessage" class="fixed top-5 left-1/2 -translate-x-1/2 z-50 flex items-center gap-3 px-6 py-3 bg-emerald-600 text-white rounded-full shadow-2xl border border-emerald-400 min-w-[300px]">
        <svg class="w-5 h-5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7"></path></svg>
        <span class="text-sm font-bold truncate">{{ successMessage }}</span>
      </div>
    </transition>

    <form @submit.prevent="handleSubmit" class="bg-white p-8 rounded-3xl border border-gray-100 shadow-sm space-y-6">
      
      <div>
        <label class="block text-[11px] font-bold text-gray-400 uppercase mb-2 ml-1 tracking-widest">Titre du questionnaire</label>
        <input 
          v-model="form.title" 
          type="text" 
          placeholder="ex: Évaluation Frontend Vue.js"
          class="w-full px-4 py-3 border border-gray-200 rounded-xl bg-gray-50 text-sm focus:ring-2 focus:ring-emerald-400 outline-none transition"
        />
      </div>

      

      <div class="flex gap-3">
        <button 
          v-for="status in (['draft', 'published'] as const)" 
          :key="status"
          type="button"
          @click="form.status = status"
          class="flex-1 py-3 rounded-xl text-[10px] font-black tracking-widest transition-all border uppercase"
          :class="form.status === status 
            ? (status === 'draft' ? 'bg-gray-100 border-gray-400 text-gray-900 shadow-inner' : 'bg-emerald-50 border-emerald-400 text-emerald-700 shadow-inner') 
            : 'bg-white border-gray-100 text-gray-400 hover:border-gray-200'"
        >
          {{ status === 'draft' ? 'Brouillon' : 'Publier' }}
        </button>
      </div>

      <div class="pt-4 flex items-center gap-4">
        <button type="button" @click="router.back()" class="text-[11px] font-bold text-gray-400 hover:text-gray-600 transition uppercase tracking-widest">Retour</button>
        <button 
          type="submit" 
          :disabled="isLoading" 
          class="flex-1 h-12 rounded-2xl bg-gray-900 text-white font-bold text-sm transition-all shadow-xl hover:shadow-emerald-200 disabled:opacity-50 active:scale-[0.98] flex items-center justify-center gap-3"
        >
          <span v-if="!isLoading" class="tracking-widest uppercase">Enregistrer</span>
          <svg v-else class="animate-spin h-5 w-5 text-emerald-400" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
        </button>
      </div>

    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';

interface QuestionnaireForm {
  title: string;
  status: 'draft' | 'published';
  selectedQuestionsIds: number[];
}

const router = useRouter();
const route = useRoute();
const isLoading = ref(false);
const successMessage = ref('');

const form = reactive<QuestionnaireForm>({
  title: '',
  status: 'draft',
  selectedQuestionsIds: []
});

// Récupération automatique des IDs au retour de la banque
onMounted(() => {
  if (route.query.ids) {
    const idsString = route.query.ids as string;
    form.selectedQuestionsIds = idsString.split(',').filter(id => id !== '').map(Number);
  }
});

const goToQuestions = () => {
  router.push('/evaluator/gestionView');
};

const handleSubmit = () => {
  if (!form.title) {
    alert("Veuillez donner un titre au questionnaire.");
    return;
  }

  isLoading.value = true;
  
  setTimeout(() => {
    isLoading.value = false;
    successMessage.value = "Questionnaire enregistré avec succès !";
    setTimeout(() => { successMessage.value = ''; }, 3000);
  }, 1000);
};
</script>

<style scoped>
.toast-enter-active { transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275); }
.toast-enter-from { transform: translate(-50%, -100px); opacity: 0; }
.toast-leave-active { transition: all 0.3s ease-in; }
.toast-leave-to { opacity: 0; transform: translate(-50%, -20px) scale(0.9); }
</style>