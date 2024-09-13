import axios, { AxiosError, AxiosInstance, AxiosResponse, InternalAxiosRequestConfig } from 'axios';
import authService from './auth.service';
import { ValidationError } from '@/types/types.betsy';
import { useAppStore } from '@/stores/app.store';

const instance = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
    headers: {
        "Content-Type": "application/json",
        'Accept': 'application/json'
    }
});


const onRequest = (config: InternalAxiosRequestConfig<any>): InternalAxiosRequestConfig<any> => {
    const appStore = useAppStore();
    appStore.startServerRequest();
    console.info(`[request] [${JSON.stringify(config)}]`);
    return config;
}

const onRequestError = (error: AxiosError): Promise<AxiosError> => {
    const appStore = useAppStore();
    appStore.endServerRequest();
    console.error(`[request error] [${JSON.stringify(error)}]`);
    return Promise.reject(error);
}

const onResponse = (response: AxiosResponse): AxiosResponse => {
    const appStore = useAppStore();
    appStore.endServerRequest();
    console.info(`[response] [${JSON.stringify(response)}]`);
    return response;
}

const onResponseError = (error: AxiosError): Promise<AxiosError | ValidationError> => {
    const appStore = useAppStore();
    appStore.endServerRequest();
    const statusCode = error.response?.status

    // logging only errors that are not 401
    if (statusCode && statusCode !== 401) {
        if (error.response?.data && Array.isArray(error.response.data)) {
            const validationError = error.response?.data[0] as ValidationError;
            if (validationError) {
                console.error(`[response error] [${validationError.code}] [${validationError.description}]`);
                return Promise.reject(validationError);
            }
        }
        console.error(`[response error] [${JSON.stringify(error)}]`);
        return Promise.reject(error);
    }

    authService.logout()

    return Promise.reject(error)
}

export function setupInterceptorsTo(axiosInstance: AxiosInstance): AxiosInstance {
    axiosInstance.interceptors.request.use(onRequest, onRequestError);
    axiosInstance.interceptors.response.use(onResponse, onResponseError);
    return axiosInstance;
}

export default setupInterceptorsTo(instance);


