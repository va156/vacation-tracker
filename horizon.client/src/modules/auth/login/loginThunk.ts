/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAsyncThunk } from '@reduxjs/toolkit';
import { apiClient } from '../../../shared/api/baseApi';
import { AuthResponse, LoginRequest } from '../authApi';

export const loginThunk = createAsyncThunk(
    'auth/login',
    async (data: LoginRequest, { rejectWithValue }) => {
        try {
            const response = await apiClient<AuthResponse>('/auth/login', {
                method: 'POST',
                body: JSON.stringify(data),
            });

            localStorage.setItem('accessToken', response.accessToken);

            return response;
        } catch (error: any) {
            localStorage.removeItem('accessToken');

            // Возвращаем понятное сообщение
            return rejectWithValue(error.message || 'Неверный email или пароль');
        }
    }
);