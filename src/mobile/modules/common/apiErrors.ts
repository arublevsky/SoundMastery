
export class ApiError {
    constructor(public response: Response, public code: string, public id?: string, public payload?: unknown) {
    }

    public isUnauthenticated() {
        return this.response.status === 401;
    }
}

