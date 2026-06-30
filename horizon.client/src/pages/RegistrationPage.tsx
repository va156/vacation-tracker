/* eslint-disable @typescript-eslint/no-unused-vars */
import { useState, useEffect } from "react";
import {
    Alert, Stack, Box, Button,
    FormControl, FormLabel, Link, TextField, Typography,
    LinearProgress, FormHelperText, CircularProgress,
    Checkbox, FormControlLabel, InputAdornment, IconButton, Paper
} from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { SubmitHandler, useForm, Controller } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useRegisterMutation } from "../modules/auth/authApi";
import { FetchBaseQueryError } from "@reduxjs/toolkit/query";
import { SerializedError } from "@reduxjs/toolkit";

// Добавляем интерфейс для пропсов PhoneInput
interface PhoneInputProps {
    value?: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    error?: boolean;
    helperText?: string;
    [key: string]: unknown;
}

const PhoneInput = ({ value = '', onChange, error, helperText, ...props }: PhoneInputProps) => {
    const formatPhone = (input: string) => {
        const numbers = input.replace(/\D/g, '');
        const limited = numbers.slice(0, 11);

        if (limited.length <= 1) return `+7 (${limited}`;
        if (limited.length <= 4) return `+7 (${limited.slice(1, 4)}`;
        if (limited.length <= 7) return `+7 (${limited.slice(1, 4)}) ${limited.slice(4, 7)}`;
        if (limited.length <= 9) return `+7 (${limited.slice(1, 4)}) ${limited.slice(4, 7)}-${limited.slice(7, 9)}`;
        return `+7 (${limited.slice(1, 4)}) ${limited.slice(4, 7)}-${limited.slice(7, 9)}-${limited.slice(9, 11)}`;
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const formatted = formatPhone(e.target.value);
        const newEvent = {
            ...e,
            target: {
                ...e.target,
                value: formatted
            }
        };
        onChange(newEvent);
    };

    return (
        <TextField
            {...props}
            value={value}
            onChange={handleChange}
            placeholder="+7 (999) 999-99-99"
            fullWidth
            size="small"
            error={error}
            helperText={helperText}
            inputProps={{ maxLength: 18 }}
        />
    );
};

type RegisterFields = {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
    firstName: string;
    lastName: string;
    middleName?: string;
    phone?: string;
    agreeToTerms: boolean;
};

// Тип для ошибки от бэкенда
interface ProblemDetails {
    title?: string;
    detail?: string;
    status?: number;
    [key: string]: unknown;
}

function isFetchBaseQueryError(error: unknown): error is FetchBaseQueryError {
    return typeof error === 'object' && error != null && 'status' in error;
}

function isProblemDetails(data: unknown): data is ProblemDetails {
    return typeof data === 'object' && data != null;
}

