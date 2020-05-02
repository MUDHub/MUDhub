import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IUser } from 'src/app/model/IUser';
import {
	ILoginResponse,
	IRegisterResponse,
	IPasswordResetResponse,
} from 'src/app/model/AuthDTO';

import { environment as env } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	constructor(private http: HttpClient) {}

	private _token: string;
	public get token() {
		return this._token;
	}

	private _user: IUser;
	public get user() {
		return this._user;
	}

	public get isLoggedIn() {
		return this._token !== undefined;
	}

	public async login(email: string, password: string): Promise<boolean> {
		try {
			const response = await this.http
				.post<ILoginResponse>(env.api.path + 'auth/login', {
					email,
					password,
				})
				.toPromise();

			this._token = response.token;
			this._user = response.user;

			return true;
		} catch (err) {
			console.error(err);
			return false;
		}
	}

	public async register(user: IUser) {
		const response = await this.http
			.post<IRegisterResponse>(env.api.path + 'auth/register', {
				user,
			})
			.toPromise();

		console.log(response);
	}

	public async reset(mail: string) {
		const response = await this.http
			.get<IPasswordResetResponse>(env.api.path + 'auth/reset', {
				params: { mail },
			})
			.toPromise();

		console.log(response);
	}

	public logout() {
		this._token = undefined;
		this._user = undefined;
	}
}
