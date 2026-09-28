import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

const target =
  process.env['services__webapi__https__0'] ||
  process.env['services__webapi__http__0'];

const proxyOptions = target
  ? { target, secure: false, changeOrigin: true }
  : undefined;

export default defineConfig({
  plugins: [vue()],
  server: {
    port: process.env.PORT ? Number(process.env.PORT) : 5173,
    proxy: proxyOptions
      ? {
          '/api': proxyOptions,
          '/openapi': proxyOptions,
          '/scalar': proxyOptions,
          '/weatherforecast': proxyOptions,
          '/WeatherForecast': proxyOptions,
        }
      : undefined,
  },
  build: {
    outDir: 'build',
  },
});
