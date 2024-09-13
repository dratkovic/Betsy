import { PaginationQuery, PaginationResult, TicketInfo } from "@/types/types.betsy"
import api from './api.service';

class TicketService {
    async getTickets(query: PaginationQuery): Promise<PaginationResult<TicketInfo>> {
        return api
            .get<PaginationResult<TicketInfo>>("/tickets", { params: query })
            .then(response => {
                return response.data as PaginationResult<TicketInfo>
            })
    }

    async createTicket(ticketAmount: number, selectedBetTypesIds: string[]): Promise<TicketInfo> {
        return api
            .post<TicketInfo>("/tickets", { ticketAmount, selectedBetTypesIds })
            .then(response => {
                return response.data as TicketInfo
            })
    }
}

export default new TicketService()