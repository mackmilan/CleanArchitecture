import { ref, watchEffect } from 'vue';

export type Theme = 'auto' | 'light' | 'dark';

const storageKey = 'picoColorScheme';
const savedTheme = localStorage.getItem(storageKey);
export const theme = ref<Theme>(
  savedTheme === 'light' || savedTheme === 'dark' ? savedTheme : 'auto',
);

watchEffect(() => {
  if (theme.value === 'auto') {
    document.documentElement.removeAttribute('data-theme');
  } else {
    document.documentElement.setAttribute('data-theme', theme.value);
  }

  localStorage.setItem(storageKey, theme.value);
});

export function toggleTheme(): void {
  theme.value = theme.value === 'auto'
    ? 'light'
    : theme.value === 'light'
      ? 'dark'
      : 'auto';
}
