<template>
  <div class="min-h-screen bg-white text-gray-900 font-sans selection:bg-emerald-100 italic selection:text-emerald-900">
    
    <!-- Navigation -->
    <nav class="fixed top-0 w-full z-50 bg-white/80 backdrop-blur-xl border-b border-gray-100 px-6 py-4 flex items-center justify-between">
      <div class="flex items-center gap-2">
        <div class="w-8 h-8 bg-emerald-600 rounded-lg flex items-center justify-center text-white font-black text-xs rotate-3 shadow-lg shadow-emerald-200">AT</div>
        <span class="text-lg font-black tracking-tighter uppercase italic text-gray-900">App<span class="text-emerald-600">Test</span></span>
      </div>
      
      <div class="hidden md:flex items-center gap-8">
        <a href="#solutions" class="text-[10px] font-bold uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Solutions</a>
        <a href="#features" class="text-[10px] font-bold uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">Fonctionnalités</a>
        <a href="#about" class="text-[10px] font-bold uppercase tracking-widest text-gray-400 hover:text-emerald-600 transition-colors">À Propos</a>
      </div>

      <button 
        @click="goTo('/auth')" 
        class="px-6 py-2.5 bg-gray-900 text-white rounded-full text-[10px] font-black uppercase tracking-widest shadow-xl shadow-gray-200 hover:scale-105 transition-all active:scale-95 cursor-pointer"
      >
        Connexion
      </button>
    </nav>

    <!-- Hero Section -->
    <main class="relative pt-40 pb-20 overflow-hidden">
      <!-- Background Elements -->
      <div class="absolute top-0 left-1/2 -translate-x-1/2 w-[1200px] h-[600px] bg-emerald-50/50 rounded-[100%] blur-3xl -z-10 opacity-60"></div>
      
      <div class="max-w-7xl mx-auto px-6 text-center space-y-12">
        <div class="inline-flex items-center gap-2 px-4 py-1.5 bg-emerald-100/50 border border-emerald-200 rounded-full text-[9px] font-black text-emerald-700 uppercase tracking-widest animate-pulse">
           Nouveau : Recrutement Intelligent
        </div>

        <h1 class="text-5xl md:text-8xl font-black text-gray-900 tracking-tighter leading-[0.9] italic max-w-4xl mx-auto uppercase">
          L'Évaluation <span class="text-emerald-600">Augmentée</span> pour vos équipes.
        </h1>

        <p class="text-base text-gray-500 font-bold max-w-xl mx-auto uppercase leading-relaxed tracking-tight">
          Simplifiez votre processus de recrutement avec des tests techniques précis et une plateforme collaborative intuitive.
        </p>

        <div class="flex flex-col sm:flex-row items-center justify-center gap-4 pt-6">
          <button 
            @click="goTo('/auth/register')" 
            class="w-full sm:w-auto px-8 py-4 bg-emerald-600 text-white rounded-2xl text-xs font-black uppercase tracking-widest shadow-2xl shadow-emerald-200 hover:bg-emerald-700 transition-all active:scale-95 flex items-center justify-center gap-3 cursor-pointer"
          >
            Commencer Gratuitement
            <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path d="M5 12h14m-7-7l7 7-7 7" stroke-width="3"/></svg>
          </button>
          <a href="#solutions" class="w-full sm:w-auto px-8 py-4 bg-white border border-gray-100 text-gray-900 rounded-2xl text-xs font-black uppercase tracking-widest shadow-xl shadow-gray-100 hover:border-gray-200 transition-all active:scale-95">
            Voir Démo
          </a>
        </div>
      </div>
    </main>

    <!-- Stats Section -->
    <section class="py-20 bg-gray-50/50 flex flex-wrap justify-center gap-12 md:gap-32 items-center px-6">
      <div class="text-center group">
        <p class="text-4xl font-black text-gray-900 italic tracking-tighter group-hover:text-emerald-600 transition-colors">500+</p>
        <p class="text-[9px] font-black text-gray-400 uppercase tracking-widest mt-1">Entreprises</p>
      </div>
      <div class="text-center group">
        <p class="text-4xl font-black text-gray-900 italic tracking-tighter group-hover:text-emerald-600 transition-colors">10k+</p>
        <p class="text-[9px] font-black text-gray-400 uppercase tracking-widest mt-1">Candidats</p>
      </div>
      <div class="text-center group">
        <p class="text-4xl font-black text-gray-900 italic tracking-tighter group-hover:text-emerald-600 transition-colors">98%</p>
        <p class="text-[9px] font-black text-gray-400 uppercase tracking-widest mt-1">Satisfaction</p>
      </div>
    </section>

    <!-- Feature Grid -->
    <section id="solutions" class="py-32 max-w-7xl mx-auto px-6 space-y-20">
      <div class="text-center space-y-4">
        <h2 class="text-3xl md:text-5xl font-black tracking-tighter uppercase italic">Nos <span class="text-emerald-600">Solutions</span></h2>
        <p class="text-[10px] text-gray-400 font-bold uppercase tracking-[0.3em]">Tout ce dont vous avez besoin pour recruter</p>
      </div>

      <div class="grid md:grid-cols-3 gap-8">
        <div v-for="(feature, i) in features" :key="i" class="p-8 bg-white border border-gray-100 rounded-[2.5rem] shadow-sm hover:shadow-2xl hover:shadow-emerald-100/50 transition-all group">
          <div class="w-12 h-12 bg-gray-50 rounded-2xl flex items-center justify-center text-gray-900 mb-6 group-hover:bg-emerald-600 group-hover:text-white transition-colors duration-500">
            <component :is="feature.icon" class="w-6 h-6" />
          </div>
          <h3 class="text-lg font-black uppercase italic tracking-tight mb-3 text-gray-900">{{ feature.title }}</h3>
          <p class="text-xs font-bold text-gray-500 leading-relaxed uppercase">{{ feature.desc }}</p>
        </div>
      </div>
    </section>

    <!-- Footer -->
    <footer class="py-20 border-t border-gray-100 bg-white">
      <div class="max-w-7xl mx-auto px-6 flex flex-col md:flex-row justify-between items-center gap-12">
        <div class="flex items-center gap-2">
          <div class="w-6 h-6 bg-gray-900 rounded flex items-center justify-center text-white font-black text-[9px]">AT</div>
          <span class="text-xs font-black tracking-tighter uppercase italic text-gray-900">App<span class="text-emerald-600">Test</span></span>
        </div>
        
        <div class="flex gap-8">
           <a href="#" class="text-[9px] font-black text-gray-400 uppercase hover:text-emerald-600 tracking-widest transition-colors">Twitter</a>
           <a href="#" class="text-[9px] font-black text-gray-400 uppercase hover:text-emerald-600 tracking-widest transition-colors">LinkedIn</a>
           <a href="#" class="text-[9px] font-black text-gray-400 uppercase hover:text-emerald-600 tracking-widest transition-colors">Privacy</a>
        </div>

        <p class="text-[9px] font-black text-gray-300 uppercase tracking-widest italic">© 2026 AppTest. All control reserved.</p>
      </div>
    </footer>

  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
import { 
  BriefcaseIcon, 
  UserGroupIcon, 
  CodeBracketIcon 
} from '@heroicons/vue/24/outline';

const router = useRouter();

const goTo = (path) => {
  router.push(path).catch(err => {
    console.error('Navigation Error:', err);
    window.location.href = path; // Fallback if router fails
  });
};

// Mocking icons if not installed to avoid breakage
const features = [
  {
    title: "Tests Techniques",
    desc: "Évaluez les compétences réelles avec des environnements de codage en direct.",
    icon: CodeBracketIcon
  },
  {
    title: "Gestion Candidat",
    desc: "Suivez chaque étape du processus de recrutement en un clin d'œil.",
    icon: UserGroupIcon
  },
  {
    title: "Support Entreprise",
    desc: "Espaces dédiés pour les recruteurs et managers avec des analytics avancés.",
    icon: BriefcaseIcon
  }
];
</script>

<style scoped>
html { scroll-behavior: smooth; }
@keyframes float {
  0% { transform: translateY(0px); }
  50% { transform: translateY(-10px); }
  100% { transform: translateY(0px); }
}
.hero-float { animation: float 6s ease-in-out infinite; }
</style>
