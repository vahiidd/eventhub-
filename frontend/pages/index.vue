<script setup lang="ts">
import type { Event, PagedResult } from '~/types'

const config = useRuntimeConfig()
const { data, pending, error } = await useFetch<PagedResult<Event>>(
  `${config.public.eventServiceUrl}/api/events`
)
const events = computed(() => data.value?.items ?? [])
</script>

<template>
  <div>
    <h1>Events</h1>
    <p v-if="pending">Lade Events...</p>
    <p v-else-if="error">Fehler beim Laden.</p>
    <ul v-else>
      <li v-for="event in events" :key="event.id">
        <NuxtLink :to="`/events/${event.id}`">
          {{ event.title }} – {{ event.venue.city }}
        </NuxtLink>
      </li>
    </ul>
  </div>
</template>