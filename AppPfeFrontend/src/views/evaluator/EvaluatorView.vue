<template>
  <div class="max-w-5xl mx-auto space-y-6 pb-20 px-6 pt-10 relative">
    
    <header class="flex items-center justify-between mb-4">
      <div>
        <h1 class="text-xl font-bold text-gray-900 uppercase tracking-tight italic leading-none">
          Dashboard <span class="text-emerald-600">Évaluateur</span>
        </h1>
        <p class="text-[10px] text-gray-400 mt-1.5 font-bold uppercase tracking-[0.2em]">Gestion des évaluations</p>
      </div>
      
      <div class="flex items-center gap-3">
        <router-link to="/evaluator/corbeille" class="h-9 px-4 flex items-center gap-2 rounded-xl border border-gray-100 bg-white text-gray-500 hover:text-red-500 transition-all text-[10px] font-bold uppercase tracking-widest shadow-sm">
          <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>
          Corbeille
        </router-link>

        <button @click="showWizard = !showWizard" 
          :class="showWizard ? 'bg-gray-100 text-gray-500' : 'bg-emerald-600 text-white shadow-md'"
          class="h-9 px-5 rounded-xl hover:opacity-90 transition-all flex items-center gap-2 uppercase tracking-widest font-bold text-[10px]">
          <svg v-if="!showWizard" class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M12 4v16m8-8H4"/></svg>
          <svg v-else class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M6 18L18 6M6 6l12 12"/></svg>
          {{ showWizard ? 'Annuler' : 'Nouveau Test' }}
        </button>
      </div>
    </header>

    <transition name="fade-slide">
      <div v-if="showWizard" class="bg-gray-900 p-8 rounded-2xl border border-gray-800 shadow-xl relative overflow-hidden z-30 mb-8">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6 relative z-10">
          <div class="space-y-3">
            <label class="text-[9px] font-bold uppercase text-emerald-500 tracking-[0.2em]">1. Titre de l'évaluation</label>
            <input v-model="wizardData.name" type="text" placeholder="NOM DU TEST..." class="w-full p-4 bg-gray-800 text-white rounded-xl border border-gray-700 outline-none focus:ring-2 focus:ring-emerald-500 font-bold uppercase text-xs" />
          </div>
          <div class="space-y-3">
            <label class="text-[9px] font-bold uppercase text-emerald-500 tracking-[0.2em]">2. Configuration Rapide</label>
            <div class="flex gap-3">
              <router-link to="/evaluator/candidatsView" class="flex-1 p-4 bg-gray-800 rounded-xl border border-gray-700 flex justify-between items-center hover:border-emerald-500 transition-colors">
                <span class="text-[10px] text-gray-400 font-bold uppercase">Candidats</span>
                <span class="bg-emerald-600 text-white text-[10px] px-2 py-0.5 rounded-md font-bold">{{ wizardData.candidatesCount }}</span>
              </router-link>
              <router-link to="/evaluator/gestionView" class="flex-1 p-4 bg-gray-800 rounded-xl border border-gray-700 flex justify-between items-center hover:border-emerald-500 transition-colors">
                <span class="text-[10px] text-gray-400 font-bold uppercase">Questions</span>
                <span class="bg-emerald-600 text-white text-[10px] px-2 py-0.5 rounded-md font-bold">{{ wizardData.questionsCount }}</span>
              </router-link>
            </div>
          </div>
          <div class="md:col-span-2 flex justify-end gap-3 pt-6 border-t border-gray-800">
            <button @click="finalizeTest" :disabled="!isWizardValid" class="px-8 h-12 bg-emerald-600 text-white rounded-xl font-bold uppercase tracking-widest text-[10px] disabled:opacity-20 hover:bg-emerald-500 transition-all">
              Publier l'évaluation
            </button>
          </div>
        </div>
      </div>
    </transition>

    <nav class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <div class="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm flex items-center gap-4">
        <div class="w-10 h-10 rounded-xl bg-emerald-50 flex items-center justify-center text-emerald-600 flex-shrink-0"><svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"/></svg></div>
        <div class="min-w-0"><p class="text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-0.5">Total Tests</p><span class="text-xl font-bold text-gray-900 leading-none">{{ tests.length }}</span></div>
      </div>

      <router-link to="/evaluator/candidatsView" class="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm flex items-center gap-4 transition-all hover:border-emerald-500 group">
        <div class="w-10 h-10 rounded-xl bg-emerald-50 flex items-center justify-center text-emerald-600 group-hover:bg-emerald-600 group-hover:text-white transition-colors flex-shrink-0"><svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/></svg></div>
        <div class="min-w-0"><p class="text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-0.5 group-hover:text-emerald-600">Candidats</p><span class="text-xl font-bold text-gray-900 leading-none">312</span></div>
      </router-link>

      <router-link to="/evaluator/gestionView" class="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm flex items-center gap-4 transition-all hover:border-emerald-500 group">
        <div class="w-10 h-10 rounded-xl bg-emerald-50 flex items-center justify-center text-emerald-600 group-hover:bg-emerald-600 group-hover:text-white transition-colors flex-shrink-0"><svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><circle cx="12" cy="12" r="10"/><path d="M9.09 9a3 3 0 0 1 5.83 1c0 2-3 3-3 3"/></svg></div>
        <div class="min-w-0 flex-1"><p class="text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-0.5 group-hover:text-emerald-600">Banque Questions</p>
          <div class="flex items-center gap-2"><span class="text-xl font-bold text-gray-900 leading-none">156</span><span class="text-[8px] font-bold text-gray-300 uppercase italic">Questions</span></div>
        </div>
      </router-link>
    </nav>

    <section class="space-y-4 pt-4">
      <div class="flex items-center justify-between px-2">
        <h2 class="text-[10px] font-bold text-gray-400 uppercase tracking-[0.2em]">Archives des Tests</h2>
        <div class="relative w-full max-w-xs">
          <input v-model="searchQuery" type="text" placeholder="Rechercher..." class="w-full pl-10 pr-4 py-2 bg-white border border-gray-100 rounded-xl text-[10px] outline-none focus:ring-2 focus:ring-emerald-400 shadow-sm" />
          <svg class="w-3.5 h-3.5 absolute left-3.5 top-2.5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/></svg>
        </div>
      </div>

      <div class="space-y-3">
        <div v-for="test in filteredTests" :key="test.id" class="flex flex-col">
          <article class="group bg-white p-3 rounded-2xl border border-gray-100 shadow-sm hover:border-emerald-200 transition-all flex items-center relative z-20">
            <div class="w-10 h-10 rounded-xl bg-gray-900 flex items-center justify-center flex-shrink-0 text-white text-[10px] font-bold italic">{{ test.id }}</div>
            <div class="mx-6 border-r border-gray-100 pr-6 text-center shrink-0"><p class="text-lg font-bold text-gray-800 leading-none">{{ test.candidates }}</p><p class="text-[8px] font-bold text-gray-400 uppercase mt-0.5">Partic.</p></div>
            <div class="flex-1 min-w-0"><h4 class="text-xs font-bold text-gray-800 uppercase truncate tracking-tight group-hover:text-emerald-700 transition-colors">{{ test.name }}</h4><p class="text-[10px] text-gray-400 font-bold italic mt-0.5 tracking-wide">{{ test.date }} • {{ test.questions.length }} Questions</p></div>
            
            <div class="flex items-center gap-1.5 bg-gray-50/80 p-1.5 rounded-xl ml-4 shrink-0 border border-gray-100">
               <button @click="test.showQuestions = !test.showQuestions" class="w-9 h-9 flex items-center justify-center rounded-lg bg-white text-emerald-600 hover:bg-emerald-600 hover:text-white transition-all shadow-sm" :class="test.showQuestions ? 'bg-emerald-600 text-white' : ''">
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"/><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"/></svg>
               </button>

               <router-link :to="`/evaluator/RapportView/${test.id}`" class="w-9 h-9 flex items-center justify-center rounded-lg bg-white text-blue-500 hover:bg-blue-600 hover:text-white transition-all shadow-sm">
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/></svg>
               </router-link>

               <button @click="deleteTest(test.id)" class="w-9 h-9 flex items-center justify-center text-gray-300 hover:text-red-500 transition-colors">
                 <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>
               </button>
            </div>
          </article>

          <transition name="slide-down">
            <div v-if="test.showQuestions" class="bg-gray-900 mx-6 rounded-b-2xl -mt-6 pt-10 pb-6 px-8 space-y-4 shadow-xl z-10 border border-gray-800">
               <div v-for="(q, idx) in test.questions" :key="idx" class="flex gap-4 items-start border-b border-white/5 pb-3 last:border-0">
                  <span class="text-[10px] font-bold text-emerald-500 mt-0.5">{{ (idx + 1).toString().padStart(2, '0') }}</span>
                  <p class="text-xs font-bold text-gray-100 uppercase tracking-tight italic">{{ q.text }}</p>
               </div>
            </div>
          </transition>
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';

