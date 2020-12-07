
export class ApiError implements Error {
    constructor(public response: Response, public code: string, public id?: string, public payload?: unknown) { }

    public name: string;
    public message: string;
    public stack?: string;

    public isUnauthenticated() {
        return this.response.status === 401;
    }
}

