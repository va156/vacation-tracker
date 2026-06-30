/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAsyncThunk } from '@reduxjs/toolkit';
import { apiClient } from '../../shared/api/baseApi';

export const refreshTokenThunk = createAsyncThunk(
    'auth/refreshToken',
    async (_, { rejectWithValue }) => {
        try {
            const data = await apiClient<any>('/auth/refresh-token', {
                method: 'POST',
            });

            localStorage.setItem('accessToken', data.accessToken);
            return data;
        } catch (error) {
            localStorage.removeItem('accessToken');
            return rejectWithValue(error instanceof Error ? error.message : 'Unknown error');
        }
    }
);