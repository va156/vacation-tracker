import { createContext, useEffect, useState } from "react";
import { authSelectors } from "../../modules/auth/authSlice";
import { getCurrentUserThunk } from "../../modules/auth/getCurrentUserThunk";
import { useAppDispatch, useAppSelector } from "../redux";
import { Box, CircularProgress } from "@mui/material";

export const AuthContext = createContext<{ accessToken: string | null }>({
    accessToken: null,
});

type Props = { children: React.ReactNode };

export const AuthProvider = ({ children }: Props) => {
    const [isInitialized, setIsInitialized] = useState(false);
    const token = useAppSelector(authSelectors.selectAccessToken);
    const user = useAppSelector(authSelectors.selectUser);
    const dispatch = useAppDispatch();

    useEffect(() => {
        const initAuth = async () => {
            console.log('🔵 AuthProvider init');
            console.log('   - Token from store:', token);
            console.log('   - Token from localStorage:', localStorage.getItem('accessToken'));

            const storedToken = localStorage.getItem('accessToken');

            // Только загружаем пользователя, если есть токен
            if (storedToken && !user) {
                console.log('🟢 Loading user data...');
                try {
                    await dispatch(getCurrentUserThunk()).unwrap();
                    console.log('✅ User loaded');
                } catch (error) {
                    console.error('❌ Failed to load user:', error);
                    localStorage.removeItem('accessToken');
                }
            }

            setIsInitialized(true);
        };

        initAuth();
    }, [dispatch]); 

    if (!isInitialized) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    return (
        <AuthContext.Provider value={{ accessToken: token }}>
            {children}
        </AuthContext.Provider>
    );
};