const showWizard = ref(false);
const searchQuery = ref('');
const wizardData = ref({ name: '', candidatesCount: 24, questionsCount: 15 });

const tests = ref([
  { 
    id: 102, name: "Évaluation Algorithmique Python", candidates: 24, date: "28 Fév 2026", showQuestions: false, 
    questions: [{ text: "Quelle est la sortie de print(type([])) ?" }, { text: "Comment définit-on une fonction ?" }] 
  },
  { 
    id: 101, name: "Test Intégration React Junior", candidates: 12, date: "25 Fév 2026", showQuestions: false, 
    questions: [{ text: "Quel hook pour l'état ?" }, { text: "Expliquer le Virtual DOM." }] 
  }
]);

const isWizardValid = computed(() => wizardData.value.name.length > 3);

const finalizeTest = () => {
  tests.value.unshift({
    id: tests.value.length + 101,
    name: wizardData.value.name,
    candidates: wizardData.value.candidatesCount,
    date: "Aujourd'hui",
    showQuestions: false,
    questions: [{ text: "Question générée..." }]
  });
  showWizard.value = false;
};

const deleteTest = (id: number) => {
  if(confirm("Supprimer l'archive ?")) tests.value = tests.value.filter(t => t.id !== id);
};

const filteredTests = computed(() => {
  return tests.value.filter(t => t.name.toLowerCase().includes(searchQuery.value.toLowerCase()));
});
</script>

<style scoped>
.fade-slide-enter-active, .fade-slide-leave-active { transition: all 0.4s ease-out; }
.fade-slide-enter-from, .fade-slide-leave-to { opacity: 0; transform: translateY(-30px); }

.slide-down-enter-active, .slide-down-leave-active { transition: all 0.4s ease; max-height: 800px; overflow: hidden; }
.slide-down-enter-from, .slide-down-leave-to { opacity: 0; max-height: 0; transform: translateY(-20px); }
</style>