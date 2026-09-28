<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { WeatherForecastsClient } from '../web-api-client';

const client = new WeatherForecastsClient();
type Forecast = Awaited<ReturnType<WeatherForecastsClient['getWeatherForecasts']>>[number];

const forecasts = ref<Forecast[]>([]);
const loading = ref(true);
const error = ref('');

onMounted(async () => {
  try {
    forecasts.value = await client.getWeatherForecasts();
  } catch {
    error.value = 'Unable to load weather forecasts. Please try again later.';
  } finally {
    loading.value = false;
  }
});
</script>

<template>
  <section>
    <h1>Weather</h1>
    <p>This component demonstrates fetching data from the server with the generated API client.</p>
    <span v-if="loading" aria-busy="true">Fetching your weather forecast…</span>
    <p v-else-if="error" class="error" role="alert">{{ error }}</p>
    <table v-else>
      <thead>
        <tr><th>Date</th><th>Temp. (C)</th><th>Temp. (F)</th><th>Summary</th></tr>
      </thead>
      <tbody>
        <tr v-for="(forecast, index) in forecasts" :key="forecast.date?.toISOString() ?? index">
          <td>{{ new Date(forecast.date ?? 0).toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' }) }}</td>
          <td>{{ forecast.temperatureC ?? '—' }}</td>
          <td>{{ forecast.temperatureF ?? '—' }}</td>
          <td>{{ forecast.summary ?? '' }}</td>
        </tr>
      </tbody>
    </table>
  </section>
</template>
