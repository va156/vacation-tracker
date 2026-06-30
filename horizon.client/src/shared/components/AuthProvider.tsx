import { createContext, useEffect, useRef, useState } from "react";
import { authSelectors } from "../../modules/auth/authSlice";
import { getCurrentUserThunk } from "../../modules/auth/getCurrentUserThunk";
import { useAppDispatch, useAppSelector } from "../redux";
import { Box, CircularProgress } from "@mui/material";

export const AuthContext = createContext<{ accessToken: string | null; isInitialized: boolean }>({
    accessToken: null,
    isInitialized: false,
});

type Props = { children: React.ReactNode };

export const AuthProvider = ({ children }: Props) => {
    const [isInitialized, setIsInitialized] = useState(false);
    const token = useAppSelector(authSelectors.selectAccessToken);
    const user = useAppSelector(authSelectors.selectUser);
    const dispatch = useAppDispatch();
    const prevTokenRef = useRef<string | null>(undefined);

    useEffect(() => {
        if (prevTokenRef.current === token) return;
        prevTokenRef.current = token;

        const initAuth = async () => {
            if (token && !user) {
                try {
                    await dispatch(getCurrentUserThunk()).unwrap();
                } catch {
                    localStorage.removeItem('accessToken');
                }
            }
            setIsInitialized(true);
        };

        initAuth();
    }, [token, dispatch, user]);

    if (!isInitialized) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    return (
        <AuthContext.Provider value={{ accessToken: token, isInitialized }}>
            {children}
        </AuthContext.Provider>
    );
};