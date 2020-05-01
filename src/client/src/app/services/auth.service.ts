import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IUser } from "../model/IUser";

@Injectable({
	providedIn: "root",
})
export class AuthService {
	constructor(private http: HttpClient) {}

	apiPath: string = "http://localhost:5000/";

	token: string;

	user: IUser;

	public login(mail: string, password: string) {
		this.http
			.post<string>(this.apiPath + "api/auth/login", { mail, password })
			.toPromise()
			.then((res) => {
				console.log("Login erfolgreich", res);
			})
			.catch((err) => console.log("Login fehlgeschlagen: ", err));
	}

	public register(user: IUser) {
		this.http
			.post<IUser>(this.apiPath + "api/auth/register", { user })
			.toPromise()
			.then((res) => {
				console.log("Registrierung erfolgreich: ", res);
			})
			.catch((err) => {
				console.log("Registrierung fehlgeschlagen: ", err);
			});
	}

	public reset(mail: string) {
		this.http
			.get<string>(this.apiPath + "api/auth/reset", { params: { mail } })
			.toPromise()
			.then((res) => {
				console.log(res);
			})
			.catch((err) => {
				console.log(err);
			});
	}
}
