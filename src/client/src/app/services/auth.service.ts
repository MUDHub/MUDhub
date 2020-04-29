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
	public login(mail: string, password: string):boolean {
		return true;
	}

	/**
	 * Register
	 * @param firstname
	 * @param lastname
	 * @param mail
	 * @param password
	 */
	public register(firstname: string, lastname: string, mail: string, password: string): boolean{
		return true;
	}

	/**
	 * 
	 * @param mail 
	 */
	public reset(mail:string): boolean {
		return true;
	}
}
