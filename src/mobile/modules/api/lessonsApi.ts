import {httpGet, httpPost, httpPut} from "../common/requestApi";
import {User} from "./profileApi.ts";

const apiController = "individualLessons";

export interface Lesson {
    id: number;
    teacher: User;
    student: User;
    completed: boolean;
    cancelled: boolean;
    description: string;
    date: string;
    hour: number;
}

export interface AddLessonRequest {
    teacherId: string;
    description: string;
    date: Date;
    hour: number;
}

export const getMyLessons = () => httpGet<Lesson[]>(`${apiController}/my`);

export const getAvailableLessons = (teacher: string, date: Date) => {
    return httpGet<{ availableHours: number[] }>(
        `${apiController}/availability?teacherId=${teacher}&date=${date.toISOString()}`
    );
};

export const addLesson = (body: AddLessonRequest) => httpPut(`${apiController}/add`, {body: body});

export const cancel = (lessonId: number) => httpPost(`${apiController}/cancel/${lessonId}`);

export const complete = (lessonId: number) => httpPost(`${apiController}/complete/${lessonId}`);
