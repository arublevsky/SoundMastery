import { httpGet } from "../common/requestApi";

const apiController = "individualLessons";

export interface Lesson {
    id: number;
    teacherFullname: string;
    completed: boolean;
    startAt: Date;
}

export const getMyLessons = () => {
    return httpGet<Lesson[]>(`${apiController}/my`);
};
