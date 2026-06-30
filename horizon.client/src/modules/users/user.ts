export type UserId = string;

export interface User {
    id: number;
    username: string;
    email: string;
    firstName: string;  
    lastName: string;   
    middleName?: string;
    phone?: string;
    roleId: number;
    roleName?: string;
    isEmployee: boolean;
    lastLoginAt?: string;
}