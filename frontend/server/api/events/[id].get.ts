import type { Event } from '~/types'

export default defineEventHandler((event): Event => {
  const id = getRouterParam(event, 'id')
  const found = mockEvents.find(e => e.id === id)
  if (!found) {
    throw createError({ statusCode: 404, statusMessage: 'Event nicht gefunden' })
  }
  return found
})