<template>
  <div class="min-h-screen bg-gray-50 p-8 space-y-8">
    <header class="flex justify-between items-end">
      <div>
        <h2 class="text-3xl font-black text-gray-900 uppercase italic tracking-tighter">Tableau de Bord <span class="text-emerald-600">Entreprise</span></h2>
        <p class="text-[10px] text-gray-400 mt-1 font-bold uppercase tracking-[0.2em]">Gestion de vos recrutements et tests</p>
      </div>
      <div class="flex gap-3">
        <router-link to="/adminEntreprise/jobs" class="h-11 px-6 bg-gray-900 text-white rounded-2xl font-black uppercase tracking-widest text-[9px] shadow-xl hover:scale-105 transition-all flex items-center justify-center">
          + Nouvelle Offre
        </router-link>
      </div>
    </header>

    <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
      <div v-for="stat in stats" :key="stat.label" class="bg-white p-8 rounded-[2.5rem] border border-gray-100 shadow-sm group">
        <p class="text-[9px] font-black text-gray-400 uppercase tracking-widest mb-2 group-hover:text-emerald-600 transition-colors">{{ stat.label }}</p>
        <div class="flex items-end gap-2">
          <span class="text-5xl font-black text-gray-900 leading-none tracking-tighter italic">{{ stat.value }}</span>
        </div>
      </div>
    </div>

    <div class="bg-white rounded-[2.5rem] border border-gray-100 shadow-sm overflow-hidden">
      <div class="p-6 border-b border-gray-50 flex items-center justify-between">
        <h3 class="text-sm font-black text-gray-900 uppercase italic tracking-tight">Candidats <span class="text-emerald-600">Récents</span></h3>
        <button class="text-[10px] font-black text-gray-400 uppercase hover:text-emerald-600">Voir tout</button>
      </div>
      <div class="p-12 text-center">
        <p class="text-xs font-bold text-gray-300 uppercase italic tracking-widest">Aucune activité récente pour le moment.</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const stats = [
  { label: 'Offres Actives', value: '0' },
  { label: 'Tests Envoyés', value: '0' },
  { label: 'Candidats', value: '0' },
];
</script>