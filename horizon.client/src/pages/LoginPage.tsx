/* eslint-disable @typescript-eslint/no-unused-vars */
import { Alert, Stack } from "@mui/material";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import FormControl from "@mui/material/FormControl";
import FormLabel from "@mui/material/FormLabel";
import Link from "@mui/material/Link";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import { SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../shared/redux";
import { loginThunk } from "../modules/auth/login/loginThunk";
import { authSelectors } from "../modules/auth/authSlice";
import { useState } from "react";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { IconButton, InputAdornment, Paper } from "@mui/material";

type LoginFields = {
    email: string;
    password: string;
};

export default function LoginPage() {
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginFields>();

    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

    const isLoading = useAppSelector(authSelectors.selectIsLoading);
    const error = useAppSelector(authSelectors.selectError);

    const onSubmit: SubmitHandler<LoginFields> = async (data, event) => {
        event?.preventDefault();
        try {
            await dispatch(loginThunk(data)).unwrap();
            navigate('/dashboard');
        } catch (err) {
            // Ошибка уже в Redux
        }
    };

    return (
        <Box sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
        }}>
            <Paper
                elevation={0}
                sx={{
                    width: '100%',
                    maxWidth: 360,
                    p: 3,
                    borderRadius: 3,
                    bgcolor: 'background.paper',
                    border: '1px solid',
                    borderColor: 'divider',
                }}
            >
                <Typography
                    variant="h5"
                    align="center"
                    sx={{
                        fontWeight: 500,
                        mb: 3
                    }}
                >
                    Вход
                </Typography>

                <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate>
                    <Stack spacing={2.5}>
                        <FormControl fullWidth>
                            <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>
                                Email
                            </FormLabel>
                            <TextField
                                error={!!errors.email}
                                helperText={errors.email?.message}
                                size="small"
                                placeholder="your@email.com"
                                autoComplete="email"
                                autoFocus
                                required
                                fullWidth
                                disabled={isLoading}
                                {...register("email", {
                                    required: "Это поле обязательно",
                                    pattern: {
                                        value: /\S+@\S+\.\S+/,
                                        message: "Введите корректный email",
                                    },
                                })}
                            />
                        </FormControl>

                        <FormControl fullWidth>
                            <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>
                                Пароль
                            </FormLabel>
                            <TextField
                                size="small"
                                placeholder="••••••••"
                                type={showPassword ? "text" : "password"}
                                autoComplete="current-password"
                                required
                                fullWidth
                                error={!!errors.password}
                                helperText={errors.password?.message}
                                disabled={isLoading}
                                InputProps={{
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <IconButton
                                                onClick={() => setShowPassword(!showPassword)}
                                                edge="end"
                                                size="small"
                                            >
                                                {showPassword ? <VisibilityOff /> : <Visibility />}
                                            </IconButton>
                                        </InputAdornment>
                                    ),
                                }}
                                {...register("password", {
                                    required: "Это поле обязательно",
                                    minLength: {
                                        value: 6,
                                        message: "Минимум 6 символов",
                                    },
                                })}
                            />
                        </FormControl>

                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            disabled={isLoading}
                            sx={{
                                py: 1.2,
                                borderRadius: 2,
                                textTransform: 'none',
                                fontSize: '1rem',
                            }}
                        >
                            {isLoading ? "Вход..." : "Войти"}
                        </Button>

                        {error && (
                            <Alert severity="error" sx={{ borderRadius: 2 }}>
                                {error}
                            </Alert>
                        )}

                        <Typography align="center" variant="body2" color="text.secondary">
                            Нет аккаунта?{' '}
                            <Link
                                href="/registration"
                                sx={{
                                    color: 'primary.main',
                                    textDecoration: 'none',
                                    '&:hover': {
                                        textDecoration: 'underline',
                                    }
                                }}
                            >
                                Зарегистрироваться
                            </Link>
                        </Typography>
                    </Stack>
                </Box>
            </Paper>
        </Box>
    );
}