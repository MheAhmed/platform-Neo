<template>
  <div class="min-h-screen bg-[#FBFBFC] flex items-center justify-center p-4 overflow-x-hidden relative">
    
    <div class="max-w-xl w-full space-y-5 pb-10 transition-all duration-500" :class="{'mr-[400px] opacity-40 scale-95 pointer-events-none': showAI}">
      
      <div class="relative flex items-center justify-center mb-2">
        <router-link to="/evaluator/gestionView" class="absolute left-0 p-1.5 text-gray-400 hover:text-emerald-600 transition-colors bg-white rounded-xl shadow-sm border border-gray-100">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M10 19l-7-7m0 0l7-7m-7 7h18"/></svg>
        </router-link>
        <h2 class="text-lg font-bold text-gray-900 uppercase tracking-tight italic">Nouvelle Question</h2>
      </div>

      <div class="bg-white rounded-2xl p-6 shadow-xl shadow-gray-200/40 border border-gray-100 space-y-5">
        <div class="space-y-4 pr-1 custom-scroll max-h-[75vh] overflow-y-auto">
          
          <div>
            <div class="flex items-center justify-between mb-1.5 ml-1">
              <label class="text-[10px] font-bold text-gray-400 uppercase tracking-wider">Énoncé de la question</label>
              
              <button 
                @click="showAI = true" 
                class="flex items-center gap-1.5 text-emerald-600 hover:text-emerald-700 transition-colors group"
                title="Générer avec l'IA"
              >
                <span class="text-[9px] font-bold uppercase italic opacity-0 group-hover:opacity-100 transition-opacity">Magic Create</span>
                <div class="p-1.5 bg-emerald-50 rounded-lg group-hover:bg-emerald-600 group-hover:text-white transition-all shadow-sm">
                  <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"/>
                  </svg>
                </div>
              </button>
            </div>
            
            <textarea 
              v-model="newQuestion.text" 
              placeholder="Saisissez votre question ou utilisez l'IA à droite..."
              rows="3" 
              class="w-full px-4 py-3 border border-gray-100 rounded-xl bg-gray-50 text-xs focus:ring-2 focus:ring-emerald-400 outline-none transition resize-none font-medium"
            ></textarea>
          </div>

          <div class="grid grid-cols-2 gap-3">
            <div>
              <label class="block text-[10px] font-bold text-gray-400 uppercase mb-1.5 ml-1 tracking-wider">Questionnaire</label>
              <select v-model="newQuestion.assignedQuestionnaireId" class="w-full px-4 py-2.5 border border-gray-100 rounded-xl bg-gray-50 text-[11px] font-bold text-gray-700 outline-none cursor-pointer">
                <option value="" disabled>Choisir...</option>
                <option v-for="q in existingQuestionnaires" :key="q.id" :value="q.id">{{ q.title.toUpperCase() }}</option>
              </select>
            </div>
            <div>
              <label class="block text-[10px] font-bold text-gray-400 uppercase mb-1.5 ml-1 tracking-wider">Complexité</label>
              <select v-model="newQuestion.difficulty" class="w-full px-4 py-2.5 border border-gray-100 rounded-xl bg-gray-50 text-[11px] font-bold text-gray-700 outline-none cursor-pointer">
                <option value="facile">Facile</option>
                <option value="moyen">Moyen</option>
                <option value="difficile">Difficile</option>
              </select>
            </div>
          </div>

          <div>
            <label class="block text-[10px] font-bold text-gray-400 uppercase mb-1.5 ml-1 tracking-wider">Type de réponse</label>
            <div class="grid grid-cols-3 gap-1 bg-gray-100 p-1 rounded-xl">
              <button v-for="type in ['QCM', 'QCU', 'Vrai/Faux']" :key="type" type="button" @click="changeType(type)" :class="newQuestion.type === type ? 'bg-white text-emerald-600 shadow-sm font-bold' : 'text-gray-400'" class="py-1.5 rounded-lg text-[9px] uppercase transition-all">
                {{ type }}
              </button>
            </div>
          </div>

          <div class="bg-gray-50 p-4 rounded-2xl border border-gray-100 space-y-3">
            <div class="flex items-center justify-between mb-1">
              <label class="text-[10px] font-bold text-gray-900 uppercase tracking-widest italic">Options & Réponses</label>
              <button v-if="newQuestion.type !== 'Vrai/Faux'" @click="addOption" class="text-[9px] font-bold text-emerald-600 uppercase hover:underline">+ Ajouter</button>
            </div>

            <div v-for="(opt, index) in options" :key="index" class="flex items-center gap-3 bg-white p-2.5 rounded-lg border border-gray-100 transition-all shadow-sm" :class="{'ring-2 ring-emerald-400/20 border-emerald-200': opt.isCorrect}">
              <input :type="newQuestion.type === 'QCM' ? 'checkbox' : 'radio'" v-model="opt.isCorrect" @change="handleOptionCheck(index)" class="w-3.5 h-3.5 text-emerald-600 rounded-full border-gray-300" />
              <input v-model="opt.text" type="text" class="flex-1 bg-transparent border-none text-[11px] outline-none font-bold text-gray-700" placeholder="Saisir une option..." />
            </div>
          </div>
        </div>

        <div class="flex gap-3 pt-3 border-t border-gray-50">
          <button @click="$router.push('/evaluator/gestionView')" class="flex-1 py-3 text-[10px] font-bold text-gray-400 uppercase tracking-widest hover:text-gray-600 transition-colors">Annuler</button>
          <button @click="saveQuestion" class="flex-[2] py-3 bg-gray-900 text-white rounded-xl font-bold text-[10px] tracking-widest shadow-lg hover:bg-emerald-600 active:scale-[0.98] transition-all uppercase">
            Enregistrer
          </button>
        </div>
      </div>
    </div>

    <transition name="panel">
      <aside v-if="showAI" class="fixed top-0 right-0 h-screen w-[400px] bg-gray-900 z-50 shadow-2xl p-8 border-l border-white/10 overflow-y-auto custom-scroll-dark">
        <button @click="showAI = false" class="absolute top-6 right-6 text-gray-500 hover:text-white transition-colors">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path d="M6 18L18 6M6 6l12 12" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>
        </button>

        <div class="space-y-8 mt-4">
          <header>
            <h3 class="text-xl font-bold text-white uppercase tracking-tighter italic">AI <span class="text-emerald-500">Laboratory</span></h3>
            <p class="text-[9px] font-bold text-gray-500 uppercase tracking-[0.2em] mt-1">Génération de deux variantes</p>
          </header>

          <div class="space-y-4">
            <div class="space-y-2">
              <label class="text-[9px] font-bold text-emerald-500 uppercase tracking-widest">Sujet de recherche</label>
              <input v-model="aiTopic" type="text" placeholder="EX: REACTIVE VS REF VUE 3..." 
                class="w-full bg-white/5 border border-white/10 rounded-xl p-4 text-white text-[10px] font-bold uppercase outline-none focus:border-emerald-500 transition-colors" />
            </div>

            <button @click="generateAIDouble" :disabled="isGenerating" 
              class="w-full py-4 bg-emerald-600 text-white rounded-xl font-bold uppercase text-[10px] tracking-widest hover:bg-emerald-500 transition-all disabled:opacity-30">
              {{ isGenerating ? 'Analyse en cours...' : 'Générer les choix' }}
            </button>
          </div>

          <div v-if="aiChoices.length > 0" class="space-y-6">
            <div v-for="(choice, index) in aiChoices" :key="index" class="relative bg-white/5 border border-white/10 rounded-2xl p-5 space-y-4 hover:border-emerald-500/40 transition-all group">
              <span class="absolute -top-3 left-4 px-2 py-0.5 bg-emerald-600 text-white text-[8px] font-bold uppercase rounded shadow-lg">Variante {{ index + 1 }}</span>
              
              <p class="text-[11px] font-bold text-gray-100 italic leading-relaxed uppercase">{{ choice.text }}</p>

              <div class="space-y-1.5">
                <div v-for="(opt, i) in choice.options" :key="i" class="flex items-center justify-between p-2.5 rounded-lg bg-white/5 border border-white/5">
                  <span class="text-[9px] text-gray-400 font-medium italic">{{ opt.text }}</span>
                  <div v-if="opt.isCorrect" class="w-1.5 h-1.5 bg-emerald-500 rounded-full shadow-[0_0_8px_rgba(16,185,129,0.6)]"></div>
                </div>
              </div>

              <button @click="applyChoice(choice)" class="w-full py-3 bg-white/10 text-white hover:bg-white hover:text-gray-900 rounded-xl font-bold uppercase text-[9px] tracking-[0.1em] transition-all">
                Utiliser cette question
              </button>
            </div>
          </div>
        </div>
      </aside>
    </transition>

  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

