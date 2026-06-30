export type Error = {
	code: string;
	message: string;
	type: ErrorType;
	invalidField: string | null;
};

export enum ErrorType {
	Validation = 0,
	NotFound,
	Failure,
	Conflict,
}
