import { SelectedBetType } from '@/types/selected-bet.type'
import { defineStore } from 'pinia'


export const usePlayingTicketStore = defineStore('playingTicketStore', {
    state: () => ({
        selectedBets: [] as SelectedBetType[],
        ticketAmount: 1.00 as number,
    }),
    actions: {
        addSelectedBet(selectedBet: SelectedBetType) {
            const existingBet = this.selectedBets.find(bet => bet.SelectedBetType.id === selectedBet.SelectedBetType.id)
            if (existingBet)
                return

            const existingOffer = this.selectedBets.find(bet => bet.Offer.matchId === selectedBet.Offer.matchId)
            if (existingOffer)
                this.selectedBets = this.selectedBets.filter(bet => bet !== existingOffer)

            this.selectedBets.push(selectedBet)
        },
        removeSelectedBet(selectedBet: SelectedBetType) {
            this.selectedBets = this.selectedBets.filter(bet => bet !== selectedBet)
        },
        clearTicket() {
            this.selectedBets = []
            this.ticketAmount = 1.00
        }
    },
    getters: {
        serviceCost(): number {
            return Math.round(((this.ticketAmount * 0.05) + Number.EPSILON) * 100) / 100
        },
        betAmount(): number {
            return Math.round(((this.ticketAmount - this.serviceCost) + Number.EPSILON) * 100) / 100
        },
        totalOdds(): number {
            return this.selectedBets.reduce((acc, bet) => Math.round(((acc * bet.SelectedBetType.quota) + Number.EPSILON) * 100) / 100, 1)
        },
        totalPossibleWin(): number {
            return Math.round(((this.totalOdds * this.betAmount) + Number.EPSILON) * 100) / 100
        }

    }
})