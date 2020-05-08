import { Injectable } from '@angular/core';
import { environment as env } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { IAreaCreateRequest, IAreaCreateResponse } from '../model/areas/AreaDTO';
import { IArea } from '../model/areas/IArea';

@Injectable({
	providedIn: 'root',
})
export class AreaService {
	constructor(private http: HttpClient) {}

	/* ##### AREAS ##### */
	public async getAreasForMUD(mudid: string) {
		return await this.http
			.get<IArea[]>(`${env.api.url}/muds/${mudid}/areas`)
			.toPromise();
	}

	public async getArea(mudid: string, areaid: string) {
		return await this.http
			.get(`${env.api.url}/muds/${mudid}/areas/${areaid}`)
			.toPromise();
	}

	public async createArea(mudid: string, area: IAreaCreateRequest) {
		return await this.http
			.post<IAreaCreateResponse>(`${env.api.url}/muds/${mudid}/areas`, area)
			.toPromise();
	}

	public async deleteArea(mudid: string, areaid: string) {
		return await this.http
			.delete(`${env.api.url}/muds/${mudid}/areas/${areaid}`)
			.toPromise();
	}

	/* ##### ROOMS ##### */
	public async getRooms(mudid: string, areaid: string) {
		return await this.http
			.get(`${env.api.url}/muds/${mudid}/areas/${areaid}/rooms`)
			.toPromise();
	}
}
