import { User } from "../modules/api/profileApi";

export const formatFullName = (user: User) => `${user.firstName} ${user.lastName}`;
