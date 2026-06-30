import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import LogoutIcon from "@mui/icons-material/Logout";
import {
    AppBar,
    Box,
    Container,
    Toolbar,
    Typography,
    IconButton,
    Avatar,
    Menu,
    MenuItem,
    Divider,
    ListItemIcon,
    alpha,
    useScrollTrigger,
    Slide,
    Button,
} from "@mui/material";
import { authSelectors } from "../../../modules/auth/authSlice";
import { useLogoutMutation } from "../../../modules/auth/authApi";
import { useAppSelector, useAppDispatch } from "../../redux";
import { authActions } from "../../../modules/auth/authSlice";
import MainNavigationTabs from "./MainNavigationTabs";

interface Props {
    children?: React.ReactElement<unknown>;
}

function HideOnScroll(props: Props) {
    const { children } = props;
    const trigger = useScrollTrigger();
    return (
        <Slide appear={false} direction="down" in={!trigger}>
            {children ?? <div />}
        </Slide>
    );
}

export default function Header() {
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

    const isAuthenticated = useAppSelector(authSelectors.selectIsAuthenticated);
    const user = useAppSelector(authSelectors.selectUser);
    const [logout] = useLogoutMutation();

    const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleLogout = async () => {
        handleClose();
        try {
            await logout().unwrap();
            dispatch(authActions.logOut());
            localStorage.removeItem('accessToken');
            navigate('/');
        } catch (error) {
            console.error('Logout failed:', error);
            dispatch(authActions.logOut());
            localStorage.removeItem('accessToken');
            navigate('/');
        }
    };

    const handleProfile = () => {
        handleClose();
        navigate('/profile');
    };

    const handleLogoClick = () => {
        if (isAuthenticated) {
            navigate('/dashboard');
        } else {
            navigate('/');
        }
    };

    return (
        <Box sx={{ flexGrow: 1 }}>
            <HideOnScroll>
                <AppBar
                    position="fixed"
                    elevation={0}
                    sx={{
                        bgcolor: 'background.paper',
                        borderBottom: '1px solid',
                        borderColor: alpha('#000', 0.08),
                    }}
                >
                    <Container maxWidth="xl">
                        <Toolbar disableGutters sx={{ minHeight: 64 }}>
                            {/* Логотип */}
                            <Typography
                                variant="h6"
                                component="div"
                                onClick={handleLogoClick}
                                sx={{
                                    flexGrow: 1,
                                    fontWeight: 500,
                                    color: 'text.primary',
                                    cursor: 'pointer',
                                    letterSpacing: '-0.01em'
                                }}
                            >
                                Horizon
                            </Typography>

                            {isAuthenticated && (
                                <Box sx={{ display: { xs: 'none', md: 'block' }, mr: 2 }}>
                                    <MainNavigationTabs />
                                </Box>
                            )}

                            {isAuthenticated ? (
                                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                    <Typography
                                        variant="body2"
                                        sx={{
                                            display: { xs: 'none', sm: 'block' },
                                            color: 'text.secondary',
                                            mr: 1
                                        }}
                                    >
                                        {user?.firstName} {user?.lastName}
                                    </Typography>
                                    <IconButton
                                        onClick={handleMenu}
                                        size="small"
                                        sx={{
                                            p: 0.5,
                                            border: '1px solid',
                                            borderColor: alpha('#000', 0.08),
                                            borderRadius: 2,
                                        }}
                                    >
                                        <Avatar sx={{ width: 32, height: 32, bgcolor: 'primary.main' }}>
                                            {user?.firstName?.[0]}{user?.lastName?.[0]}
                                        </Avatar>
                                    </IconButton>
                                    <Menu
                                        anchorEl={anchorEl}
                                        open={open}
                                        onClose={handleClose}
                                        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                                        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
                                        PaperProps={{
                                            elevation: 0,
                                            sx: {
                                                mt: 1,
                                                minWidth: 180,
                                                border: '1px solid',
                                                borderColor: alpha('#000', 0.08),
                                                borderRadius: 2,
                                                boxShadow: '0 4px 12px rgba(0,0,0,0.05)',
                                            }
                                        }}
                                    >
                                        <MenuItem onClick={handleProfile}>
                                            <ListItemIcon>
                                                <AccountCircleIcon fontSize="small" />
                                            </ListItemIcon>
                                            Профиль
                                        </MenuItem>
                                        <Divider />
                                        <MenuItem onClick={handleLogout}>
                                            <ListItemIcon>
                                                <LogoutIcon fontSize="small" />
                                            </ListItemIcon>
                                            Выйти
                                        </MenuItem>
                                    </Menu>
                                </Box>
                            ) : (
                                <Box sx={{ display: 'flex', gap: 1 }}>
                                    <Button
                                        onClick={() => navigate('/login')}
                                        sx={{
                                            color: 'text.primary',
                                            textTransform: 'none',
                                            fontWeight: 500,
                                            borderRadius: 2,
                                            px: 2,
                                        }}
                                    >
                                        Войти
                                    </Button>
                                    <Button
                                        variant="contained"
                                        onClick={() => navigate('/registration')}
                                        sx={{
                                            textTransform: 'none',
                                            fontWeight: 500,
                                            borderRadius: 2,
                                            px: 2,
                                        }}
                                    >
                                        Регистрация
                                    </Button>
                                </Box>
                            )}
                        </Toolbar>
                    </Container>
                </AppBar>
            </HideOnScroll>
            <Toolbar sx={{ minHeight: 64 }} />
        </Box>
    );
}