import { baseApi } from "../../shared/api/baseApi";

export interface LeaveRequest {
    id: number;
    startDate: string;
    endDate: string;
    status: string;
    days: number;
}

export const leaveApi = baseApi.injectEndpoints({
    endpoints: (builder) => ({
        getMyLeaves: builder.query<LeaveRequest[], void>({
            query: () => '/leaves/my',
        }),
        getLeaveById: builder.query<LeaveRequest, number>({
            query: (id) => `/leaves/${id}`,
        }),
    }),
});

export const { useGetMyLeavesQuery, useGetLeaveByIdQuery } = leaveApi;