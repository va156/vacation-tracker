import {
	createAsyncThunk,
	ThunkAction,
	UnknownAction,
} from "@reduxjs/toolkit/react";
import { useDispatch, useSelector, useStore } from "react-redux";
import { extraArgument, store } from "../app/store";

export type AppState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export type AppThunk<R = void> = ThunkAction<
	R,
	AppState,
	typeof extraArgument,
	UnknownAction
>;

export const useAppStore = useStore.withTypes<typeof store>();
export const useAppSelector = useSelector.withTypes<AppState>();
export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const createAppAsyncThunk = createAsyncThunk.withTypes<{
	state: AppState;
	dispatch: AppDispatch;
	extra: typeof extraArgument;
	rejectValue: string;
}>();
