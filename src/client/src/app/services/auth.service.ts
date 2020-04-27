import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	constructor(private http: HttpClient) {}

	/**
	 * Login
	 * @param mail
	 * @param password
	 */
	public login(mail: string, password: string) {
		// TODO: implement logic
	}

	/**
	 * Register
	 */
	public register(){

	}
}
