import offerService from '@/services/offer.service'
import { Offer } from '@/types/types.betsy'
import { defineStore } from 'pinia'


export const useOfferStore = defineStore('offer', {
    state: () => ({
        offers: [] as Offer[],
        totalRecords: 0,
        currentPage: 1,
        pageSize: 30,
        filter: ""
    }),
    actions: {
        async refreshOffers() {
            offerService.getOffers({ "page": this.currentPage, "pageSize": this.pageSize }, this.filter).then(response => {
                this.offers = response.data
                this.totalRecords = response.totalRecords
            })
        },
        setFilter(filter: string) {
            this.filter = filter
            this.refreshOffers()
        }
    },
    getters: {

    }
})