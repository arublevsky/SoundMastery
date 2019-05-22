class AuthStore {
    private loggedIn: boolean = false;

    public login() {
        this.loggedIn = true;
    }

    public logout() {
        this.loggedIn = false;
    }

    public get isLoggedIn() {
        return this.loggedIn;
    }
}

const authStore = new AuthStore();
export default authStore;