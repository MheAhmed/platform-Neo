<template>
  <div class="min-h-screen bg-[#FBFBFC] text-[#1A1C1E] font-sans selection:bg-emerald-100 selection:text-emerald-900">
    <div class="max-w-[1200px] mx-auto p-6 space-y-6">
      
      <header class="flex flex-col md:flex-row md:items-center justify-between gap-4 pb-6 border-b border-gray-200/60">
        <div class="flex items-center gap-3">
          <div class="w-10 h-10 bg-emerald-600 rounded-xl flex items-center justify-center shadow-lg shadow-emerald-100/50">
            <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>
            </svg>
          </div>
          <div>
            <h1 class="text-lg font-bold tracking-tight uppercase italic leading-none">
              Gestion <span class="text-emerald-600">Questions</span>
            </h1>
            <p class="text-[9px] font-bold uppercase tracking-widest text-gray-400 mt-1 flex items-center gap-2">
              <span class="w-1 h-1 bg-emerald-500 rounded-full animate-pulse"></span> 
              Console Évaluateur
            </p>
          </div>
        </div>

        <div class="flex items-center gap-2">
          <router-link to="/evaluator/questionnaire" 
            class="group flex items-center gap-2 px-4 py-2.5 bg-gray-900 rounded-xl font-bold text-[10px] text-white uppercase tracking-wider hover:bg-emerald-600 transition-all shadow-md active:scale-95">
            <svg class="w-3.5 h-3.5 text-emerald-500 group-hover:text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M12 4v16m8-8H4"/>
            </svg>
            Nouveau Questionnaire
          </router-link>
          
          <router-link to="/evaluator/question" 
            class="group flex items-center gap-2 px-4 py-2.5 bg-gray-900 rounded-xl font-bold text-[10px] text-white uppercase tracking-wider hover:bg-emerald-600 transition-all shadow-md active:scale-95">
            <svg class="w-3.5 h-3.5 text-emerald-500 group-hover:text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M12 4v16m8-8H4"/>
            </svg>
            Nouvelle Question
          </router-link>
        </div>
      </header>

      <div class="grid grid-cols-12 gap-6 h-[calc(100vh-180px)]">
        
        <aside class="col-span-3 bg-white rounded-2xl p-5 border border-gray-100 shadow-sm flex flex-col">
          <h3 class="text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-5 px-1">Bibliothèque</h3>
          <nav class="flex-1 overflow-y-auto space-y-1 pr-1 custom-scroll text-[10px]">
            <button @click="selectedTag = 'Tous'"
              :class="selectedTag === 'Tous' ? 'bg-emerald-600 text-white shadow-md' : 'text-gray-500 hover:bg-gray-50'"
              class="w-full text-left px-3 py-2.5 rounded-lg font-bold uppercase tracking-wider transition-all">
              # Tout explorer
            </button>
            <button v-for="tag in availableTags" :key="tag" @click="selectedTag = tag"
              :class="selectedTag === tag ? 'bg-emerald-50 text-emerald-700 ring-1 ring-emerald-100' : 'text-gray-400 hover:text-gray-900'"
              class="w-full text-left px-3 py-2.5 rounded-lg font-bold uppercase tracking-wider transition-all flex justify-between items-center group">
              <span># {{ tag }}</span>
              <span v-if="selectedTag === tag" class="w-1 h-3 bg-emerald-500 rounded-full"></span>
            </button>
          </nav>
        </aside>

        <main class="col-span-9 flex flex-col gap-4 relative">
          
          <div class="relative group">
            <div class="absolute inset-y-0 left-4 flex items-center pointer-events-none">
              <svg class="w-3.5 h-3.5 text-gray-300 group-focus-within:text-emerald-500 transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/>
              </svg>
            </div>
            <input v-model="searchQuery" type="text" placeholder="Rechercher un test ou une question..." 
              class="w-full pl-10 pr-4 py-3 bg-white border border-gray-100 rounded-xl text-[11px] focus:ring-2 focus:ring-emerald-50 outline-none transition-all shadow-sm font-medium italic" />
          </div>

          <div class="flex-1 overflow-y-auto space-y-3 pr-2 custom-scroll pb-20">
            <div v-for="quiz in filteredContent" :key="quiz.id" 
                 class="bg-white rounded-xl border border-gray-100 shadow-sm transition-all overflow-hidden">
              
              <div @click="toggleQuiz(quiz.id)" class="p-4 flex items-center justify-between cursor-pointer group select-none">
                <div class="flex items-center gap-3">
                  <div class="w-9 h-9 bg-gray-50 rounded-lg flex items-center justify-center border border-gray-100 group-hover:bg-emerald-50 transition-colors">
                    <span class="text-[10px] font-bold text-gray-400 group-hover:text-emerald-600">{{ quiz.questions.length }}</span>
                  </div>
                  <div>
                    <h3 class="text-xs font-bold text-gray-800 uppercase italic group-hover:text-emerald-700 transition-colors">{{ quiz.title }}</h3>
                    <p class="text-[8px] font-bold text-gray-400 uppercase mt-0.5">{{ quiz.status }} • {{ quiz.date }}</p>
                  </div>
                </div>
                <svg :class="expandedQuizId === quiz.id ? 'rotate-180 text-emerald-600' : 'text-gray-300'" 
                     class="w-3.5 h-3.5 transition-transform duration-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M19 9l-7 7-7-7"/>
                </svg>
              </div>

              <transition name="quiz-slide">
                <div v-if="expandedQuizId === quiz.id" class="px-4 pb-4 bg-gray-50/30">
                  <div class="grid gap-2 pt-2 border-t border-gray-100">
                    <div v-for="q in quiz.questions" :key="q.id" 
                         class="flex items-center justify-between p-3 bg-white rounded-lg border border-gray-100 hover:border-emerald-200 transition-all">
                      <div class="flex items-center gap-3">
                        <input type="checkbox" v-model="selectedQuestionIds" :value="q.id"
                          class="w-3.5 h-3.5 rounded border-gray-300 text-emerald-600 focus:ring-emerald-500 cursor-pointer" />
                        <div>
                          <p class="text-[11px] font-bold text-gray-700 leading-tight">{{ q.text }}</p>
                          <div class="flex items-center gap-2 mt-1">
                            <span v-for="t in q.tags" :key="t" class="text-[7px] font-bold text-emerald-600 bg-emerald-50 px-1.5 py-0.5 rounded uppercase tracking-tighter">#{{ t }}</span>
                            <span :class="getDifficultyColor(q.difficulty)" class="text-[7px] font-bold uppercase px-1.5 py-0.5 rounded tracking-tighter">{{ q.difficulty }}</span>
                          </div>
                        </div>
                      </div>
                    </div>

                    <div class="mt-2 flex justify-center">
                      <router-link 
                        :to="{ path: '/evaluator/question', query: { questionnaireName: quiz.title } }" 
                        class="flex items-center gap-2 px-5 py-2.5 bg-emerald-50 text-emerald-700 border border-emerald-100 rounded-lg font-bold text-[9px] uppercase tracking-widest hover:bg-emerald-600 hover:text-white transition-all shadow-sm active:scale-95"
                      >
                        <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M12 4v16m8-8H4"/>
                        </svg>
                        Ajouter à "{{ quiz.title }}"
                      </router-link>
                    </div>

                  </div>
                </div>
              </transition>
            </div>
          </div>

          <transition name="fade-slide">
            <div v-if="selectedQuestionIds.length > 0" 
                 class="absolute bottom-4 left-0 right-0 flex justify-center z-50 pointer-events-none">
              <button @click="ajouterAuTest" 
                      class="pointer-events-auto flex items-center gap-3 px-6 py-3 bg-emerald-600 text-white rounded-xl shadow-lg hover:bg-emerald-700 hover:scale-105 transition-all group">
                <div class="text-left border-r border-white/20 pr-3">
                  <span class="block text-[10px] font-bold uppercase tracking-wider">{{ selectedQuestionIds.length }} Sélectionnée(s)</span>
                </div>
                <div class="flex items-center gap-1 text-[10px] font-bold uppercase tracking-widest">
                  Exporter <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
                </div>
              </button>
            </div>
          </transition>

        </main>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const selectedTag = ref('Tous');
