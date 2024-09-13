import { User } from "@/types/types.betsy"

class TokenService {
  getLocalAccessToken(): string | null {
    const user = this.getUser()

    return user?.token || null
  }

  updateLocalAccessToken(token: string) {
    const user = this.getUser()
    if (!user)
      return

    user.token = token
    localStorage.setItem("user", JSON.stringify(user))
  }

  getUser(): User | null {
    const localStorageUser = localStorage.getItem("user")
    if (!localStorageUser) {
      return null
    }
    const user: User = JSON.parse(localStorageUser)
    return user
  }

  setUser(user: User) {
    localStorage.setItem("user", JSON.stringify(user))
  }

  removeUser() {
    localStorage.removeItem("user")
  }
}

export default new TokenService()
