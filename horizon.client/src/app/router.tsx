import { createBrowserRouter, Navigate } from "react-router-dom";
import LandingPage from "../pages/LandingPage";
import MainPage from "../pages/MainPage";
import RegistrationPage from "../pages/RegistrationPage";
import RootLayout from "./RootLayout";
import LoginPage from "../pages/LoginPage";
import { ProtectedRoute } from "../shared/components/ProtectedRoute";
import { authSelectors } from "../modules/auth/authSlice";
import { useAppSelector } from "../shared/redux";

// Компонент для редиректа авторизованных пользователей
const PublicRoute = ({ children }: { children: React.ReactNode }) => {
    const isAuthenticated = useAppSelector(authSelectors.selectIsAuthenticated);

    if (isAuthenticated) {
        return <Navigate to="/dashboard" replace />;
    }

    return <>{children}</>;
};

export const router = createBrowserRouter([
    {
        path: "/",
        element: <RootLayout />,
        children: [
            {
                path: "/",
                element: (
                    <PublicRoute>
                        <LandingPage />
                    </PublicRoute>
                ),
            },
            {
                path: "/login",
                element: (
                    <PublicRoute>
                        <LoginPage />
                    </PublicRoute>
                ),
            },
            {
                path: "/registration",
                element: (
                    <PublicRoute>
                        <RegistrationPage />
                    </PublicRoute>
                ),
            },
            {
                path: "/dashboard",
                element: (
                    <ProtectedRoute>
                        <MainPage />
                    </ProtectedRoute>
                ),
            },
            // Редирект с /main на /dashboard
            {
                path: "/main",
                element: <Navigate to="/dashboard" replace />,
            },
            // Если пользователь заходит на любой другой URL
            {
                path: "*",
                element: <Navigate to="/" replace />,
            },
        ],
    },
]);