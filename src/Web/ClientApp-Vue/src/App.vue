<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useAuth } from './composables/useAuth';
import { theme, toggleTheme } from './composables/useTheme';

const router = useRouter();
const { isAuthenticated, isLoading, initializeAuth, logout } = useAuth();
const themeLabel = computed(() => `${theme.value} color scheme`);
const themeIcon = computed(() => theme.value === 'auto' ? '◐' : theme.value === 'light' ? '☀' : '☾');

onMounted(() => void initializeAuth());

async function handleLogout(): Promise<void> {
  await logout();
  await router.push('/login');
}
</script>

<template>
  <header>
    <nav aria-label="Main navigation">
      <ul>
        <li><RouterLink class="brand" to="/">Clean Architecture</RouterLink></li>
      </ul>
      <ul>
        <li><RouterLink to="/">Home</RouterLink></li>
        <li><RouterLink to="/counter">Counter</RouterLink></li>
        <li><RouterLink to="/weather">Weather</RouterLink></li>
        <li><RouterLink to="/todo">Tasks</RouterLink></li>
      </ul>
      <ul>
        <li v-if="isLoading"><small aria-live="polite">Checking session…</small></li>
        <li v-else-if="isAuthenticated"><a href="#" @click.prevent="handleLogout">Log out</a></li>
        <template v-else>
          <li><RouterLink to="/login">Log in</RouterLink></li>
          <li><RouterLink to="/register">Register</RouterLink></li>
        </template>
        <li aria-hidden="true" class="nav-separator"></li>
        <li>
          <button class="theme-toggle-btn" type="button" :aria-label="themeLabel" @click="toggleTheme">
            {{ themeIcon }}
          </button>
        </li>
      </ul>
    </nav>
  </header>
  <main>
    <RouterView />
  </main>
</template>
