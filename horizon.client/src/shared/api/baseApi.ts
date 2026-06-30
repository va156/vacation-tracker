import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { Mutex } from "async-mutex";
import { AuthResponse } from "../../modules/auth/authApi";
import { authActions } from "../../modules/auth/authSlice";
import { AppState } from "../redux";

export const BASE_URL = "https://localhost:7158/api";

// Тип для кастомных опций запроса
interface RequestOptions extends RequestInit {
    retry?: boolean;
}

// Функция для обычных fetch запросов
export async function apiClient<T>(
    endpoint: string,
    options: RequestOptions = {}
): Promise<T> {
    const url = `${BASE_URL}${endpoint}`;
    const token = localStorage.getItem('accessToken');

    const headers = new Headers({
        'Content-Type': 'application/json',
        ...(token && { 'Authorization': `Bearer ${token}` }),
        ...(options.headers as Record<string, string> || {}),
    });

    const fetchOptions: RequestInit = {
        ...options,
        headers,
        credentials: 'include',
    };

    const response = await fetch(url, fetchOptions);

    // Если 401 и это не повторная попытка - пробуем обновить токен
    if (response.status === 401 && !options.retry) {
        try {
            const refreshResponse = await fetch(`${BASE_URL}/auth/refresh-token`, {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (refreshResponse.ok) {
                const data = await refreshResponse.json();
                localStorage.setItem('accessToken', data.accessToken);

                // Повторяем исходный запрос
                const retryOptions: RequestOptions = {
                    ...options,
                    retry: true,
                    headers: {
                        ...(options.headers as Record<string, string> || {}),
                        'Authorization': `Bearer ${data.accessToken}`,
                    },
                };

                return apiClient<T>(endpoint, retryOptions);
            }
        } catch (refreshError) {
            console.error('Refresh failed:', refreshError);
        }

        localStorage.removeItem('accessToken');
    }

    // Читаем ответ ОДИН раз
    const responseText = await response.text();

    // Пробуем распарсить JSON
    let responseData;
    try {
        responseData = responseText ? JSON.parse(responseText) : {};
    } catch {
        responseData = { message: responseText };
    }

    // Если ответ не успешный - выбрасываем ошибку
    if (!response.ok) {
        // Для 401 показываем понятное сообщение
        if (response.status === 401) {
            throw new Error('Неверный email или пароль');
        }

        throw new Error(
            responseData.detail ||
            responseData.title ||
            responseData.message ||
            `Ошибка ${response.status}`
        );
    }

    return responseData as T;
}

// RTK Query baseQuery
export const baseQuery = fetchBaseQuery({
    baseUrl: BASE_URL,
    credentials: "include",
    prepareHeaders: (headers, { getState }) => {
        const state = getState() as AppState;
        const accessToken = state.auth.accessToken || localStorage.getItem('accessToken');

        if (accessToken) {
            headers.set("Authorization", `Bearer ${accessToken}`);
        }

        return headers;
    },
});

const mutex = new Mutex();

const baseQueryWithRefresh: typeof baseQuery = async (
    args,
    api,
    extraOptions
) => {
    await mutex.waitForUnlock();
    let result = await baseQuery(args, api, extraOptions);

    if (result.error && result.error.status === 401) {
        if (!mutex.isLocked()) {
            const release = await mutex.acquire();

            try {
                const refreshResult = await baseQuery(
                    {
                        url: "/auth/refresh-token",
                        method: "POST",
                    },
                    api,
                    extraOptions
                );

                if (refreshResult.data) {
                    const data = refreshResult.data as AuthResponse;

                    api.dispatch(
                        authActions.tokenReceived({
                            accessToken: data.accessToken,
                            user: data.user,
                        })
                    );

                    localStorage.setItem('accessToken', data.accessToken);
                    result = await baseQuery(args, api, extraOptions);
                } else {
                    api.dispatch(authActions.logOut());
                    localStorage.removeItem('accessToken');
                }
            } finally {
                release();
            }
        } else {
            await mutex.waitForUnlock();
            result = await baseQuery(args, api, extraOptions);
        }
    }

    return result;
};

export const baseApi = createApi({
    baseQuery: baseQueryWithRefresh,
    endpoints: () => ({}),
});

export default baseApi;