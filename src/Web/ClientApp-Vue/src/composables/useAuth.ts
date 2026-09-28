import { ref } from 'vue';
import { LoginRequest, RegisterRequest, UsersClient } from '../web-api-client';

const client = new UsersClient();

export const isAuthenticated = ref(false);
export const isLoading = ref(true);

let initialization: Promise<void> | undefined;

export function initializeAuth(): Promise<void> {
  initialization ??= client.infoGET()
    .then(() => {
      isAuthenticated.value = true;
    })
    .catch(() => {
      isAuthenticated.value = false;
    })
    .finally(() => {
      isLoading.value = false;
    });

  return initialization;
}

export function useAuth() {
  async function login(email: string, password: string): Promise<void> {
    await client.login(true, undefined, new LoginRequest({ email, password }));
    isAuthenticated.value = true;
  }

  async function register(email: string, password: string): Promise<void> {
    await client.register(new RegisterRequest({ email, password }));
  }

  async function logout(): Promise<void> {
    await client.logout({});
    isAuthenticated.value = false;
  }

  return { isAuthenticated, isLoading, initializeAuth, login, register, logout };
}
