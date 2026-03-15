<template>
  <div class="min-h-screen bg-[#F8FAFC] text-[#0A0B0C] font-sans selection:bg-emerald-100">
    <div class="max-w-[1200px] mx-auto p-6 space-y-6">
      
      <header class="flex flex-col md:flex-row md:items-center justify-between gap-4 pb-4 border-b border-gray-200/60">
        <div class="flex items-center gap-3">
          <div class="w-11 h-11 bg-emerald-600 rounded-lg flex items-center justify-center shadow-lg shadow-emerald-100 rotate-1 border border-emerald-500">
            <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/>
            </svg>
          </div>
          <div>
            <h1 class="text-lg font-bold tracking-tight uppercase italic leading-none">
              Suivi <span class="text-emerald-600">Candidats</span>
            </h1>
            <p class="text-[9px] font-bold uppercase tracking-widest text-gray-400 mt-1 flex items-center gap-1.5">
              <span class="w-1 h-1 bg-emerald-500 rounded-full"></span> 
              Base de données talents
            </p>
          </div>
        </div>

        <div class="relative w-full md:w-80 group">
          <div class="absolute inset-y-0 left-3.5 flex items-center pointer-events-none z-10">
            <svg class="w-4 h-4 text-gray-300 group-focus-within:text-emerald-500 transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/>
            </svg>
          </div>
          <input 
            v-model="searchQuery" 
            type="text" 
            placeholder="Rechercher..." 
            class="w-full pl-9 pr-4 py-2.5 bg-white border border-gray-200 rounded-xl text-[11px] font-bold uppercase tracking-wider focus:ring-2 focus:ring-emerald-100 outline-none transition-all shadow-sm italic placeholder:font-normal placeholder:not-italic"
          />
        </div>
      </header>

      <main class="bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden">
        <div class="overflow-x-auto">
          <table class="w-full text-left border-collapse">
            <thead>
              <tr class="bg-gray-50/50 border-b border-gray-100">
                <th class="px-6 py-4 text-[10px] font-bold text-gray-400 uppercase tracking-widest">Candidat</th>
                <th class="px-6 py-4 text-[10px] font-bold text-gray-400 uppercase tracking-widest">Email</th>
                <th class="px-6 py-4 text-[10px] font-bold text-gray-400 uppercase tracking-widest">Score</th>
                <th class="px-6 py-4 text-[10px] font-bold text-gray-400 uppercase tracking-widest text-center">Actions</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-gray-50">
              <tr v-for="candidate in filteredCandidates" :key="candidate.id" class="group hover:bg-emerald-50/30 transition-colors">
                <td class="px-6 py-4">
                  <div class="flex items-center gap-3">
                    <div class="w-8 h-8 rounded bg-gray-900 text-white flex items-center justify-center text-[10px] font-bold uppercase italic shadow-sm">
                      {{ candidate.name.split(' ').map(n => n[0]).join('') }}
                    </div>
                    <span class="text-[12px] font-bold text-gray-800 uppercase italic">{{ candidate.name }}</span>
                  </div>
                </td>
                <td class="px-6 py-4 text-[12px] font-medium text-gray-500 lowercase">{{ candidate.email }}</td>
                <td class="px-6 py-4">
                  <div class="flex items-center gap-3">
                    <div class="flex-1 h-1.5 bg-gray-100 rounded-full max-w-[100px] overflow-hidden">
                      <div 
                        class="h-full bg-emerald-500 rounded-full" 
                        :style="{ width: candidate.score + '%' }"
                      ></div>
                    </div>
                    <span class="text-[10px] font-bold text-emerald-600">{{ candidate.score }}%</span>
                  </div>
                </td>
                <td class="px-6 py-4 text-center">
                  <button 
                    @click="openResume(candidate)" 
                    class="p-2 bg-white border border-gray-200 rounded-lg text-gray-400 hover:text-emerald-600 hover:border-emerald-200 hover:shadow-sm transition-all group/btn"
                  >
                    <svg class="w-4 h-4 group-hover/btn:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/>
                    </svg>
                  </button>
                </td>
              </tr>

              <tr v-if="filteredCandidates.length === 0">
                <td colspan="4" class="px-6 py-16 text-center">
                  <div class="inline-flex items-center justify-center w-14 h-14 bg-gray-50 rounded-full border border-gray-100 mb-4">
                    <svg class="w-6 h-6 text-gray-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/>
                    </svg>
                  </div>
                  <p class="text-[10px] font-bold text-gray-300 uppercase tracking-widest italic">Aucun résultat trouvé</p>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';

interface Candidate {
  id: number;
  name: string;
  email: string;
  score: number;
  fileUrl: string;
}

const searchQuery = ref('');

const candidates = ref<Candidate[]>([
  { id: 1, name: "Youssef Ben Ammar", email: "youssef.ba@sfax.dev", score: 91, fileUrl: "dossier_youssef.txt" },
  { id: 2, name: "Aicha Trabelsi", email: "aicha.trabelsi@net.co", score: 96, fileUrl: "Aicha_Trabelsi_Resume.txt" },
  { id: 3, name: "Hamza Lassoued", email: "hamza.l@cloud.co.tn", score: 45, fileUrl: "hamza-lassoued-resume.txt" },
  { id: 4, name: "Rim El Feki", email: "rim.f@it-corp.tn", score: 82, fileUrl: "rim_feki_cv.txt" },
  { id: 5, name: "Omar Gharbi", email: "gharbi.omar@startup.tn", score: 68, fileUrl: "gharbi-omar.txt" },
]);

const filteredCandidates = computed(() => {
  const query = searchQuery.value.toLowerCase().trim();
  if (!query) return candidates.value;
  
  return candidates.value.filter(c => 
    c.name.toLowerCase().includes(query) || 
    c.email.toLowerCase().includes(query) ||
    c.score.toString().includes(query)
  );
});

const openResume = (candidate: Candidate) => {
  alert(`Ouverture du dossier : ${candidate.name}`);
};
</script>

<style scoped>
.custom-scroll::-webkit-scrollbar { width: 4px; }
.custom-scroll::-webkit-scrollbar-thumb { background: #e2e8f0; border-radius: 10px; }
</style>