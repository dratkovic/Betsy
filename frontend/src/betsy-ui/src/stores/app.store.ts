import authService from '@/services/auth.service'
import tokenService from '@/services/token.service'
import { LoginRequest, RegisterRequest, User } from '@/types/types.betsy'
import { defineStore } from 'pinia'

const initialUser = tokenService.getUser()

export const useAppStore = defineStore('app', {
    state: () => ({
        user: initialUser as User | null,
        errorSnackbar: false as boolean,
        errorMessage: '' as string,
        ongoingServerRequests: 0 as number,
    }),
    actions: {
        login(login: LoginRequest): Promise<User> {
            return authService.login(login).then(response => {
                this.user = response as User

                return response
            })
        },
        register(register: RegisterRequest): Promise<User> {
            return authService.register(register).then(response => {
                this.user = response as User

                return response
            })
        },
        logout() {
            authService.logout()
            this.user = null
        },
        showError(message: string) {
            this.errorSnackbar = true
            this.errorMessage = message
        },
        startServerRequest() {
            this.ongoingServerRequests++
        },
        endServerRequest() {
            this.ongoingServerRequests--
        }
    },
    getters: {
        isLoggedIn(): boolean {
            return !!this.user
        },
        isLoadingFromServer(): boolean {
            return this.ongoingServerRequests > 0
        }
    },
})