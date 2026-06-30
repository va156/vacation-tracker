import { useState, useEffect } from "react";
import KeyboardArrowUpIcon from "@mui/icons-material/KeyboardArrowUp";
import {
    Box, Fab, Table, TableContainer, TableHead, TableRow,
    TableCell, TableBody, Paper, Typography, Chip,
    TablePagination, Button, Stack, CircularProgress,
    FormControl, InputLabel, Select, MenuItem, alpha
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import FilterListIcon from "@mui/icons-material/FilterList";
import ScrollTop from "../shared/components/ScrollTop";
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../shared/redux";
import { authSelectors } from "../modules/auth/authSlice";
import type { FunctionComponent } from 'react';

// Тип для отпуска
interface LeaveRequest {
    id: number;
    startDate: string;
    endDate: string;
    status: string;
    days: number;
}

// Мок-данные
const mockLeaves: LeaveRequest[] = [
    {
        id: 1,
        startDate: "2025-06-01",
        endDate: "2025-06-15",
        status: "PENDING",
        days: 15
    },
    {
        id: 2,
        startDate: "2025-07-10",
        endDate: "2025-07-20",
        status: "APPROVED",
        days: 11
    },
    {
        id: 3,
        startDate: "2025-08-05",
        endDate: "2025-08-12",
        status: "REJECTED",
        days: 8
    },
    {
        id: 4,
        startDate: "2025-09-01",
        endDate: "2025-09-10",
        status: "PENDING",
        days: 10
    },
    {
        id: 5,
        startDate: "2025-10-15",
        endDate: "2025-10-20",
        status: "SENT_BACK",
        days: 6
    }
];

const getStatusColor = (status: string): "warning" | "success" | "error" | "info" | "default" => {
    switch (status) {
        case "PENDING":
            return "warning";
        case "APPROVED":
            return "success";
        case "REJECTED":
            return "error";
        case "SENT_BACK":
            return "info";
        default:
            return "default";
    }
};

const getStatusText = (status: string): string => {
    switch (status) {
        case "PENDING": return "Ожидает";
        case "APPROVED": return "Согласован";
        case "REJECTED": return "Отклонен";
        case "SENT_BACK": return "На доработке";
        default: return status;
    }
};

const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('ru-RU', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
};

const MainPage: FunctionComponent = () => {
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);
    const [statusFilter, setStatusFilter] = useState('all');

    const navigate = useNavigate();
    const user = useAppSelector(authSelectors.selectUser);
    const isAuthenticated = useAppSelector(authSelectors.selectIsAuthenticated);
    const isLoading = useAppSelector(authSelectors.selectIsLoading);

    useEffect(() => {
        if (!isAuthenticated && !isLoading) {
            navigate('/');
        }
    }, [isAuthenticated, isLoading, navigate]);

    const filteredLeaves = mockLeaves.filter((leave: LeaveRequest) => {
        const matchesStatus = statusFilter === 'all' || leave.status === statusFilter;
        return matchesStatus;
    });

    const handleChangePage = (_event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const uniqueStatuses = ['all', ...new Set(mockLeaves.map((leave: LeaveRequest) => leave.status))];

    // Показываем загрузку
    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '50vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    // Если не авторизован
    if (!isAuthenticated) {
        return (
            <Box sx={{
                p: 3,
                textAlign: 'center',
                maxWidth: 400,
                mx: 'auto',
                mt: 8
            }}>
                <Typography variant="h5" gutterBottom fontWeight={500}>
                    Мои отпуска
                </Typography>
                <Typography variant="body1" color="text.secondary" sx={{ mt: 2, mb: 3 }}>
                    Войдите в систему, чтобы увидеть свои отпуска
                </Typography>
                <Button
                    variant="contained"
                    size="large"
                    onClick={() => navigate('/login')}
                    sx={{
                        borderRadius: 2,
                        textTransform: 'none',
                        px: 4
                    }}
                >
                    Войти
                </Button>
            </Box>
        );
    }

    // Если авторизован - показываем таблицу
    return (
        <Box sx={{
            p: { xs: 2, md: 4 },
            maxWidth: 1400,
            mx: 'auto',
            width: '100%'
        }}>
            {/* Заголовок и приветствие */}
            <Box sx={{ mb: 4 }}>
                <Typography
                    variant="h4"
                    gutterBottom
                    fontWeight={500}
                    sx={{ fontSize: { xs: '1.75rem', md: '2.125rem' } }}
                >
                    Мои отпуска
                </Typography>
                <Typography variant="body1" color="text.secondary">
                    {user?.firstName} {user?.lastName}, здесь вы можете управлять своими заявками
                </Typography>
            </Box>

            {/* Панель действий */}
            <Stack
                direction={{ xs: 'column', sm: 'row' }}
                spacing={2}
                sx={{ mb: 4 }}
                justifyContent="space-between"
                alignItems={{ xs: 'stretch', sm: 'center' }}
            >
                <Box sx={{ display: 'flex', gap: 2, minWidth: 250 }}>
                    <FormControl size="small" fullWidth>
                        <InputLabel>Статус</InputLabel>
                        <Select
                            value={statusFilter}
                            label="Статус"
                            onChange={(e) => setStatusFilter(e.target.value)}
                            startAdornment={
                                <FilterListIcon sx={{ mr: 1, color: 'action.active' }} />
                            }
                            sx={{
                                borderRadius: 2,
                                '& .MuiOutlinedInput-notchedOutline': {
                                    borderColor: alpha('#000', 0.1)
                                }
                            }}
                        >
                            <MenuItem value="all">Все статусы</MenuItem>
                            {uniqueStatuses.filter(s => s !== 'all').map((status) => (
                                <MenuItem key={status} value={status}>
                                    {getStatusText(status)}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </Box>

                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => navigate('/create-request')}
                    sx={{
                        borderRadius: 2,
                        textTransform: 'none',
                        px: 3,
                        py: 1,
                        boxShadow: 'none',
                        '&:hover': {
                            boxShadow: 'none',
                        }
                    }}
                >
                    Новая заявка
                </Button>
            </Stack>

            {/* Таблица */}
            <Paper
                elevation={0}
                sx={{
                    width: '100%',
                    mb: 2,
                    border: '1px solid',
                    borderColor: alpha('#000', 0.08),
                    borderRadius: 3,
                    overflow: 'hidden'
                }}
            >
                <TableContainer>
                    <Table sx={{ minWidth: 600 }} aria-label="таблица отпусков">
                        <TableHead>
                            <TableRow sx={{
                                backgroundColor: alpha('#000', 0.02),
                                '& th': {
                                    fontWeight: 500,
                                    color: 'text.secondary',
                                    borderBottom: '1px solid',
                                    borderColor: alpha('#000', 0.08),
                                    py: 2
                                }
                            }}>
                                <TableCell>Дата начала</TableCell>
                                <TableCell>Дата окончания</TableCell>
                                <TableCell>Дней</TableCell>
                                <TableCell>Статус</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {filteredLeaves.length > 0 ? (
                                filteredLeaves
                                    .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                                    .map((leave: LeaveRequest) => (
                                        <TableRow
                                            key={leave.id}
                                            hover
                                            sx={{
                                                cursor: 'pointer',
                                                transition: 'all 0.2s',
                                                '&:hover': {
                                                    backgroundColor: alpha('#000', 0.02)
                                                },
                                                '& td': {
                                                    borderBottom: '1px solid',
                                                    borderColor: alpha('#000', 0.04),
                                                    py: 2
                                                },
                                                '&:last-child td': {
                                                    borderBottom: 'none'
                                                }
                                            }}
                                            onClick={() => navigate(`/request/${leave.id}`)}
                                        >
                                            <TableCell>{formatDate(leave.startDate)}</TableCell>
                                            <TableCell>{formatDate(leave.endDate)}</TableCell>
                                            <TableCell>
                                                <Typography variant="body2">
                                                    {leave.days} {leave.days === 1 ? 'день' : leave.days < 5 ? 'дня' : 'дней'}
                                                </Typography>
                                            </TableCell>
                                            <TableCell>
                                                <Chip
                                                    label={getStatusText(leave.status)}
                                                    color={getStatusColor(leave.status)}
                                                    size="small"
                                                    sx={{
                                                        borderRadius: 1.5,
                                                        fontWeight: 500,
                                                        fontSize: '0.75rem',
                                                        height: 24
                                                    }}
                                                />
                                            </TableCell>
                                        </TableRow>
                                    ))
                            ) : (
                                <TableRow>
                                    <TableCell colSpan={4} align="center" sx={{ py: 6 }}>
                                        <Typography variant="body1" color="text.secondary" gutterBottom>
                                            У вас пока нет заявок на отпуск
                                        </Typography>
                                        <Button
                                            variant="text"
                                            onClick={() => navigate('/create-request')}
                                            sx={{
                                                textTransform: 'none',
                                                mt: 1
                                            }}
                                        >
                                            Создать первую заявку
                                        </Button>
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>

                {/* Пагинация */}
                {filteredLeaves.length > rowsPerPage && (
                    <TablePagination
                        rowsPerPageOptions={[5, 10, 25]}
                        component="div"
                        count={filteredLeaves.length}
                        rowsPerPage={rowsPerPage}
                        page={page}
                        onPageChange={handleChangePage}
                        onRowsPerPageChange={handleChangeRowsPerPage}
                        labelRowsPerPage="Строк на странице:"
                        labelDisplayedRows={({ from, to, count }) => `${from}-${to} из ${count}`}
                        sx={{
                            borderTop: '1px solid',
                            borderColor: alpha('#000', 0.08),
                        }}
                    />
                )}
            </Paper>

            {/* Кнопка "Наверх" */}
            <ScrollTop>
                <Fab
                    size="small"
                    aria-label="scroll back to top"
                    sx={{
                        position: 'fixed',
                        bottom: 24,
                        right: 24,
                        boxShadow: 'none',
                        border: '1px solid',
                        borderColor: alpha('#000', 0.08),
                        bgcolor: 'background.paper',
                        '&:hover': {
                            bgcolor: alpha('#000', 0.02)
                        }
                    }}
                >
                    <KeyboardArrowUpIcon />
                </Fab>
            </ScrollTop>
        </Box>
    );
};

export default MainPage;