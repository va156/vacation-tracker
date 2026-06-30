import { FetchBaseQueryError } from "@reduxjs/toolkit/query/react";
import { SerializedError } from "@reduxjs/toolkit/react";
import { Envelope } from "../models/Envelope";
import { errorMessages } from "./errorMessages";

export const getErrorMessage = (
	error: FetchBaseQueryError | SerializedError | undefined
) => {
	if (error) {
		// Проверяем, является ли это FetchBaseQueryError (ошибка запроса)
		if ("status" in error) {
			// Проверяем наличие данных ошибки и наличие поля `errors`
			const errorData = error.data as Envelope<null> | undefined;
			if (errorData && errorData.errors) {
				// Извлекаем сообщения об ошибках и пытаемся найти соответствующий перевод
				const messages = errorData.errors.map((err) => {
					// Ищем сообщение на русском языке по коду ошибки
					return (
						errorMessages[err.code] ||
						err.message ||
						"Произошла неизвестная ошибка"
					);
				});

				return messages.join(", ");
			}
			// Если данных нет, возвращаем сообщение о серверной ошибке
			return "Серверная ошибка";
		}
		// Если ошибка не FetchBaseQueryError, возвращаем сообщение об ошибке
		return "Непредвиденная ошибка";
	}
	// Если ошибки нет, возвращаем пустую строку
	return "";
};
