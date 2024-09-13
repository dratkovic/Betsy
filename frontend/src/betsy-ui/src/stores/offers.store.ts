import offerService from '@/services/offer.service'
import { Offer } from '@/types/types.betsy'
import { defineStore } from 'pinia'


export const useOfferStore = defineStore('offer', {
    state: () => ({
        offers: [] as Offer[],
        totalRecords: 0,
        currentPage: 1,
        pageSize: 20,
    }),
    actions: {
        async refreshOffers() {
            offerService.getOffers({ "page": this.currentPage, "pageSize": this.pageSize }).then(response => {
                this.offers = response.data
                this.totalRecords = response.totalRecords
            })
        }
    },
    getters: {

    }
})