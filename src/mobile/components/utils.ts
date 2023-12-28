import { User } from "../modules/api/profileApi";

export const formatFullName = (user: User) => `${user.firstName} ${user.lastName}`;

export const arrayRange = (start: number, stop: number, step: number) =>
    Array.from(
        { length: (stop - start) / step + 1 },
        (_, index) => start + index * step);

export const isValidUrl = (url?: string | null) => !!url && url.startsWith("http://") && !url.startsWith("https://");