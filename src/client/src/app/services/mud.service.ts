import { Injectable } from '@angular/core';
import { IMudCreateRequest, IMudCreateResponse } from '../model/MudDTO';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';
import { IMud } from '../model/IMud';

@Injectable({
	providedIn: 'root',
})
export class MudService {
	constructor(private http: HttpClient) {}

	async getAll() {
		return await this.http.get<IMud[]>(`${env.api.url}/muds`).toPromise();
	}

	async create(args: IMudCreateRequest) {
		const response = await this.http.post<IMudCreateResponse>(`${env.api.url}/muds`, args).toPromise();
		return response.mud;
	}
}
