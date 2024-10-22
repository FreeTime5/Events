export const serverUrl = "http://localhost:5078";

export type User = {
    jwtToken: string,
    refreshToken: string,
    isLogedIn: boolean
}

export type RegisterDTO = {
    userName: string,
    email: string,
    password: string
}

export type LoginDto = {
    userName: string,
    password: string
}

export type GetEventResponseDto = {
    id: string,
    title: string,
    description: string,
    date: Date,
    place: string,
    categoryName: string,
    maxMembers: number,
    registrationCount: number,
    eventImageUrl: string,
    creatorName: string
}

export enum Pages {
    home,
    event,
    account,
    login,
    register,
}