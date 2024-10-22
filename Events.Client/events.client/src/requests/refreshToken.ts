import axios from "axios";
import { serverUrl, User } from "../constants";


export function refreshToken(setUser: (user: User) => void){
    var user = {} as User;
    axios.put(`${serverUrl}/LogIn/RefreshToken`)
    .then(response => {
        if (response.status === 200){
            user = response.data as User;
            setUser(user);
        }
        if (response.status === 401){
            user = {isLogedIn: false, jwtToken: "", refreshToken: ""} as User;
            setUser(user);
        }
    });
    return user;
}