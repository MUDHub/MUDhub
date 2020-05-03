import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';
import { UserRole } from '../model/UserRole';
import { IUser } from '../model/IUser';

@Injectable({
	providedIn: 'root',
})
export class UsersService {
	constructor(private http: HttpClient) {}

	async getAll() {
		return this.http.get<IUser[]>(`${env.api.url}/users`).toPromise();
	}

	async getById(id: string) {
		return this.http.get<IUser>(`${env.api.url}/users/${id}`).toPromise();
	}

	async addRoleToUser(id: string, role: UserRole) {
		return this.http.post<IUser>(`${env.api.url}/users/${id}/roles?role=${UserRole[role]}`, {}).toPromise();
	}

	async removeRoleFromUser(id: string, role: UserRole) {
		return this.http.delete<IUser>(`${env.api.url}/users/${id}/roles?role=${UserRole[role]}`).toPromise();
	}
}
