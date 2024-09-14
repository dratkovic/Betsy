import offerService from '@/services/offer.service'
import { Offer } from '@/types/types.betsy'
import { defineStore } from 'pinia'

// example using setup store (we got mre control to create offers as shallowRef)
export const useOfferStore = defineStore('offer', () => {

    const offers = shallowRef([] as Offer[])
    const totalRecords = ref(0)
    const currentPage = ref(1)
    const pageSize = ref(30)
    const filter = ref("")

    const refreshOffers = async () => {
        offerService.getOffers({ "page": currentPage.value, "pageSize": pageSize.value }, filter.value).then(response => {
            offers.value = response.data
            totalRecords.value = response.totalRecords
        })
    }

    const setFilter = (offersFilter: string) => {
        filter.value = offersFilter
        refreshOffers()
    }

    return {
        offers,
        totalRecords,
        currentPage,
        pageSize,
        filter,
        refreshOffers,
        setFilter
    }
})