<template>
  <div class="max-w-5xl mx-auto space-y-6 pb-20 px-4 pt-10">
    
    <header class="flex flex-col md:flex-row md:items-center justify-between gap-6">
      <div class="flex items-center gap-4">
        <router-link 
          to="/evaluator" 
          class="p-2.5 bg-white rounded-xl shadow-sm border border-gray-100 text-gray-400 hover:text-emerald-600 transition-all shrink-0"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M10 19l-7-7m0 0l7-7m-7 7h18"/>
          </svg>
        </router-link>

        <div class="flex items-center gap-4">
          <div class="w-11 h-11 bg-emerald-600 rounded-xl flex items-center justify-center shadow-lg shadow-emerald-100 shrink-0">
            <div class="w-6 h-6 rounded-full border-2 border-white flex items-center justify-center">
              <span class="text-white font-bold text-xs">?</span>
            </div>
          </div>
          <div>
            <h1 class="text-xl font-bold text-gray-900 uppercase tracking-tight italic leading-none">Candidats</h1>
            <div class="flex items-center gap-2 mt-1">
              <div class="w-1.5 h-1.5 rounded-full bg-emerald-500"></div>
              <p class="text-[9px] text-slate-400 font-bold uppercase tracking-widest">Console Évaluateur</p>
            </div>
          </div>
        </div>
      </div>
      
      <button 
        @click="showAddForm = !showAddForm" 
        class="h-10 px-6 rounded-xl bg-gray-900 text-white shadow-md font-bold text-[10px] uppercase tracking-widest hover:bg-emerald-600 transition-all"
      >
        {{ showAddForm ? '× Fermer' : '+ Nouveau Candidat' }}
      </button>
    </header>

    <transition name="expand">
      <div v-if="showAddForm" class="bg-white p-6 rounded-2xl border border-emerald-100 shadow-xl shadow-emerald-50/50 space-y-4 overflow-hidden">
        <h3 class="text-[10px] font-bold text-gray-900 uppercase tracking-widest italic mb-2">Informations du nouveau candidat</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <input v-model="newCandidate.name" type="text" placeholder="NOM & PRÉNOM" class="custom-input" />
          <input v-model="newCandidate.email" type="email" placeholder="EMAIL" class="custom-input" />
          <input v-model="newCandidate.dept" type="text" placeholder="DÉPARTEMENT" class="custom-input" />
        </div>
        <div class="flex justify-end pt-2">
          <button @click="handleAdd" class="px-8 py-2.5 bg-emerald-600 text-white rounded-xl font-bold text-[10px] uppercase tracking-widest hover:bg-gray-900 transition-all">
            Confirmer l'ajout
          </button>
        </div>
      </div>
    </transition>

    <div class="flex items-center justify-between bg-white p-3 px-5 rounded-2xl border border-gray-100 shadow-sm">
      <span class="text-[9px] font-bold text-gray-400 uppercase tracking-widest">
        {{ filteredCandidates.length }} Candidats enregistrés
      </span>
      <div class="relative w-48 md:w-56">
        <input v-model="search" type="text" placeholder="RECHERCHER..." class="w-full pl-9 pr-4 py-2 bg-gray-50 border-none rounded-xl text-[9px] font-bold outline-none focus:ring-2 focus:ring-emerald-400 transition-all" />
        <svg class="w-3.5 h-3.5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/></svg>
      </div>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      <transition-group name="list">
        <div v-for="c in filteredCandidates" :key="c.id" 
          class="group bg-white p-4 rounded-xl border border-gray-100 shadow-sm flex items-center gap-4 transition-all hover:border-emerald-200"
          :class="c.selected ? 'ring-2 ring-emerald-500 bg-emerald-50/10' : ''">
          
          <button @click="toggleSelect(c.id)" 
            :class="c.selected ? 'bg-emerald-500 border-emerald-500' : 'bg-gray-50 border-gray-200'" 
            class="w-6 h-6 rounded-full border-2 flex items-center justify-center transition-all shrink-0">
            <svg v-if="c.selected" class="w-3 h-3 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="4" d="M5 13l4 4L19 7"/></svg>
          </button>

          <div class="flex-1 truncate text-left">
            <h3 class="font-bold text-gray-900 uppercase text-[10px] tracking-tight truncate">{{ c.name }}</h3>
            <div class="flex items-center gap-2">
              <span class="text-[8px] font-bold text-emerald-600 uppercase italic">{{ c.dept || 'GÉNÉRAL' }}</span>
              <span class="text-gray-200">|</span>
              <p class="text-[9px] text-gray-400 font-medium truncate">{{ c.email }}</p>
            </div>
          </div>

          <button @click="deleteCandidate(c.id)" class="opacity-0 group-hover:opacity-100 p-2 text-gray-300 hover:text-red-500 transition-all">
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>
          </button>
        </div>
      </transition-group>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted, watch } from 'vue';

const showAddForm = ref(false);
const search = ref('');
const newCandidate = reactive({ name: '', email: '', dept: '' });

const candidates = ref([
  { id: 1, name: "Ahmed Ben Ali", email: "ahmed@mail.com", dept: "DESIGN", selected: false },
  { id: 2, name: "Sonia Mansour", email: "sonia.m@pro.tn", dept: "RH", selected: true },
  { id: 3, name: "Yassine Karoui", email: "y.karoui@dev.io", dept: "TECH", selected: false },
  { id: 4, name: "Ines Dridi", email: "ines.dridi@corp.com", dept: "MARKETING", selected: false }
]);

onMounted(() => {
  const saved = localStorage.getItem('candidates_v3');
  if (saved) candidates.value = JSON.parse(saved);
});

watch(candidates, (newVal) => {
  localStorage.setItem('candidates_v3', JSON.stringify(newVal));
}, { deep: true });

const filteredCandidates = computed(() => {
  return candidates.value.filter(c => c.name.toLowerCase().includes(search.value.toLowerCase()));
});

const handleAdd = () => {
  if (!newCandidate.name || !newCandidate.email) return alert("Nom et Email requis");
  candidates.value.unshift({
    id: Date.now(),
    name: newCandidate.name,
    email: newCandidate.email,
    dept: newCandidate.dept.toUpperCase(),
    selected: false
  });
  newCandidate.name = ''; newCandidate.email = ''; newCandidate.dept = '';
  showAddForm.value = false;
};

const toggleSelect = (id: number) => {
  const c = candidates.value.find(cand => cand.id === id);
  if (c) c.selected = !c.selected;
};

const deleteCandidate = (id: number) => {
  if(confirm("Supprimer ce candidat ?")) candidates.value = candidates.value.filter(c => c.id !== id);
};
</script>

<style scoped>
.custom-input {
  width: 100%;
  padding: 0.65rem 1rem;
  background-color: #f9fafb;
  border: 1px solid #f3f4f6;
  border-radius: 0.75rem;
  font-size: 10px;
  font-weight: 700;
  outline: none;
  transition: all 0.2s;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}
.custom-input:focus {
  box-shadow: 0 0 0 2px #34d399;
}

.expand-enter-active, .expand-leave-active { transition: all 0.3s ease-in-out; max-height: 500px; }
.expand-enter-from, .expand-leave-to { max-height: 0; opacity: 0; }

.list-enter-active, .list-leave-active { transition: all 0.4s ease; }
.list-enter-from { opacity: 0; transform: scale(0.9); }
.list-leave-to { opacity: 0; transform: translateX(20px); }
</style>