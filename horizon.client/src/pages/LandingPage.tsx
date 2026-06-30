import { Box, Button, Container, Typography, Paper, alpha, Grid, Card, CardContent } from "@mui/material";
import { useNavigate } from "react-router-dom";
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import HistoryIcon from '@mui/icons-material/History';
import PeopleIcon from '@mui/icons-material/People';

export default function LandingPage() {
    const navigate = useNavigate();
    const primaryColor = '#1976d2';

    return (
        <Box sx={{
            minHeight: '100%',
            bgcolor: '#ffffff',
            py: 4
        }}>
            <Container maxWidth="lg">
                {/* Основная карточка с чистым градиентом */}
                <Paper
                    elevation={0}
                    sx={{
                        p: { xs: 4, md: 6 },
                        mb: 6,
                        borderRadius: 4,
                        background: `linear-gradient(135deg, ${primaryColor} 0%, #64b5f6 100%)`,
                        color: 'white',
                        textAlign: 'center',
                    }}
                >
                    <Typography variant="h3" gutterBottom fontWeight={500}>
                        Портал управления отпусками
                    </Typography>
                    <Typography variant="h6" sx={{ mb: 4, opacity: 0.9, fontWeight: 400 }}>
                        Простой и удобный сервис для сотрудников компании
                    </Typography>

                    <Box sx={{
                        display: 'flex',
                        gap: 2,
                        justifyContent: 'center',
                        flexDirection: { xs: 'column', sm: 'row' }
                    }}>
                        <Button
                            variant="contained"
                            size="large"
                            onClick={() => navigate('/login')}
                            sx={{
                                px: 4,
                                py: 1.5,
                                borderRadius: 2,
                                textTransform: 'none',
                                fontSize: '1.1rem',
                                bgcolor: 'white',
                                color: primaryColor,
                                '&:hover': {
                                    bgcolor: alpha('#fff', 0.9),
                                }
                            }}
                        >
                            Войти
                        </Button>

                        <Button
                            variant="outlined"
                            size="large"
                            onClick={() => navigate('/registration')}
                            sx={{
                                px: 4,
                                py: 1.5,
                                borderRadius: 2,
                                textTransform: 'none',
                                fontSize: '1.1rem',
                                borderColor: 'white',
                                color: 'white',
                                borderWidth: 2,
                                '&:hover': {
                                    borderColor: 'white',
                                    bgcolor: alpha('#fff', 0.1),
                                }
                            }}
                        >
                            Регистрация
                        </Button>
                    </Box>
                </Paper>

                {/* Информационные плашки */}
                <Typography variant="h5" gutterBottom sx={{ mb: 3, fontWeight: 500 }}>
                    Возможности портала
                </Typography>

                <Grid container spacing={3}>
                    {/* Плашка 1 */}
                    <Grid item xs={12} md={6} lg={3}>
                        <Card sx={{
                            height: '100%',
                            borderRadius: 3,
                            boxShadow: '0 2px 8px rgba(25, 118, 210, 0.05)',
                            transition: 'transform 0.2s',
                            border: '1px solid',
                            borderColor: alpha(primaryColor, 0.1),
                            '&:hover': {
                                transform: 'translateY(-4px)',
                                boxShadow: `0 4px 12px ${alpha(primaryColor, 0.2)}`,
                            }
                        }}>
                            <CardContent sx={{ p: 3 }}>
                                <CalendarMonthIcon sx={{ fontSize: 40, color: primaryColor, mb: 2 }} />
                                <Typography variant="h6" gutterBottom fontWeight={500}>
                                    Планирование отпусков
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    Создавайте заявки на отпуск и выбирайте удобные даты
                                </Typography>
                            </CardContent>
                        </Card>
                    </Grid>

                    {/* Плашка 2 */}
                    <Grid item xs={12} md={6} lg={3}>
                        <Card sx={{
                            height: '100%',
                            borderRadius: 3,
                            boxShadow: '0 2px 8px rgba(25, 118, 210, 0.05)',
                            transition: 'transform 0.2s',
                            border: '1px solid',
                            borderColor: alpha(primaryColor, 0.1),
                            '&:hover': {
                                transform: 'translateY(-4px)',
                                boxShadow: `0 4px 12px ${alpha(primaryColor, 0.2)}`,
                            }
                        }}>
                            <CardContent sx={{ p: 3 }}>
                                <CheckCircleIcon sx={{ fontSize: 40, color: primaryColor, mb: 2 }} />
                                <Typography variant="h6" gutterBottom fontWeight={500}>
                                    Согласование заявок
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    Руководители могут согласовывать или отклонять заявки
                                </Typography>
                            </CardContent>
                        </Card>
                    </Grid>

                    {/* Плашка 3 */}
                    <Grid item xs={12} md={6} lg={3}>
                        <Card sx={{
                            height: '100%',
                            borderRadius: 3,
                            boxShadow: '0 2px 8px rgba(25, 118, 210, 0.05)',
                            transition: 'transform 0.2s',
                            border: '1px solid',
                            borderColor: alpha(primaryColor, 0.1),
                            '&:hover': {
                                transform: 'translateY(-4px)',
                                boxShadow: `0 4px 12px ${alpha(primaryColor, 0.2)}`,
                            }
                        }}>
                            <CardContent sx={{ p: 3 }}>
                                <HistoryIcon sx={{ fontSize: 40, color: primaryColor, mb: 2 }} />
                                <Typography variant="h6" gutterBottom fontWeight={500}>
                                    История отпусков
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    Просматривайте историю своих отпусков и баланс дней
                                </Typography>
                            </CardContent>
                        </Card>
                    </Grid>

                    {/* Плашка 4 */}
                    <Grid item xs={12} md={6} lg={3}>
                        <Card sx={{
                            height: '100%',
                            borderRadius: 3,
                            boxShadow: '0 2px 8px rgba(25, 118, 210, 0.05)',
                            transition: 'transform 0.2s',
                            border: '1px solid',
                            borderColor: alpha(primaryColor, 0.1),
                            '&:hover': {
                                transform: 'translateY(-4px)',
                                boxShadow: `0 4px 12px ${alpha(primaryColor, 0.2)}`,
                            }
                        }}>
                            <CardContent sx={{ p: 3 }}>
                                <PeopleIcon sx={{ fontSize: 40, color: primaryColor, mb: 2 }} />
                                <Typography variant="h6" gutterBottom fontWeight={500}>
                                    Командный календарь
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    Видите, кто из коллег в отпуске в любой момент
                                </Typography>
                            </CardContent>
                        </Card>
                    </Grid>
                </Grid>

                {/* Дополнительная информация */}
                <Box sx={{ mt: 6, textAlign: 'center' }}>
                    <Typography variant="body2" color="text.secondary">
                        Для доступа к порталу используйте корпоративный email
                    </Typography>
                </Box>
            </Container>
        </Box>
    );
}