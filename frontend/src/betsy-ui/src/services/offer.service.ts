import { Offer, PaginationQuery, PaginationResult } from '@/types/types.betsy';
import api from './api.service';

class OfferService {
    async getOffers(query: PaginationQuery): Promise<PaginationResult<Offer>> {
        return api
            .get<PaginationResult<Offer>>("/offers", { params: query })
            .then(response => {
                return response.data as PaginationResult<Offer>
            })
    }
}

export default new OfferService()