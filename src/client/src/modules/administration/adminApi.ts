import { httpGet } from "../common/requestApi";

const apiController = "administration";

export const getUsers = async () => {
    const result = await httpGet(`${apiController}/get-users`);
    console.log(result);
    return result;
};
