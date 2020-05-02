import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IUser } from '../model/IUser';
import { ILoginResponse, IRegisterResponse, IPasswordResetResponse } from '../model/AuthDTO';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	constructor(private http: HttpClient) {}

	apiPath: string = 'http://localhost:5000/';

	token: string;

	user: IUser;

	public async login(mail: string, password: string) {
		const response = await this.http
			.post<ILoginResponse>(this.apiPath + 'api/auth/login', {
				mail,
				password,
			})
			.toPromise();

		console.log(response);
	}

	public async register(user: IUser) {
		const response = await this.http
			.post<IRegisterResponse>(this.apiPath + 'api/auth/register', {
				user,
			})
			.toPromise();

		console.log(response);
	}

	public async reset(mail: string) {
		const response = await this.http
			.get<IPasswordResetResponse>(this.apiPath + 'api/auth/reset', { params: { mail } })
			.toPromise();

		console.log(response);
	}
}
