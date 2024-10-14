export const serverUrl = "http://localhost:5078";

export type User = {
    jwtToken: string,
    refreshToken: string,
    isSignin: boolean
}

export type RegisterDTO = {
    userName: string,
    email: string,
    password: string
}

export enum Pages {
    home,
    event,
    account,
    signup,
    register,
}