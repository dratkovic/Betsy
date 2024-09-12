import { LoginRequest, RegisterRequest, User } from '@/types/models/types.betsy';
import api from './api.service';
import tokenService from './token.service';

class AuthService {
    login(loginRequest: LoginRequest): Promise<User> {
        return api
            .post<LoginRequest, User>("/login", loginRequest)
            .then(response => {
                tokenService.setUser(response)
                return response
            })
    }

    logout() {
        tokenService.removeUser()
    }

    register(registerRequest: RegisterRequest): Promise<User> {
        return api.post<RegisterRequest, User>("/register", registerRequest)
            .then(response => {
                tokenService.setUser(response)
                return response
            })
    }
}

export default new AuthService()