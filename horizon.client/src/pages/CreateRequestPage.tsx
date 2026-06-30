import { useState } from "react";
import {
    Box, Button, Typography, TextField, MenuItem, Select,
    FormControl, InputLabel, Paper, Stack, Alert,
    CircularProgress, Divider, alpha
} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import SendIcon from "@mui/icons-material/Send";
import { useNavigate } from "react-router-dom";
import { useCreateRequestMutation } from "../modules/requests/requestsApi";

// Leave types matching the seeded reference data
const LEAVE_TYPES = [
    { id: 1, label: "Ежегодный оплачиваемый" },
    { id: 2, label: "Больничный" },
    { id: 3, label: "Без сохранения зарплаты" },
    { id: 4, label: "Декретный" },
];

// Defaults matching seeded ApprovalTemplate and OperationType
const DEFAULT_OPERATION_TYPE_ID = 1;  // NEW_LEAVE
const DEFAULT_APPROVAL_TEMPLATE_ID = 1; // STANDARD
const DEFAULT_EMPLOYEE_ID = 1; // TODO: replace with current user's employeeId
const DEFAULT_DEPARTMENT_ID = 1; // TODO: replace with current user's departmentId

const CreateRequestPage = () => {
    const navigate = useNavigate();
    const [createRequest, { isLoading }] = useCreateRequestMutation();

    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [leaveTypeId, setLeaveTypeId] = useState<number>(1);
    const [comment, setComment] = useState("");
    const [error, setError] = useState<string | null>(null);

    const isFormValid = startDate && endDate && new Date(endDate) >= new Date(startDate);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        try {
            const result = await createRequest({
                operationTypeId: DEFAULT_OPERATION_TYPE_ID,
                employeeId: DEFAULT_EMPLOYEE_ID,
                departmentId: DEFAULT_DEPARTMENT_ID,
                approvalTemplateId: DEFAULT_APPROVAL_TEMPLATE_ID,
                comment: comment || undefined,
                leaves: [
                    {
                        startDate: new Date(startDate).toISOString(),
                        endDate: new Date(endDate).toISOString(),
                        leaveTypeId,
                    },
                ],
            }).unwrap();

            console.info("Request created with id:", result.id);
            navigate("/dashboard");
        } catch (err: unknown) {
            const message = err && typeof err === "object" && "data" in err
                ? (err as { data?: { detail?: string; title?: string } }).data?.detail
                  ?? (err as { data?: { title?: string } }).data?.title
                  ?? "Ошибка при создании заявки"
                : "Ошибка при создании заявки";
            setError(message);
        }
    };

    return (
        <Box sx={{ p: { xs: 2, md: 4 }, maxWidth: 680, mx: "auto" }}>
            <Button
                startIcon={<ArrowBackIcon />}
                onClick={() => navigate("/dashboard")}
                sx={{ mb: 3, textTransform: "none" }}
            >
                Назад
            </Button>

            <Typography variant="h4" fontWeight={500} gutterBottom>
                Новая заявка на отпуск
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 4 }}>
                Заполните форму, чтобы подать заявку на согласование
            </Typography>

            <Paper
                elevation={0}
                sx={{
                    p: { xs: 2, md: 3 },
                    border: "1px solid",
                    borderColor: alpha("#000", 0.08),
                    borderRadius: 3,
                }}
                component="form"
                onSubmit={handleSubmit}
            >
                <Stack spacing={3}>
                    {error && (
                        <Alert severity="error" onClose={() => setError(null)}>
                            {error}
                        </Alert>
                    )}

                    {/* Leave type */}
                    <FormControl fullWidth>
                        <InputLabel id="leave-type-label">Тип отпуска</InputLabel>
                        <Select
                            labelId="leave-type-label"
                            value={leaveTypeId}
                            label="Тип отпуска"
                            onChange={(e) => setLeaveTypeId(Number(e.target.value))}
                        >
                            {LEAVE_TYPES.map((lt) => (
                                <MenuItem key={lt.id} value={lt.id}>
                                    {lt.label}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>

                    {/* Date range */}
                    <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                        <TextField
                            label="Дата начала"
                            type="date"
                            fullWidth
                            required
                            InputLabelProps={{ shrink: true }}
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                            inputProps={{ min: new Date().toISOString().split("T")[0] }}
                        />
                        <TextField
                            label="Дата окончания"
                            type="date"
                            fullWidth
                            required
                            InputLabelProps={{ shrink: true }}
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            inputProps={{ min: startDate || new Date().toISOString().split("T")[0] }}
                            error={!!startDate && !!endDate && new Date(endDate) < new Date(startDate)}
                            helperText={
                                startDate && endDate && new Date(endDate) < new Date(startDate)
                                    ? "Дата окончания должна быть не раньше даты начала"
                                    : undefined
                            }
                        />
                    </Stack>

                    {/* Duration preview */}
                    {startDate && endDate && new Date(endDate) >= new Date(startDate) && (
                        <Typography variant="body2" color="text.secondary">
                            Продолжительность: {
                                Math.round(
                                    (new Date(endDate).getTime() - new Date(startDate).getTime()) /
                                    (1000 * 60 * 60 * 24)
                                ) + 1
                            } кал. дней
                        </Typography>
                    )}

                    <Divider />

                    {/* Comment */}
                    <TextField
                        label="Комментарий (необязательно)"
                        multiline
                        rows={3}
                        fullWidth
                        value={comment}
                        onChange={(e) => setComment(e.target.value)}
                        placeholder="Укажите причину или дополнительные сведения"
                    />

                    <Button
                        type="submit"
                        variant="contained"
                        size="large"
                        endIcon={isLoading ? <CircularProgress size={18} color="inherit" /> : <SendIcon />}
                        disabled={!isFormValid || isLoading}
                        sx={{
                            borderRadius: 2,
                            textTransform: "none",
                            boxShadow: "none",
                            "&:hover": { boxShadow: "none" },
                        }}
                    >
                        {isLoading ? "Отправка..." : "Подать заявку"}
                    </Button>
                </Stack>
            </Paper>
        </Box>
    );
};

export default CreateRequestPage;
