export interface Venue {
  id: string
  name: string
  city: string
}

export interface TicketType {
  id: string
  label: string
  price: number
  available: number
}

export interface Event {
  id: string
  title: string
  description: string
  date: string
  venue: Venue
  ticketTypes: TicketType[]
}

export interface CartItem {
  eventId: string
  ticketTypeId: string
  label: string
  price: number
  quantity: number
}

export interface PagedResult<T> {
  items: T[]
  total: number
  page: number
  pageSize: number
}