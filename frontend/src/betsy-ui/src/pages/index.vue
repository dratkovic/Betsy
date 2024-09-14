<template>
  <v-navigation-drawer permanent>
    <playing-ticket />
  </v-navigation-drawer>
  <v-main class="d-flex align-center justify-center">
    <v-container class="scrollable-main" fluid>
      <v-row justify="center">
        <v-btn class="mx-2" size="x-small" @click="setFilter('')">All</v-btn>
        <v-btn class="mx-2" size="x-small" @click="setFilter('football')">Football</v-btn>
        <v-btn class="mx-2" size="x-small" @click="setFilter('tennis')">Tennis</v-btn>
        <v-btn class="mx-2" size="x-small" @click="setFilter('special-offers')">Special Offers</v-btn>

      </v-row>
      <v-row>
        <v-col v-for="(offer, i) in offerStore.offers" :key="i" cols="12" md="4">
          <v-card class="mx-auto" color="surface-variant">
            <v-card-title>
              <p class="text-center text-body-1 ">{{ offer.description }}</p>
            </v-card-title>
            <v-card-subtitle>
              <h3 class="text-center ">{{ offer.sport }}</h3>
              <p class="text-center ">{{ toUIString(offer.startsAtUtc) }}</p>

            </v-card-subtitle>
            <v-card-actions>
              <v-row justify="space-around">
                <v-col v-for="(betType, j) in offer.bettingTypes" :key="j">
                  <v-btn block size="x-large" @click="selectBet(betType, offer)"
                    :class="{ 'selected-bet': isSelectedBet(betType) }">
                    <v-row>
                      <v-col cols="12">
                        <p class=" offer-btn-text-title">{{ betType.title }}</p>
                      </v-col>
                      <v-col cols="12">
                        <p class="text-red-darken-1 offer-btn-text-quota">{{ betType.quota }}</p>
                      </v-col>
                    </v-row>
                  </v-btn>
                </v-col>
              </v-row>
            </v-card-actions>
          </v-card>

        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script lang="ts" setup>
import { useOfferStore } from '@/stores/offers.store';
import { usePlayingTicketStore } from '@/stores/playing-ticket.store';
import { SelectedBetType } from '@/types/selected-bet.type';
import { BetType, Offer, OfferBetType } from '@/types/types.betsy';

const offerStore = useOfferStore();
const playingTicketStore = usePlayingTicketStore();

onMounted(() => {
  offerStore.refreshOffers();
});

const toUIString = (date: Date) => {
  return new Date(date).toLocaleString();
};

function selectBet(bet: OfferBetType, offer: Offer) {
  const selectedBet: SelectedBetType = {
    SelectedBetType: bet,
    Offer: offer,
  };
  playingTicketStore.addSelectedBet(selectedBet);
}

function isSelectedBet(betType: OfferBetType): boolean {
  return playingTicketStore.selectedBetTypesIds.some((bet) => bet === betType.id);
}

function setFilter(filter: string) {
  offerStore.setFilter(filter);
}


</script>

<style scoped>
.scrollable-main {
  height: calc(100vh - 90px);
  /* Adjust based on your app bar and footer height */
  overflow-y: auto;
}

.offer-btn-text-title {
  font-size: 0.7rem !important;
  font-weight: 800;
  line-height: 0;
  letter-spacing: 0.03125em !important;
  font-family: "Roboto", sans-serif;
  text-transform: none !important;
}

.offer-btn-text-quota {
  font-size: 0.7rem !important;
  font-weight: 800;
  line-height: 0;
  letter-spacing: 0.03125em !important;
  font-family: "Roboto", sans-serif;
  text-transform: none !important;
}

.selected-bet {
  background: #00000069;
}
</style>
