<template>
      <v-card
        class="mx-auto"
        color="surface-variant"
        
      >
        <v-card-title>
          <p class="text-center ">Possible winning: {{ ticket.potentialPayout }}</p>
        </v-card-title  >
        <v-card-subtitle>
          <v-row v-for="(betType,i) in ticket.betTypes" :key="i" justify="space-between" no-gutters>
            <v-col cols="12">
                <p class="font-weight-bold">{{ betType.match.description }}</p>
            </v-col>
            <v-col cols="12">
                <p class="bet-text text-right"><span class="font-weight-regular">{{ betType.title }}</span> - <span class="font-weight-bold text-red-darken-1">{{ betType.quota }}</span></p>
            </v-col>

          </v-row>
          
        </v-card-subtitle>
        <v-card-actions>
            <hr/>
        <v-row no-gutters>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Ticket amount: </span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info">{{ ticket.ticketAmount }}</span>
        </v-col>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Processing fee: </span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info">{{ ticket.vig }}</span>
        </v-col>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Bet amount:</span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info">{{ ticket.stake }}</span>
        </v-col>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Total odds:</span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info">{{ ticket.totalQuota }}</span>
        </v-col>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Possible Win:</span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info font-weight-black">{{ ticket.potentialPayout}}</span>
        </v-col>

    </v-row>
        </v-card-actions>
      </v-card>
</template>
<script lang="ts" setup>
import { TicketInfo } from '@/types/types.betsy';


defineProps({
  ticket: {
    type: Object as PropType<TicketInfo>,
    required: true,
  },
});

</script>

<style scoped>
.bet-text{
    font-size: 0.9rem !important;
    font-weight: 800;
    letter-spacing: 0.03125em !important;
    font-family: "Roboto", sans-serif;
    text-transform: none !important;
    text-align: right;
}

.bet-info-label{
    font-size: 0.7rem !important;
    line-height: 0;
    letter-spacing: 0.03125em !important;
    font-family: "Roboto", sans-serif;
    text-transform: none !important;
}

</style>