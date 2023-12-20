import { httpGet, httpPost } from "../common/requestApi";

const apiController = "profile";

export interface User {
    id: number;
    email: string;
    firstName: string;
    lastName: string;
    userName: string;
    avatar: string;
}

export interface UserProfile {
    user: User;
    isTeacher: boolean;
}

export const getProfile = () => {
    return httpGet<UserProfile>(`${apiController}/get-profile`);
};

export const updateProfile = (body: UserProfile) => {
    return httpPost<void>(`${apiController}/update-profile`, { body: body });
};
