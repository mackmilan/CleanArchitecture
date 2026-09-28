<script setup lang="ts">
import { ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useAuth } from '../composables/useAuth';

const email = ref('');
const password = ref('');
const invalid = ref(false);
const { login } = useAuth();
const route = useRoute();
const router = useRouter();

async function handleSubmit(): Promise<void> {
  invalid.value = false;
  try {
    await login(email.value, password.value);
    const returnUrl = typeof route.query.returnUrl === 'string' ? route.query.returnUrl : '/';
    await router.replace(returnUrl);
  } catch {
    invalid.value = true;
  }
}
</script>

<template>
  <article>
    <h2>Log in</h2>
    <form @submit.prevent="handleSubmit">
      <label for="email">Email</label>
      <input
        id="email"
        v-model="email"
        type="email"
        autocomplete="username"
        :aria-invalid="invalid || undefined"
        :aria-describedby="invalid ? 'login-error' : undefined"
        required
      />
      <label for="password">Password</label>
      <input
        id="password"
        v-model="password"
        type="password"
        autocomplete="current-password"
        :aria-invalid="invalid || undefined"
        :aria-describedby="invalid ? 'login-error' : undefined"
        required
      />
      <small v-if="invalid" id="login-error" class="error">Invalid email or password.</small>
      <button type="submit">Log in</button>
      <p class="form-footer">Don't have an account? <RouterLink to="/register">Register</RouterLink></p>
    </form>
  </article>
</template>
