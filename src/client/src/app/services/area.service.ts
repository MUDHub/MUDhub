import { Injectable } from '@angular/core';
import { environment as env } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import {
	IAreaCreateRequest,
	IAreaCreateResponse,
} from '../model/areas/AreaDTO';
import { IArea } from '../model/areas/IArea';
import { IRoom } from '../model/areas/IRoom';
import {
	IRoomDeleteResponse,
	IRoomCreateRequest,
	IRoomCreateResponse,
} from '../model/areas/RoomDTO';
import { Subject } from 'rxjs';
import {
	IConnectionCreateRequest,
	IConnectionCreateResponse,
} from '../model/areas/ConnectionsDTO';
import { IConnection } from '../model/areas/IConnection';
import { IBaseResponse } from '../model/BaseResponse';

@Injectable({
	providedIn: 'root',
})
export class AreaService {
	constructor(private http: HttpClient) {}

	private roomCreatedSubject = new Subject<IRoom>();
	public roomCreated$ = this.roomCreatedSubject.asObservable();

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
			.post<IAreaCreateResponse>(
				`${env.api.url}/muds/${mudid}/areas`,
				area
			)
			.toPromise();
	}

	public async updateArea(
		mudid: string,
		areaid: string,
		area: IAreaCreateRequest
	) {
		return await this.http
			.put<IAreaCreateResponse>(
				`${env.api.url}/muds/${mudid}/areas/${areaid}`,
				area
			)
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
			.get<IRoom[]>(`${env.api.url}/muds/${mudid}/areas/${areaid}/rooms`)
			.toPromise();
	}

	public async createRoom(
		mudid: string,
		areaid: string,
		room: IRoomCreateRequest
	) {
		try {
			const response = await this.http
				.post<IRoomCreateResponse>(
					`${env.api.url}/muds/${mudid}/areas/${areaid}/rooms`,
					room
				)
				.toPromise();
			this.roomCreatedSubject.next(response.room);
			return response;
		} catch (err) {
			throw err;
		}
	}

	public async deleteRoom(mudid: string, areaid: string, roomid: string) {
		return await this.http
			.delete<IRoomDeleteResponse>(
				`${env.api.url}/muds/${mudid}/areas/${areaid}/rooms/${roomid}`
			)
			.toPromise();
	}

	/* ##### ROOMS ##### */
	public async getConnections(
		mudid: string,
		areaid: string,
		roomid?: string
	) {
		return await this.http
			.get<IConnection[]>(
				`${env.api.url}/muds/${mudid}/areas/${areaid}/connections${
					roomid ? '?roomId=' + roomid : ''
				}`
			)
			.toPromise();
	}

	public async getConnection(mudid: string, areaid: string, connid: string) {
		return await this.http
			.get<IConnection>(
				`${env.api.url}/muds/${mudid}/areas/${areaid}/connections/${connid}`
			)
			.toPromise();
	}

	public async createConnection(
		mudid: string,
		areaid: string,
		connection: IConnectionCreateRequest
	) {
		return await this.http
			.post<IConnectionCreateResponse>(
				`${env.api.url}/muds/${mudid}/areas/${areaid}/connections`,
				connection
			)
			.toPromise();
	}

	public async deleteConnection(
		mudid: string,
		areaid: string,
		connid: string
	) {
		return await this.http
			.delete<IBaseResponse>(
				`${env.api.url}/muds/${mudid}/areas/${areaid}/connections/${connid}`
			)
			.toPromise();
	}
}
