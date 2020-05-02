import { Injectable } from '@angular/core';
import { IMudCreateRequest, IMudCreateResponse } from '../model/MudDTO';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class MudService {
	constructor(private http: HttpClient) {}


	async createMUD(args: IMudCreateRequest) {
		const response = await this.http.post<IMudCreateResponse>(env.api.path + 'muds', args).toPromise();
		return response.mud;
	}
}
