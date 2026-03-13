import axios from "axios";

//ref: https://www.npmjs.com/package/axios/v/0.27.2
//ref: https://axios-http.com/docs/instance

export const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    headers: {
        "Content-Type": "application/json"
    }
});