const searchQuery = ref('');
const expandedQuizId = ref<number | null>(null);
const selectedQuestionIds = ref<number[]>([]);

interface Question {
  id: number;
  text: string;
  tags: string[];
  difficulty: 'Facile' | 'Moyen' | 'Difficile';
}

interface Questionnaire {
  id: number;
  title: string;
  status: string;
  date: string;
  questions: Question[];
}

const questionnaires = ref<Questionnaire[]>([
  {
    id: 1, title: "Onboarding Frontend", status: "Actif", date: "28/02/2026",
    questions: [
      { id: 10, text: "Expliquez le concept de Props Drilling.", tags: ['React'], difficulty: 'Moyen' },
      { id: 11, text: "Citez trois hooks de base dans Vue 3.", tags: ['Vue.js'], difficulty: 'Facile' }
    ]
  },
  {
    id: 2, title: "Algorithmique Python", status: "Brouillon", date: "25/02/2026",
    questions: [
      { id: 12, text: "Complexité de la recherche binaire ?", tags: ['Python'], difficulty: 'Difficile' }
    ]
  }
]);

const availableTags = computed(() => {
  const tags = new Set<string>();
  questionnaires.value.forEach(quiz => quiz.questions.forEach(q => q.tags.forEach(t => tags.add(t))));
  return Array.from(tags).sort();
});

const filteredContent = computed(() => {
  return questionnaires.value.filter(quiz => {
    const searchMatch = quiz.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
                        quiz.questions.some(q => q.text.toLowerCase().includes(searchQuery.value.toLowerCase()));
    const tagMatch = selectedTag.value === 'Tous' || quiz.questions.some(q => q.tags.includes(selectedTag.value));
    return searchMatch && tagMatch;
  });
});

const toggleQuiz = (id: number) => { expandedQuizId.value = expandedQuizId.value === id ? null : id; };

const getDifficultyColor = (diff: string) => {
  if (diff === 'Facile') return 'bg-blue-50 text-blue-600';
  if (diff === 'Moyen') return 'bg-orange-50 text-orange-600';
  return 'bg-red-50 text-red-600';
};

const ajouterAuTest = () => {
  if (selectedQuestionIds.value.length === 0) return alert("Sélectionnez des questions !");
  router.push({
    path: '/evaluator/questionnaire',
    query: { ids: selectedQuestionIds.value.join(',') }
  });
};
</script>

<style scoped>
.custom-scroll::-webkit-scrollbar { width: 3px; }
.custom-scroll::-webkit-scrollbar-thumb { background: #e2e8f0; border-radius: 10px; }

.quiz-slide-enter-active, .quiz-slide-leave-active { transition: all 0.3s ease-out; max-height: 800px; }
.quiz-slide-enter-from, .quiz-slide-leave-to { max-height: 0; opacity: 0; overflow: hidden; }

.fade-slide-enter-active, .fade-slide-leave-active { transition: all 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275); }
.fade-slide-enter-from, .fade-slide-leave-to { opacity: 0; transform: translateY(15px); }
</style>