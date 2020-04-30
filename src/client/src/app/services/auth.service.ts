import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	constructor(private http: HttpClient) {}

	public login(mail: string, password: string) {
		// TODO: implement logic
	}

	public register() {

	}
}
