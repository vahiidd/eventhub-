<script setup lang="ts">
import type { Event } from '~/types'

const config = useRuntimeConfig()
const route = useRoute()
const { data: eventItem, error } = await useFetch<Event>(
  `${config.public.eventServiceUrl}/api/events/${route.params.id}`
)
</script>

<template>
  <div v-if="error">Event nicht gefunden.</div>
  <div v-else-if="eventItem">
    <h1>{{ eventItem.title }}</h1>
    <p>{{ eventItem.description }}</p>
    <p>{{ eventItem.date }} – {{ eventItem.venue.name }}, {{ eventItem.venue.city }}</p>
    <BookingForm :ticket-types="eventItem.ticketTypes" :event-id="eventItem.id" />
  </div>
</template>