export default function RegistrationPage() {
    const {
        register,
        handleSubmit,
        watch,
        control,
        formState: { errors },
    } = useForm<RegisterFields>();

    const [showPassword, setShowPassword] = useState(false);
    const [passwordStrength, setPasswordStrength] = useState(0);
    const [progress, setProgress] = useState(0);

    const navigate = useNavigate();
    const [registerUser, { isLoading, error }] = useRegisterMutation();

    const password = watch('password', '');
    const formValues = watch();

    // Проверка силы пароля
    useEffect(() => {
        let strength = 0;
        if (password.length >= 8) strength += 25;
        if (/[A-Z]/.test(password)) strength += 25;
        if (/[0-9]/.test(password)) strength += 25;
        if (/[^A-Za-z0-9]/.test(password)) strength += 25;
        setPasswordStrength(strength);
    }, [password]);

    // Прогресс заполнения
    useEffect(() => {
        const requiredFields: (keyof RegisterFields)[] = ['username', 'email', 'firstName', 'lastName', 'password'];
        const filled = requiredFields.filter(field =>
            formValues[field] && String(formValues[field]).length > 0
        ).length;
        setProgress((filled / requiredFields.length) * 100);
    }, [formValues]);

    const getPasswordStrengthColor = (): "error" | "warning" | "success" => {
        if (passwordStrength < 50) return "error";
        if (passwordStrength < 75) return "warning";
        return "success";
    };

    const getPasswordStrengthText = (): string => {
        if (passwordStrength < 50) return "Слабый";
        if (passwordStrength < 75) return "Средний";
        return "Сильный";
    };

    const getErrorMessage = (): string | null => {
        if (!error) return null;

        if (isFetchBaseQueryError(error)) {
            if (error.data && isProblemDetails(error.data)) {
                return error.data.detail || error.data.title || `Ошибка ${error.status}`;
            }
            return `Ошибка сервера: ${error.status}`;
        }

        const serializedError = error as SerializedError;
        if (serializedError.message) {
            return serializedError.message;
        }

        return 'Произошла ошибка при регистрации';
    };

    const onSubmit: SubmitHandler<RegisterFields> = async (data) => {
        try {
            const { confirmPassword, agreeToTerms, ...registerData } = data;
            await registerUser(registerData).unwrap();
            navigate('/dashboard');
        } catch (err) {
            console.debug('Registration error:', err);
        }
    };

    const errorMessage = getErrorMessage();

    return (
        <Box sx={{
            minHeight: '100vh',
            display: 'flex',
            flexDirection: 'column',
            py: 3
        }}>
            <Box sx={{
                flex: 1,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
            }}>
                <Paper
                    elevation={0}
                    sx={{
                        width: '100%',
                        maxWidth: 480,
                        p: 3.5,
                        borderRadius: 3,
                        bgcolor: 'background.paper',
                        border: '1px solid',
                        borderColor: 'divider',
                        mx: 2,
                    }}
                >
                    <Typography variant="h5" align="center" sx={{ fontWeight: 500, mb: 2 }}>
                        Регистрация
                    </Typography>

                    {/* Прогресс бар */}
                    <Box sx={{ mb: 2 }}>
                        <LinearProgress
                            variant="determinate"
                            value={progress}
                            sx={{ height: 4, borderRadius: 2 }}
                        />
                        <Typography variant="caption" sx={{ mt: 0.5, display: 'block', textAlign: 'center', color: 'text.secondary' }}>
                            Заполнено {Math.round(progress)}%
                        </Typography>
                    </Box>

                    <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate>
                        <Stack spacing={2}>
                            {/* Имя пользователя */}
                            <FormControl fullWidth>
                                <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>Имя пользователя *</FormLabel>
                                <TextField
                                    error={!!errors.username}
                                    helperText={errors.username?.message}
                                    placeholder="ivan123"
                                    size="small"
                                    {...register("username", {
                                        required: "Обязательное поле",
                                        minLength: { value: 3, message: "Минимум 3 символа" },
                                        pattern: {
                                            value: /^[a-zA-Z0-9_]+$/,
                                            message: "Только буквы, цифры и _"
                                        }
                                    })}
                                />
                            </FormControl>

                            {/* Email */}
                            <FormControl fullWidth>
                                <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>Email *</FormLabel>
                                <TextField
                                    error={!!errors.email}
                                    helperText={errors.email?.message}
                                    type="email"
                                    placeholder="your@email.com"
                                    size="small"
                                    {...register("email", {
                                        required: "Обязательное поле",
                                        pattern: {
                                            value: /\S+@\S+\.\S+/,
                                            message: "Неверный формат email"
                                        }
                                    })}
                                />
                            </FormControl>

                            {/* Имя */}
                            <FormControl fullWidth>
                                <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>Имя *</FormLabel>
                                <TextField
                                    error={!!errors.firstName}
                                    helperText={errors.firstName?.message}
                                    placeholder="Иван"
                                    size="small"
                                    {...register("firstName", { required: "Обязательное поле" })}
                                />
                            </FormControl>

                            {/* Фамилия */}
                            <FormControl fullWidth>
                                <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>Фамилия *</FormLabel>
                                <TextField
                                    error={!!errors.lastName}
                                    helperText={errors.lastName?.message}
                                    placeholder="Петров"
                                    size="small"
                                    {...register("lastName", { required: "Обязательное поле" })}
                                />
                            </FormControl>

                            {/* Отчество */}
                            <FormControl fullWidth>
                                <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>Отчество</FormLabel>
                                <TextField
                                    placeholder="Иванович"
                                    size="small"
                                    {...register("middleName")}
                                />
                            </FormControl>

                            {/* Телефон */}
                            <FormControl fullWidth>
                                <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>Телефон</FormLabel>
                                <Controller
                                    name="phone"
                                    control={control}
                                    defaultValue=""
                                    render={({ field }) => (
                                        <PhoneInput
                                            value={field.value}
                                            onChange={field.onChange}
                                            error={!!errors.phone}
                                            helperText={errors.phone?.message}
                                        />
                                    )}
                                />
                            </FormControl>

                            {/* Пароль */}
                            <FormControl fullWidth>
                                <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>Пароль *</FormLabel>
                                <TextField
                                    type={showPassword ? "text" : "password"}
                                    placeholder="••••••••"
                                    size="small"
                                    error={!!errors.password}
                                    helperText={errors.password?.message}
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
                                        required: "Обязательное поле",
                                        minLength: { value: 6, message: "Минимум 6 символов" },
                                        validate: {
                                            hasUpperCase: (v) => /[A-Z]/.test(v) || "Нужна заглавная буква",
                                            hasNumber: (v) => /[0-9]/.test(v) || "Нужна цифра",
                                        }
                                    })}
                                />

                                {/* Индикатор силы пароля */}
                                {password && (
                                    <Box sx={{ mt: 1 }}>
                                        <LinearProgress
                                            variant="determinate"
                                            value={passwordStrength}
                                            color={getPasswordStrengthColor()}
                                            sx={{ height: 4, borderRadius: 2 }}
                                        />
                                        <FormHelperText>
                                            Сила пароля: {getPasswordStrengthText()}
                                        </FormHelperText>
                                    </Box>
                                )}
                            </FormControl>

                            {/* Подтверждение пароля */}
                            <FormControl fullWidth>
                                <FormLabel sx={{ fontSize: '0.9rem', mb: 0.5 }}>Подтверждение пароля *</FormLabel>
                                <TextField
                                    type="password"
                                    placeholder="••••••••"
                                    size="small"
                                    error={!!errors.confirmPassword}
                                    helperText={errors.confirmPassword?.message}
                                    {...register("confirmPassword", {
                                        required: "Подтвердите пароль",
                                        validate: (value) =>
                                            value === password || "Пароли не совпадают"
                                    })}
                                />
                            </FormControl>

                            {/* Согласие с правилами */}
                            <FormControl error={!!errors.agreeToTerms}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            size="small"
                                            {...register("agreeToTerms", {
                                                required: "Необходимо согласие"
                                            })}
                                        />
                                    }
                                    label={
                                        <Typography variant="body2">
                                            Я согласен с условиями использования
                                        </Typography>
                                    }
                                />
                                {errors.agreeToTerms && (
                                    <FormHelperText error>
                                        {errors.agreeToTerms.message}
                                    </FormHelperText>
                                )}
                            </FormControl>

                            {/* Кнопка */}
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
                                    mt: 1
                                }}
                            >
                                {isLoading ? (
                                    <>
                                        <CircularProgress size={20} sx={{ mr: 1 }} />
                                        Регистрация...
                                    </>
                                ) : "Зарегистрироваться"}
                            </Button>

                            {/* Ошибка */}
                            {errorMessage && (
                                <Alert severity="error" sx={{ borderRadius: 2 }}>
                                    {errorMessage}
                                </Alert>
                            )}

                            <Typography align="center" variant="body2" color="text.secondary">
                                Уже есть аккаунт?{' '}
                                <Link
                                    href="/login"
                                    sx={{
                                        color: 'primary.main',
                                        textDecoration: 'none',
                                        '&:hover': {
                                            textDecoration: 'underline',
                                        }
                                    }}
                                >
                                    Войти
                                </Link>
                            </Typography>
                        </Stack>
                    </Box>
                </Paper>
            </Box>
        </Box>
    );
}