// ÉTATS IA
const showAI = ref(false);
const aiTopic = ref('');
const isGenerating = ref(false);
const aiChoices = ref<any[]>([]);

// ÉTATS FORMULAIRE
const tagInput = ref('');
const options = ref([{ text: '', isCorrect: false }, { text: '', isCorrect: false }]);
const existingQuestionnaires = ref<any[]>([]);
const newQuestion = reactive({ text: '', assignedQuestionnaireId: '', difficulty: 'facile', type: 'QCM' });

onMounted(() => {
  const saved = JSON.parse(localStorage.getItem('app_questionnaires') || '[]');
  existingQuestionnaires.value = saved.length ? saved : [{ id: 1, title: "Développement Web" }];
});

const changeType = (type: string) => {
  newQuestion.type = type;
  options.value = type === 'Vrai/Faux' 
    ? [{ text: 'Vrai', isCorrect: true }, { text: 'Faux', isCorrect: false }]
    : [{ text: '', isCorrect: false }, { text: '', isCorrect: false }];
};

const handleOptionCheck = (index: number) => {
  if (newQuestion.type !== 'QCM') {
    options.value.forEach((opt, i) => opt.isCorrect = (i === index));
  }
};

const addOption = () => options.value.push({ text: '', isCorrect: false });

// LOGIQUE GÉNÉRATION IA (2 CHOIX)
const generateAIDouble = () => {
  if (!aiTopic.value) return;
  isGenerating.value = true;
  aiChoices.value = [];

  // Simulation d'une IA qui propose deux angles différents
  setTimeout(() => {
    aiChoices.value = [
      {
        text: `Quelle est la principale différence technique lors de l'utilisation de "${aiTopic.value}" ?`,
        type: 'QCU',
        options: [
          { text: 'La gestion de la réactivité profonde', isCorrect: true },
          { text: 'La coloration syntaxique du code', isCorrect: false },
          { text: 'Le temps de compilation Webpack', isCorrect: false }
        ]
      },
      {
        text: `Dans quel cas spécifique est-il recommandé d'implémenter "${aiTopic.value}" ?`,
        type: 'QCM',
        options: [
          { text: 'Pour les objets complexes nichés', isCorrect: true },
          { text: 'Pour les primitives uniquement', isCorrect: false },
          { text: 'Pour optimiser les appels API', isCorrect: true }
        ]
      }
    ];
    isGenerating.value = false;
  }, 1600);
};

