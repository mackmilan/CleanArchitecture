<script setup lang="ts">
import { computed, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuth } from '../composables/useAuth';

const minimumPasswordLength = 6;
const email = ref('');
const password = ref('');
const emailTouched = ref(false);
const passwordTouched = ref(false);
const error = ref('');
const { register } = useAuth();
const router = useRouter();

const emailValid = computed(() => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value));
const passwordValid = computed(() => password.value.length >= minimumPasswordLength);

async function handleSubmit(): Promise<void> {
  error.value = '';
  emailTouched.value = true;
  passwordTouched.value = true;
  if (!emailValid.value || !passwordValid.value) return;

  try {
    await register(email.value, password.value);
    await router.push('/login');
  } catch {
    error.value = 'Registration failed. Please try again.';
  }
}
</script>

<template>
  <article>
    <h2>Register</h2>
    <p v-if="error" class="error" role="alert">{{ error }}</p>
    <form @submit.prevent="handleSubmit">
      <label for="email">Email</label>
      <input
        id="email"
        v-model="email"
        type="email"
        autocomplete="username"
        :aria-invalid="emailTouched && !emailValid ? true : undefined"
        aria-describedby="email-helper"
        required
        @blur="emailTouched = true"
      />
      <small id="email-helper">{{ emailTouched && !emailValid ? 'Please enter a valid email address.' : '' }}</small>
      <label for="password">Password</label>
      <input
        id="password"
        v-model="password"
        type="password"
        autocomplete="new-password"
        :aria-invalid="passwordTouched && !passwordValid ? true : undefined"
        aria-describedby="password-helper"
        required
        @blur="passwordTouched = true"
      />
      <small id="password-helper">
        {{ passwordTouched && !passwordValid ? `Password must be at least ${minimumPasswordLength} characters.` : '' }}
      </small>
      <button type="submit">Register</button>
      <p class="form-footer">Already have an account? <RouterLink to="/login">Log in</RouterLink></p>
    </form>
  </article>
</template>
