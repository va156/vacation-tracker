import { configureStore } from "@reduxjs/toolkit";
import authReducer from "../modules/auth/authSlice";
import { baseApi } from "../shared/api/baseApi";
import { router } from "./router";

export const extraArgument = {
	router,
};

export const store = configureStore({
	reducer: {
		[baseApi.reducerPath]: baseApi.reducer,
		auth: authReducer,
	},
	middleware: (getDefaultMiddleware) =>
		getDefaultMiddleware({ thunk: { extraArgument } }).concat(
			baseApi.middleware
		),
});