const applyChoice = (choice: any) => {
  newQuestion.text = choice.text;
  newQuestion.type = choice.type;
  options.value = JSON.parse(JSON.stringify(choice.options));
  showAI.value = false;
  aiChoices.value = [];
  aiTopic.value = '';
};

const saveQuestion = () => {
  if (!newQuestion.text || !newQuestion.assignedQuestionnaireId) return alert("Champs requis !");
  router.push('/evaluator/gestionView');
};
</script>

<style scoped>
/* TRANSITION DU PANEL */
.panel-enter-active, .panel-leave-active { transition: transform 0.6s cubic-bezier(0.16, 1, 0.3, 1); }
.panel-enter-from, .panel-leave-to { transform: translateX(100%); }

/* CUSTOM SCROLLS */
.custom-scroll::-webkit-scrollbar { width: 3px; }
.custom-scroll::-webkit-scrollbar-thumb { background: #e2e8f0; border-radius: 10px; }
.custom-scroll-dark::-webkit-scrollbar { width: 3px; }
.custom-scroll-dark::-webkit-scrollbar-thumb { background: rgba(255,255,255,0.1); border-radius: 10px; }

select {
  appearance: none;
  background-image: url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='%2394a3b8' stroke-width='2.5' stroke-linecap='round' stroke-linejoin='round'%3e%3cpolyline points='6 9 12 15 18 9'%3e%3c/polyline%3e%3c/svg%3e");
  background-repeat: no-repeat;
  background-position: right 0.75rem center;
  background-size: 0.8em;
}
</style>