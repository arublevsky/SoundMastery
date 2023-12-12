import { httpGet, httpPost } from "../common/requestApi";

const apiController = "profile";

export interface UserProfile {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
}

export const getProfile = () => {
    return httpGet<UserProfile>(`${apiController}/get-profile`);
};

export const saveProfile = (body: UserProfile) => {
    return httpPost<void>(`${apiController}/save-profile`, { body: body });
};
