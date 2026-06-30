import { baseApi } from "../../shared/api/baseApi";
import { User } from "../users/user";

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    username: string;
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    middleName?: string;
    phone?: string;
}

export interface AuthResponse {
    accessToken: string;
    refreshToken: string; 
    expiresAt: string;
    user: User;
}

export const authApi = baseApi.injectEndpoints({
    endpoints: (builder) => ({
        register: builder.mutation<AuthResponse, RegisterRequest>({
            query: (data) => ({
                url: '/auth/register',
                method: 'POST',
                body: data,
            }),

            transformResponse: (response: AuthResponse) => {
                localStorage.setItem('accessToken', response.accessToken);
                return response;
            },
        }),

        login: builder.mutation<AuthResponse, LoginRequest>({
            query: (data) => ({
                url: '/auth/login',
                method: 'POST',
                body: data,
            }),
            transformResponse: (response: AuthResponse) => {
                localStorage.setItem('accessToken', response.accessToken);
                return response;
            },
        }),

        getCurrentUser: builder.query<User, void>({
            query: () => '/auth/me',
        }),

        logout: builder.mutation<void, void>({
            query: () => ({
                url: '/auth/logout',
                method: 'POST',
            }),
        }),

        refreshToken: builder.mutation<AuthResponse, void>({
            query: () => ({
                url: '/auth/refresh-token',
                method: 'POST',
            }),
            transformResponse: (response: AuthResponse) => {
                localStorage.setItem('accessToken', response.accessToken);
                return response;
            },
        }),
    }),
});

export const {
    useRegisterMutation,
    useLoginMutation,
    useGetCurrentUserQuery,
    useLogoutMutation,
    useRefreshTokenMutation,
} = authApi;