import axios from "axios";

const BASE_URL = window.location.origin;
const API_URL = `${BASE_URL}/api/auth/`;

const defaultConfig = {
    headers: {
        'Accept': '*/*',
        'Content-Type': 'application/json',
        'withCredentials': true
    }
}

type loginType = {
    username: string;
    password: string;
    returnUrl: string | null;
}


class AuthService {
    login(data: loginType) {
        return axios
            .post(API_URL + "login",
                data, defaultConfig)
            .then(response => {
                return response.data
            })
    }

    logout(logoutId: string | null) {
        return axios
            .get(API_URL + "logout?logoutId=" + logoutId,
                defaultConfig)
            .then(response => {
                return response.data
            })
    }
}

export default new AuthService();
