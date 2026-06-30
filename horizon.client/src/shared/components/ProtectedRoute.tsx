import { Navigate } from "react-router-dom";
import { authSelectors } from "../../modules/auth/authSlice";
import { useAppSelector } from "../redux";
import { Box, CircularProgress } from "@mui/material";

interface Props {
    children: React.ReactNode;
    requiredRoles?: string[];
}

export function ProtectedRoute({ children, requiredRoles = [] }: Props) {
    const isLoading = useAppSelector(authSelectors.selectIsLoading);
    const isAuthenticated = useAppSelector(authSelectors.selectIsAuthenticated);
    const userRoles = useAppSelector(authSelectors.selectCurrentUserRoles);

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (!isAuthenticated) {
        return <Navigate to="/" replace />;
    }

    const hasRequiredRoles = requiredRoles.length === 0 ||
        requiredRoles.some(role => userRoles.includes(role));

    if (!hasRequiredRoles) {
        return <Navigate to="/dashboard" replace />;
    }

    return <>{children}</>;
}