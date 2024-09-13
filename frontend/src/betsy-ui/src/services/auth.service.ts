import { LoginRequest, RegisterRequest, User } from '@/types/types.betsy';
import api from './api.service';
import tokenService from './token.service';
import { AxiosResponse } from 'axios';

class AuthService {
    login(loginRequest: LoginRequest): Promise<User> {
        return api
            .post<LoginRequest, AxiosResponse<User>>("/login", loginRequest)
            .then(response => {
                tokenService.setUser(response.data)
                return response.data
            })
    }

    logout() {
        tokenService.removeUser()
    }

    register(registerRequest: RegisterRequest): Promise<User> {
        return api.post<RegisterRequest, AxiosResponse<User>>("/register", registerRequest)
            .then(response => {
                tokenService.setUser(response.data)
                return response.data
            })
    }
}

export default new AuthService()