<script setup lang="ts">
import type { TicketType } from '~/types'

const props = defineProps<{
  ticketTypes: TicketType[]
  eventId: string
}>()

const cart = useCartStore()
const quantities = ref<Record<string, number>>({})

const subtotal = computed(() =>
  props.ticketTypes.reduce((sum, t) => sum + (quantities.value[t.id] || 0) * t.price, 0)
)

function addToCart() {
  for (const ticket of props.ticketTypes) {
    const qty = quantities.value[ticket.id] || 0
    if (qty > 0) {
      cart.addItem({
        eventId: props.eventId,
        ticketTypeId: ticket.id,
        label: ticket.label,
        price: ticket.price,
        quantity: qty
      })
    }
  }
  quantities.value = {}
}
</script>

<template>
  <div>
    <div v-for="ticket in ticketTypes" :key="ticket.id">
      <label>
        {{ ticket.label }} – {{ ticket.price }} €
        <input type="number" min="0" :max="ticket.available" v-model.number="quantities[ticket.id]" />
      </label>
    </div>
    <p>Zwischensumme: {{ subtotal }} €</p>
    <button @click="addToCart">In den Warenkorb</button>
  </div>
</template>