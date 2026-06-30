import { baseApi } from "../../shared/api/baseApi";

export interface LeaveDto {
    startDate: string;
    endDate: string;
    leaveTypeId: number;
}

export interface CreateRequestCommand {
    operationTypeId: number;
    employeeId: number;
    departmentId: number;
    approvalTemplateId: number;
    comment?: string;
    leaves: LeaveDto[];
}

export interface RequestSummary {
    id: number;
    requestNumber: string;
    employeeId: number;
    status: string | null;
    statusCode: string | null;
    createdAt: string;
    submittedAt: string | null;
    completedAt: string | null;
    comment: string | null;
}

export const requestsApi = baseApi.injectEndpoints({
    endpoints: (builder) => ({
        createRequest: builder.mutation<{ id: number }, CreateRequestCommand>({
            query: (body) => ({
                url: '/requests',
                method: 'POST',
                body,
            }),
        }),
        getMyRequests: builder.query<RequestSummary[], void>({
            query: () => '/requests',
        }),
    }),
});

export const { useCreateRequestMutation, useGetMyRequestsQuery } = requestsApi;
