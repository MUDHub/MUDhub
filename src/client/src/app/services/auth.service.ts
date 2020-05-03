import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IUser } from 'src/app/model/IUser';
import {
	ILoginResponse,
	IRegisterResponse,
	IPasswordResetResponse,
	IRegistrationRequest,
} from 'src/app/model/AuthDTO';

import { environment as env } from 'src/environments/environment';

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

	public async login(email: string, password: string): Promise<boolean> {
		try {
			const response = await this.http
				.post<ILoginResponse>(`${env.api.url}/auth/login`, {
					email,
					password,
				})
				.toPromise();

			this.setToken(response.token);
			this.setUser(response.user);

			return true;
		} catch (err) {
			console.error(err);
			return false;
		}
	}

	public async register(user: IRegistrationRequest) {
		const response = await this.http
			.post<IRegisterResponse>(`${env.api.url}/auth/register`, {
				user,
			})
			.toPromise();

		console.log(response);
	}

	public async reset(mail: string) {
		const response = await this.http
			.get<IPasswordResetResponse>(`${env.api.url}/auth/reset`, {
				params: { mail },
			})
			.toPromise();

		console.log(response);
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
