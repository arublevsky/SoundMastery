import { httpGet, httpPost } from "../common/requestApi";

const apiController = "profile";

export interface UserProfile {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
}

export const getProfile = async () => {
    return httpGet<UserProfile>(`${apiController}/get-profile`);
};

export const saveProfile = async (body: UserProfile) => {
    return httpPost<void>(`${apiController}/save-profile`, { body: body });
};
