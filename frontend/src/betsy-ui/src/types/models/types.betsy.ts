export interface User {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    token: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
}

export interface PaginationResult<T> {
    page: number;
    pageSize: number;
    totalPages: number;
    totalRecords: number;
    data: T[];
}

export interface OfferBetType {
    id: string;
    title: string;
    quota: number;
}

export interface Offer {
    offerId: string;
    matchId: string;
    isSpecialOffer: boolean;
    nameOne: string;
    sport: string;
    nameTwo?: string;
    description: string;
    startsAtUtc: Date;
    bettingTypes: OfferBetType[];
}

export interface Match {
    nameOne: string;
    nameTwo?: string;
    description: string;
    startsAtUtc: Date;
    correlationId?: string;
}

export interface BetType {
    match: Match;
    title: string;
    quota: number;
}

export interface CreateTicketRequest {
    ticketAmount: number;
    selectedBetTypesIds: string[];
}

export interface TicketResponse {
    id: string;
    ticketAmount: number;
    stake: number;
    vig: number;
    potentialPayout: number;
    totalQuota: number;
    betTypes: BetType[];
}

export interface ValidationError {
    code: string,
    description: string
}

