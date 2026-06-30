import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { User } from '../users/user';
import { loginThunk } from './login/loginThunk';
import { getCurrentUserThunk } from './getCurrentUserThunk';

interface AuthState {
    user: User | null;
    accessToken: string | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    error: string | null;
}

// Функция для получения токена из localStorage
const getInitialToken = (): string | null => {
    try {
        return localStorage.getItem('accessToken');
    } catch (error) {
        console.error('Failed to read token from localStorage', error);
        return null;
    }
};

const initialToken = getInitialToken();

const initialState: AuthState = {
    user: null,
    accessToken: initialToken,
    isAuthenticated: !!initialToken,
    isLoading: false,
    error: null,
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        tokenReceived: (state, action: PayloadAction<{ accessToken: string; user: User }>) => {
            state.accessToken = action.payload.accessToken;
            state.user = action.payload.user;
            state.isAuthenticated = true;
            state.error = null;
        },
        setUser: (state, action: PayloadAction<User>) => {
            state.user = action.payload;
        },
        logOut: (state) => {
            state.user = null;
            state.accessToken = null;
            state.isAuthenticated = false;
        },
        setError: (state, action: PayloadAction<string>) => {
            state.error = action.payload;
        },
        setLoading: (state, action: PayloadAction<boolean>) => {
            state.isLoading = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(loginThunk.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(loginThunk.fulfilled, (state, action) => {
                state.isLoading = false;
                state.user = action.payload.user;
                state.accessToken = action.payload.accessToken;
                state.isAuthenticated = true;
                state.error = null;
            })
            .addCase(loginThunk.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload as string || 'Неверный email или пароль';
                state.isAuthenticated = false;
                state.accessToken = null;
                state.user = null;
            })
            .addCase(getCurrentUserThunk.fulfilled, (state, action) => {
                state.user = action.payload;
                state.isAuthenticated = true;
                state.isLoading = false;
            })
            .addCase(getCurrentUserThunk.rejected, (state) => {
                state.user = null;
                state.isAuthenticated = false;
                state.accessToken = null;
                state.isLoading = false;
            });
    },
});

export const authActions = authSlice.actions;

export const authSelectors = {
    selectUser: (state: { auth: AuthState }) => state.auth.user,
    selectAccessToken: (state: { auth: AuthState }) => state.auth.accessToken,
    selectIsAuthenticated: (state: { auth: AuthState }) => state.auth.isAuthenticated,
    selectIsLoading: (state: { auth: AuthState }) => state.auth.isLoading,
    selectError: (state: { auth: AuthState }) => state.auth.error,
    selectCurrentUserRoles: (state: { auth: AuthState }) => {
        const user = state.auth.user;
        return user?.roleName ? [user.roleName] : [];
    },
};

export default authSlice.reducer;