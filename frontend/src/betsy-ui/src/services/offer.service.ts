import { Offer, PaginationQuery, PaginationResult } from '@/types/types.betsy';
import api from './api.service';

class OfferService {
    async getOffers(query: PaginationQuery, filter: string): Promise<PaginationResult<Offer>> {
        const url = filter ? `/offers/${filter}` : "/offers"
        return api
            .get<PaginationResult<Offer>>(url, { params: query })
            .then(response => {
                return response.data as PaginationResult<Offer>
            })
    }
}

export default new OfferService()