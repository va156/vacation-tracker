import { createAsyncThunk } from '@reduxjs/toolkit';
import { apiClient } from '../../../shared/api/baseApi';
import { authActions } from '../authSlice';


export const logoutThunk = createAsyncThunk(
    'auth/logout',
    async (_, { dispatch, rejectWithValue }) => {
        try {
            await apiClient('/auth/logout', { method: 'POST' });
            dispatch(authActions.logOut());
            localStorage.removeItem('accessToken');
            return true;
        } catch (error) {
            dispatch(authActions.logOut());
            localStorage.removeItem('accessToken');
            return rejectWithValue(error);
        }
    }
);