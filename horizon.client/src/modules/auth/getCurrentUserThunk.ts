import { createAsyncThunk } from '@reduxjs/toolkit';
import { apiClient } from '../../shared/api/baseApi';
import { User } from '../users/user';

export const getCurrentUserThunk = createAsyncThunk(
    'auth/getCurrentUser',
    async (_, { rejectWithValue }) => {
        try {
            const user = await apiClient<User>('/auth/me');
            return user;
        } catch (error) {
            localStorage.removeItem('accessToken');
            return rejectWithValue(error instanceof Error ? error.message : 'Failed to get user');
        }
    }
);