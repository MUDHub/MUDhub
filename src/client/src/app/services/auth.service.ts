import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IUser } from 'src/app/model/auth/IUser';
import {
	ILoginResponse,
	IRegisterResponse,
	IPasswordResetResponse,
	IRegistrationRequest,
} from 'src/app/model/auth/AuthDTO';

import { environment as env } from 'src/environments/environment';
import { IBaseResponse } from '../model/BaseResponse';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	constructor(private http: HttpClient) {
		const sessToken = sessionStorage.getItem('token');
		if (sessToken) {
			this._token = sessToken;
		}

		const sessUser = JSON.parse(sessionStorage.getItem('user'));
		if (sessUser) {
			this._user = sessUser;
		}
	}

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

	public async login(email: string, password: string) {
		try {
			const response = await this.http
				.post<ILoginResponse>(`${env.api.url}/auth/login`, {
					email,
					password,
				})
				.toPromise();

			this.setToken(response.token);
			this.setUser(response.user);
		} catch (err) {
			throw err;
		}
	}

	public async register(user: IRegistrationRequest): Promise<IUser> {
		const response = await this.http.post<IRegisterResponse>(`${env.api.url}/auth/register`, user).toPromise();
		return response.user;
	}

	public async requestReset(mail: string) {
		const response = await this.http
			.get<IPasswordResetResponse>(`${env.api.url}/auth/reset`, {
				params: {
					email: mail,
				},
			})
			.toPromise();
	}

	public async resetPassword(resetKey: string, newPassword: string) {
		return await this.http
			.post<IBaseResponse>(`${env.api.url}/auth/reset`, {
				passwordResetKey: resetKey,
				newPassword,
			})
			.toPromise();
	}

	private setToken(token: string) {
		this._token = token;
		sessionStorage.setItem('token', token);
	}

	private setUser(user: IUser) {
		this._user = user;
		sessionStorage.setItem('user', JSON.stringify(user));
	}

	public logout() {
		this._token = undefined;
		this._user = undefined;
		sessionStorage.removeItem('token');
		sessionStorage.removeItem('user');
	}
}
