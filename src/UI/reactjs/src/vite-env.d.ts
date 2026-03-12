interface ImportMetaEnv {
    VITE_API_BASE_URL: string;
    VITE_ENVIRONMENT: string;
}

interface ImportMeta {
    env: ImportMetaEnv;
}