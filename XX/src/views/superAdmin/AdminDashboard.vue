<template>
  <div class="min-h-screen bg-[#FBFBFC] text-[#1A1C1E] font-sans selection:bg-emerald-100 selection:text-emerald-900 flex">
    
    <aside class="w-72 bg-white border-r border-gray-100 flex flex-col h-screen sticky top-0">
      <div class="p-8">
        <router-link to="/" class="group flex items-center gap-2 mb-6">
          <div class="w-6 h-6 bg-emerald-600 rounded flex items-center justify-center text-white font-black text-[9px] rotate-3 group-hover:rotate-0 transition-transform">AT</div>
          <span class="text-xs font-black tracking-tighter uppercase italic text-gray-900">App<span class="text-emerald-600">Test</span></span>
        </router-link>
        <h1 class="text-xl font-bold text-gray-900 uppercase tracking-tight italic leading-none">
          Console <span class="text-emerald-600">Admin</span>
        </h1>
        <p class="text-[9px] font-bold uppercase tracking-widest text-gray-400 mt-2 flex items-center gap-2">
          <span class="w-1.5 h-1.5 bg-emerald-500 rounded-full animate-pulse"></span> Master Control
        </p>
      </div>

      <nav class="flex-1 px-4 space-y-2">
        <button v-for="item in menu" :key="item.id" @click="currentTab = item.id"
          :class="currentTab === item.id ? 'bg-gray-900 text-white shadow-xl translate-x-1' : 'text-gray-400 hover:bg-gray-50 hover:text-gray-900'"
          class="w-full flex items-center gap-4 px-5 py-3.5 rounded-2xl transition-all duration-300 group">
          <span class="text-[10px] font-bold uppercase tracking-[0.15em]">{{ item.label }}</span>
        </button>
      </nav>

      <div class="p-6 border-t border-gray-50">
        <div class="bg-gray-50 p-4 rounded-3xl flex items-center gap-3 text-left border border-gray-100">
          <div class="w-10 h-10 rounded-2xl bg-emerald-600 flex items-center justify-center text-white text-[10px] font-black italic shadow-lg shadow-emerald-100">
            {{ user?.nom?.substring(0,2).toUpperCase() || 'AD' }}
          </div>
          <div class="min-w-0">
            <p class="text-[10px] font-black text-gray-900 uppercase leading-none truncate">{{ user?.nom || 'Super Admin' }}</p>
            <button @click="handleLogout" class="text-[9px] font-black text-emerald-600 uppercase mt-1 cursor-pointer hover:text-red-500 transition-colors italic border-b border-emerald-100 hover:border-red-100">
              Déconnexion
            </button>
          </div>
        </div>
      </div>
    </aside>

    <main class="flex-1 p-10 max-w-6xl mx-auto w-full">
      <transition name="fade" mode="out-in">
        <section v-if="currentTab === 'companies'" class="space-y-8">
          
          <header class="flex justify-between items-end">
            <div>
              <h2 class="text-3xl font-black text-gray-900 uppercase italic tracking-tighter">Gestion <span class="text-emerald-600">Entreprises</span></h2>
              <p class="text-[10px] text-gray-400 mt-1 font-bold uppercase tracking-[0.2em]">Flux d'adhésion et validation système</p>
            </div>
            <button class="h-11 px-6 bg-gray-900 text-white rounded-2xl font-black uppercase tracking-widest text-[9px] shadow-2xl shadow-gray-200 hover:scale-105 transition-all active:scale-95 border-b-4 border-gray-700">
              + Nouvelle Entreprise
            </button>
          </header>

          <div class="grid grid-cols-3 gap-6">
            <div v-for="stat in stats" :key="stat.label" class="bg-white p-8 rounded-[2rem] border border-gray-100 shadow-sm hover:shadow-xl transition-shadow group">
              <p class="text-[9px] font-black text-gray-300 uppercase tracking-widest mb-2 group-hover:text-emerald-600 transition-colors">{{ stat.label }}</p>
              <div class="flex items-end gap-2">
                <span class="text-5xl font-black text-gray-900 leading-none tracking-tighter italic">{{ stat.value }}</span>
                <span class="text-[10px] font-bold text-emerald-500 mb-1">+0%</span>
              </div>
            </div>
          </div>

          <div class="bg-white rounded-3xl border border-gray-100 shadow-sm overflow-hidden">
            <div class="p-6 border-b border-gray-50 flex items-center gap-3">
              <svg class="w-5 h-5 text-indigo-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
              </svg>
              <h3 class="text-sm font-black text-gray-900 uppercase italic tracking-tight">Demandes d'accès <span class="text-indigo-600">récentes</span></h3>
            </div>

            <div class="overflow-x-auto">
              <table class="w-full text-left">
                <thead class="bg-gray-50/50">
                  <tr>
                    <th class="px-6 py-4 text-[9px] font-black text-gray-400 uppercase tracking-widest">Entreprise</th>
                    <th class="px-6 py-4 text-[9px] font-black text-gray-400 uppercase tracking-widest text-center">Email</th>
                    <th class="px-6 py-4 text-[9px] font-black text-gray-400 uppercase tracking-widest text-right">Action</th>
                  </tr>
                </thead>
                <tbody class="divide-y divide-gray-50">
                  <tr v-if="isLoading">
                    <td colspan="3" class="py-16 text-center">
                      <div class="flex flex-col items-center gap-3">
                        <div class="w-8 h-8 border-4 border-emerald-600 border-t-transparent rounded-full animate-spin"></div>
                        <p class="text-[10px] font-black text-gray-400 uppercase tracking-widest animate-pulse">Synchronisation avec le serveur...</p>
                      </div>
                    </td>
                  </tr>
                  <tr v-else-if="demandesAttente.length === 0">
                    <td colspan="3" class="py-16 text-center text-[11px] font-bold text-gray-300 uppercase italic tracking-widest">
                      Aucune demande en attente.
                    </td>
                  </tr>
                  <tr v-for="req in demandesAttente" :key="req.id" class="hover:bg-gray-50/50 transition-colors">
                    <td class="px-6 py-4 text-xs font-black text-gray-800 italic uppercase">{{ req.nomEntreprise }}</td>
                    <td class="px-6 py-4 text-xs font-bold text-gray-500 text-center">{{ req.emailResponsable }}</td>
                    <td class="px-6 py-4 text-right space-x-2">
                      <button @click="validerDemande(req)" class="px-3 py-1.5 bg-emerald-500 text-white text-[9px] font-black rounded-lg hover:bg-emerald-600 transition-all uppercase italic">Valider</button>
                      <button @click="refuserDemande(req.id)" class="px-3 py-1.5 bg-white border border-red-100 text-red-500 text-[9px] font-black rounded-lg hover:bg-red-50 transition-all uppercase italic">Refuser</button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <div class="bg-white rounded-3xl border border-gray-100 shadow-sm overflow-hidden">
             <div class="p-4 border-b border-gray-50 flex justify-between items-center bg-gray-50/30">
                <p class="text-[10px] font-black text-gray-400 uppercase tracking-widest px-2">Liste des partenaires validés</p>
                <div class="relative w-64 group">
                  <input v-model="searchQuery" type="text" placeholder="FILTRER..." 
                    class="w-full pl-10 pr-4 py-2 bg-white border border-gray-100 rounded-xl text-[9px] font-bold uppercase outline-none focus:ring-2 focus:ring-emerald-500/20 transition-all shadow-sm" />
                  <svg class="w-3.5 h-3.5 absolute left-3.5 top-2.5 text-gray-300" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-width="3"/></svg>
                </div>
              </div>
              <table class="w-full">
                <tbody class="divide-y divide-gray-50">
                  <tr v-for="company in filteredCompanies" :key="company.id" class="hover:bg-gray-50/50 transition-colors group">
                    <td class="px-6 py-4">
                      <div class="flex items-center gap-4">
                        <div class="w-8 h-8 rounded-lg bg-gray-900 flex items-center justify-center text-white text-[9px] font-black">{{ company.name.substring(0,2).toUpperCase() }}</div>
                        <p class="text-xs font-bold text-gray-800 uppercase italic">{{ company.name }}</p>
                      </div>
                    </td>
                    <td class="px-6 py-4">
                      <div class="flex items-center gap-2">
                        <span class="w-1.5 h-1.5 rounded-full bg-emerald-500"></span>
                        <span class="text-[9px] font-bold uppercase text-emerald-700 tracking-widest">Actif</span>
                      </div>
                    </td>
                    <td class="px-6 py-4 text-right">
                      <button @click="toggleCompany(company.id)" class="text-[9px] font-black text-red-400 uppercase hover:text-red-600 transition-colors">Révoquer</button>
                    </td>
                  </tr>
                </tbody>
              </table>
          </div>
        </section>

        <section v-else class="max-w-3xl mx-auto space-y-8">
           <header>
            <h2 class="text-2xl font-bold text-gray-900 uppercase italic tracking-tighter">System <span class="text-emerald-600">Config</span></h2>
          </header>
          <div class="bg-white rounded-3xl border border-gray-100 shadow-sm p-8">
             <p class="text-xs font-bold text-gray-400 uppercase italic">Configuration du serveur opérationnelle.</p>
          </div>
        </section>
      </transition>
    <!-- Modal Fallback Link (Si l'email échoue) -->
    <div v-if="showFallback" class="fixed inset-0 z-[100] flex items-center justify-center p-6 bg-gray-900/60 backdrop-blur-sm">
      <div class="bg-white rounded-[32px] shadow-2xl max-w-lg w-full p-8 border border-gray-100 animate-in fade-in zoom-in duration-300">
        <div class="w-16 h-16 bg-amber-50 rounded-2xl flex items-center justify-center mb-6">
          <svg class="w-8 h-8 text-amber-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>
        </div>
        
        <h3 class="text-xl font-black text-gray-900 uppercase italic tracking-tight mb-2">Attention : Email non envoyé</h3>
        <p class="text-[11px] font-bold text-gray-400 uppercase tracking-widest leading-relaxed mb-6">
          La demande a été validée, mais le serveur n'a pas pu envoyer l'email à <span class="text-gray-900">{{ fallbackData.email }}</span>. 
          <br><span class="text-red-500 mt-2 block">Erreur : {{ fallbackData.error }}</span>
        </p>

        <div class="bg-gray-50 rounded-2xl p-5 border border-gray-100 mb-6">
          <label class="text-[9px] font-black text-gray-400 uppercase tracking-[0.2em] block mb-2">Lien d'activation manuel :</label>
          <div class="flex items-center gap-2">
            <input readonly :value="fallbackData.link" class="flex-1 bg-transparent border-none text-[10px] font-bold text-emerald-600 outline-none select-all" />
            <button @click="copyLink" class="p-2 hover:bg-emerald-50 rounded-xl transition-colors group">
              <svg class="w-4 h-4 text-emerald-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 5H6a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2v-1M8 5a2 2 0 002 2h2a2 2 0 002-2M8 5a2 2 0 012-2h2a2 2 0 012 2m0 0h2a2 2 0 012 2v3m2 4H10m0 0l3-3m-3 3l3 3" /></svg>
            </button>
          </div>
        </div>

        <button @click="showFallback = false" class="w-full py-4 bg-gray-900 text-white rounded-2xl text-[10px] font-black uppercase tracking-[0.2em] hover:bg-emerald-600 transition-all active:scale-95">
          J'ai copié le lien, fermer
        </button>
      </div>
    </div>
    </main>

    <!-- Système de Notifications (Toasts) -->
    <div class="fixed top-6 right-6 z-[200] space-y-3 w-80">
      <transition-group 
        enter-active-class="transform ease-out duration-300 transition"
        enter-from-class="translate-y-2 opacity-0 sm:translate-y-0 sm:translate-x-4"
        enter-to-class="translate-y-0 opacity-100 sm:translate-x-0"
        leave-active-class="transition ease-in duration-200"
        leave-from-class="opacity-100"
        leave-to-class="opacity-0"
      >
        <div v-for="note in notifications" :key="note.id" 
          class="bg-white/90 backdrop-blur-xl border border-gray-100 p-4 rounded-2xl shadow-2xl flex items-start gap-3 overflow-hidden relative group"
        >
          <!-- Progress bar -->
          <div class="absolute bottom-0 left-0 h-0.5 bg-emerald-500 transition-all duration-[5000ms] w-0 group-hover:w-full" :class="note.type === 'error' ? 'bg-red-500' : 'bg-emerald-500'"></div>
          
          <div :class="note.type === 'error' ? 'bg-red-50' : 'bg-emerald-50'" class="p-2 rounded-xl mt-0.5">
            <svg v-if="note.type === 'success'" class="w-4 h-4 text-emerald-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
            <svg v-else class="w-4 h-4 text-red-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
          </div>
          
          <div class="flex-1 min-w-0">
            <p class="text-[10px] font-black text-gray-900 uppercase italic">{{ note.title }}</p>
            <p class="text-[10px] font-bold text-gray-400 uppercase tracking-tight leading-tight">{{ note.message }}</p>
          </div>
          
          <button @click="notifications = notifications.filter(n => n.id !== note.id)" class="text-gray-300 hover:text-gray-500 transition-colors">
            <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
          </button>
        </div>
      </transition-group>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { authService } from '../../services/auth.service';
import { adminService } from '../../services/admin.service';

const router = useRouter();
const user = ref(null);
const isLoading = ref(false);
const error = ref('');
const showFallback = ref(false);
const fallbackData = ref({ link: '', email: '', error: '' });
const notifications = ref([]);

onMounted(async () => {
  user.value = authService.getCurrentUser();
  await loadDemandes();
});

const loadDemandes = async () => {
  try {
    isLoading.value = true;
    demandesAttente.value = await adminService.getPendingRegistrations();
  } catch (err) {
    console.error('Erreur chargement demandes:', err);
  } finally {
    isLoading.value = false;
  }
};

const handleLogout = () => {
  authService.logout();
  router.push('/');
};

const currentTab = ref('companies');
const menu = [
  { id: 'companies', label: 'Dashboard Entreprises' },
  { id: 'settings', label: 'Paramètres Système' }
];

// Données réelles des demandes d'accès
const demandesAttente = ref([]);

const companies = ref([
  { id: 1, name: 'Alpha Digit', email: 'admin@alpha.com', sector: 'Tech', active: true },
  { id: 2, name: 'Sky Build', email: 'hq@skybuild.io', sector: 'BTP', active: true },
]);

const stats = [
  { label: 'Total Entreprises', value: '12' },
  { label: 'Utilisateurs Actifs', value: '148' },
  { label: 'Demandes Reçues', value: '2' },
];

const searchQuery = ref('');
const filteredCompanies = computed(() => {
  return companies.value.filter(c => c.name.toLowerCase().includes(searchQuery.value.toLowerCase()));
});

// Actions
const validerDemande = async (req) => {
  try {
    isLoading.value = true;
    showFallback.value = false;
    const res = await adminService.approveRegistration(req.id);
    
    if (res.TechnicalError) {
      fallbackData.value = { 
        link: res.FallbackLink || '', 
        email: req.emailResponsable,
        error: res.TechnicalError 
      };
      showFallback.value = true;
    } else {
      showNotification('Success', `L'entreprise ${req.nomEntreprise} a été validée avec succès !`, 'success');
    }
    
    await loadDemandes(); // Recharger la liste
  } catch (err) {
    console.error('Erreur validation:', err);
    alert('Erreur lors de la validation.');
  } finally {
    isLoading.value = false;
  }
};

const refuserDemande = async (id) => {
  try {
    isLoading.value = true;
    await adminService.rejectRegistration(id, 'Refusé par l\'administrateur.');
    showNotification('Refusé', 'La demande a été rejetée.', 'error');
    await loadDemandes();
  } catch (err) {
    console.error('Erreur refus:', err);
    showNotification('Erreur', 'Impossible de traiter le refus.', 'error');
  } finally {
    isLoading.value = false;
  }
};

const toggleCompany = (id) => {
  companies.value = companies.value.filter(x => x.id !== id);
};

const copyLink = () => {
  navigator.clipboard.writeText(fallbackData.value.link);
  showNotification('Copié', 'Lien copié dans le presse-papier', 'success');
};

const showNotification = (title, message, type = 'success') => {
  const id = Date.now();
  notifications.value.push({ id, title, message, type });
  setTimeout(() => {
    notifications.value = notifications.value.filter(n => n.id !== id);
  }, 5000);
};
</script>

<style scoped>
.fade-enter-active, .fade-leave-active { transition: opacity 0.3s ease, transform 0.3s ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; transform: translateY(10px); }
</style>