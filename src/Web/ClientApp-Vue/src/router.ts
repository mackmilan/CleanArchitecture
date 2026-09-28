import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { initializeAuth, isAuthenticated } from './composables/useAuth';
import CounterView from './views/CounterView.vue';
import HomeView from './views/HomeView.vue';
import LoginView from './views/LoginView.vue';
import RegisterView from './views/RegisterView.vue';
import TasksView from './views/TasksView.vue';
import WeatherView from './views/WeatherView.vue';

const routes: RouteRecordRaw[] = [
  { path: '/', component: HomeView },
  { path: '/counter', component: CounterView },
  { path: '/weather', component: WeatherView, meta: { requiresAuth: true } },
  { path: '/todo', component: TasksView, meta: { requiresAuth: true } },
  { path: '/login', component: LoginView },
  { path: '/register', component: RegisterView },
];

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach(async (to) => {
  if (!to.meta.requiresAuth) return true;

  await initializeAuth();
  if (isAuthenticated.value) return true;

  return {
    path: '/login',
    query: { returnUrl: to.fullPath },
  };
});
