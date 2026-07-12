import type { Event } from '~/types'

export const mockEvents: Event[] = [
  {
    id: '1',
    title: 'Nuxt Nation Conf',
    description: 'Ein Tag rund um Nuxt und Vue.',
    date: '2026-09-12',
    venue: { id: 'v1', name: 'Tech Hall', city: 'Berlin' },
    ticketTypes: [
      { id: 't1', label: 'Standard', price: 49, available: 120 },
      { id: 't2', label: 'VIP', price: 129, available: 20 }
    ]
  },
  {
    id: '2',
    title: 'C# Deep Dive',
    description: 'Workshop zu modernem .NET.',
    date: '2026-10-03',
    venue: { id: 'v2', name: 'Dev Center', city: 'Frankfurt' },
    ticketTypes: [
      { id: 't3', label: 'Standard', price: 39, available: 80 }
    ]
  }
]
