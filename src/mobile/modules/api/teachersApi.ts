import { httpGet } from "../common/requestApi";

const apiController = "teachers";

export interface TeacherProfile {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
}

export const getMyTeachers = () => {
    return httpGet<TeacherProfile[]>(`${apiController}/my`);
};
