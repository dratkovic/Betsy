<template>
    <p class="text-center mt-2 font-weight-black">Ticket</p>
    <v-container>
        <v-row v-for="(selectedBet, i) in playingTicketStore.selectedBets" :key="i" no-gutters>
            <v-col cols="3">
                <v-btn icon="mdi-window-close" size="x-small" @click="removeSelectedBet(selectedBet)"></v-btn>
            </v-col>
           
            <v-col cols="9">
                <p class="bet-text">{{ selectedBet.Offer.description }}</p>
                <p class="bet-text"><span class="font-weight-regular">{{ selectedBet.SelectedBetType.title }}</span> - <span class="text-red-darken-1">{{ selectedBet.SelectedBetType.quota }}</span></p>
            </v-col>
        </v-row>
    </v-container>
    <hr/>
    <v-container>
    <v-row v-if="playingTicketStore.selectedBets.length" no-gutters>
        <v-col class="text-right" cols="12">
            <v-text-field
            v-model.number="ticketAmount" 
            density="compact"
            hide-details="auto"
            label="Ticket Amount"
            prefix="€"
            step="0.1"
            type="number"
            >
            </v-text-field>            
        </v-col>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Processing fee: </span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info">{{ playingTicketStore.serviceCost }}</span>
        </v-col>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Bet amount:</span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info">{{ playingTicketStore.betAmount }}</span>
        </v-col>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Total odds:</span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info">{{ playingTicketStore.totalOdds }}</span>
        </v-col>
        <v-col class="text-right" cols="9">
            <span class="bet-info-label mr-3">Possible Win:</span>
        </v-col>
        <v-col class="text-right" cols="3">
            <span class="bet-info font-weight-black">{{ playingTicketStore.totalPossibleWin }}</span>
        </v-col>
        <v-col cols="12">
            <v-btn
                block
                class="mt-2"
                color="green-accent-2"
                variant="elevated"
              >
                Play
              </v-btn>
        </v-col>
        <v-col cols="12">
            <v-btn
                block
                class="mt-2"
                color="red-darken-1"
                variant="elevated"
                @click="playingTicketStore.clearTicket"
              >
                Cancel
              </v-btn>
        </v-col>
    </v-row>
</v-container>
</template>
<script lang="ts" setup>
import { usePlayingTicketStore } from '@/stores/playing-ticket.store';
import { SelectedBetType } from '@/types/selected-bet.type';
import { storeToRefs } from 'pinia';

const playingTicketStore = usePlayingTicketStore();
const { ticketAmount } = storeToRefs(playingTicketStore)
const someAmount = ref(0);

function removeSelectedBet(selectedBet: SelectedBetType) {
    playingTicketStore.removeSelectedBet(selectedBet);
}
</script>

<style scoped>
.bet-text{
    font-size: 0.7rem !important;
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

.bet-info{
}

</style>