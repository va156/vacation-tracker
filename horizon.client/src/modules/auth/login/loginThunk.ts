/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAsyncThunk } from '@reduxjs/toolkit';
import { apiClient } from '../../../shared/api/baseApi';
import { AuthResponse, LoginRequest } from '../authApi';

export const loginThunk = createAsyncThunk(
    'auth/login',
    async (data: LoginRequest, { rejectWithValue }) => {
        try {
            console.log('🟡 Login attempt:', data.email);

            const response = await apiClient<AuthResponse>('/auth/login', {
                method: 'POST',
                body: JSON.stringify(data),
            });

            console.log('🟢 Login response:', response);

            // Сохраняем токен в localStorage
            localStorage.setItem('accessToken', response.accessToken);
            console.log('💾 Token saved to localStorage');

            return response;
        } catch (error: any) {
            console.error('🔴 Login error:', error);
            localStorage.removeItem('accessToken');

            // Возвращаем понятное сообщение
            return rejectWithValue(error.message || 'Неверный email или пароль');
        }
    }
);