import { Injectable } from '@angular/core';
import { IMudCreateArgs } from '../model/MudDTO';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class MudService {
	constructor(private http: HttpClient) {}


	async createMUD(args: IMudCreateArgs) {
		// TODO: replace url
		return await this.http.post(env.api.path + 'muds', args).toPromise();
	}
}
