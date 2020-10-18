import { httpGet } from "../common/requestApi";

const apiController = "values";

export const loadValues = async () => {
    return httpGet<string[]>(`${apiController}`);
}
