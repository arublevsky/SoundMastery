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
    workingHours: { from: number; to: number; }
}

interface UploadAvatarRequest {
    image: string;
}

type UpdateProfileRequest = Omit<UserProfile, 'isTeacher'>;

export const getProfile = () => {
    return httpGet<UserProfile>(`${apiController}/get-profile`);
};

export const updateUserProfile = (body: UpdateProfileRequest) =>
    httpPost<UserProfile>(`${apiController}/update-profile`, { body: body });

export const uploadAvatar = (body: UploadAvatarRequest) =>
    httpPost(`${apiController}/upload-avatar`, { body: body });
