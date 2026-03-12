export const config = {
    apiUrl: import.meta.env.VITE_API_URL,
    env: import.meta.env.VITE_ENVIRONMENT,
    isProduction: import.meta.env.PROD,
    isDevelopment: import.meta.env.DEV,
    isTest: import.meta.env.TEST,
    mode : import.meta.env.MODE
}