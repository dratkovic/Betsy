<template>
    <v-main class="d-flex align-center justify-center" >
   <v-container class="scrollable-main" fluid>
    <v-row  >
        <v-col
        v-for="(ticket, i) in tickets"
        :key="i"
        cols="12"
        md="4"
        >
            <ticket :ticket="ticket" />
        </v-col>
  </v-row> 
</v-container> 
</v-main>
</template>

<script lang="ts" setup>
import ticketService from '@/services/ticket.service';
import { TicketInfo } from '@/types/types.betsy';

const tickets = ref<TicketInfo[]>([]);


  onMounted(() => {
    ticketService.getTickets({ "page": 1, "pageSize": 100 }).then((response) => {
      tickets.value = response.data;
    });
  });

</script>

<style scoped>
.scrollable-main {
  height: calc(100vh - 90px ); /* Adjust based on your app bar and footer height */
  overflow-y: auto;
}

.offer-btn-text-title{
    font-size: 0.7rem !important;
    font-weight: 800;
    line-height: 0;
    letter-spacing: 0.03125em !important;
    font-family: "Roboto", sans-serif;
    text-transform: none !important;
}

.offer-btn-text-quota{
    font-size: 0.7rem !important;
    font-weight: 800;
    line-height: 0;
    letter-spacing: 0.03125em !important;
    font-family: "Roboto", sans-serif;
    text-transform: none !important;
}
</style>
