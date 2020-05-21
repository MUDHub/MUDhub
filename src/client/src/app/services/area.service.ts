import { Injectable } from '@angular/core';
import { environment as env } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import {
	IAreaCreateRequest,
	IAreaCreateResponse,
	IAreaUpdateRequest,
} from '../model/areas/AreaDTO';
import { IArea } from '../model/areas/IArea';
import { IRoom } from '../model/areas/IRoom';
import {
	IRoomDeleteResponse,
	IRoomCreateRequest,
	IRoomCreateResponse,
	IRoomUpdateRequest,
	IRoomCreateItemRequest,
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
	private roomUpdatedSubject = new Subject<IRoom>();
	public roomUpdated$ = this.roomUpdatedSubject.asObservable();

	/* ##### AREAS ##### */
	public async getAreasForMud(mudId: string) {
		return await this.http
			.get<IArea[]>(`${env.api.url}/areas`, {
				params: {
					mudId,
				},
			})
			.toPromise();
	}

	public async getArea(areaid: string) {
		return await this.http
			.get<IArea>(`${env.api.url}/areas/${areaid}`)
			.toPromise();
	}

	public async createArea(area: IAreaCreateRequest) {
		return await this.http
			.post<IAreaCreateResponse>(`${env.api.url}/areas`, area)
			.toPromise();
	}

	public async updateArea(areaid: string, area: IAreaUpdateRequest) {
		return await this.http
			.put<IAreaCreateResponse>(`${env.api.url}/areas/${areaid}`, area)
			.toPromise();
	}

	public async deleteArea(areaid: string) {
		return await this.http
			.delete(`${env.api.url}/areas/${areaid}`)
			.toPromise();
	}

	/* ##### ROOMS ##### */
	public async getRoomsForMud(mudId: string) {
		return await this.http
			.get<IRoom[]>(`${env.api.url}/rooms`, { params: { mudId } })
			.toPromise();
	}

	public async getRoomsForArea(areaId: string) {
		return await this.http
			.get<IRoom[]>(`${env.api.url}/rooms`, { params: { areaId } })
			.toPromise();
	}

	public async getRoom(roomid: string) {
		return await this.http
			.get<IRoom>(`${env.api.url}/rooms/${roomid}`)
			.toPromise();
	}

	public async createRoom(room: IRoomCreateRequest) {
		try {
			const response = await this.http
				.post<IRoomCreateResponse>(`${env.api.url}/rooms`, room)
				.toPromise();
			this.roomCreatedSubject.next(response.room);
			return response;
		} catch (err) {
			throw err;
		}
	}

	public async updateRoom(roomid: string, room: IRoomUpdateRequest) {
		try {
			const response = await this.http
				.put<IRoomCreateResponse>(
					`${env.api.url}/rooms/${roomid}`,
					room
				)
				.toPromise();
			this.roomUpdatedSubject.next(response.room);
			return response;
		} catch (err) {
			throw err;
		}
	}

	public async deleteRoom(roomid: string) {
		return await this.http
			.delete<IRoomDeleteResponse>(`${env.api.url}/rooms/${roomid}`)
			.toPromise();
	}

	/* ##### Room Items ##### */
	public async getItemsForRoom(roomid: string) {
		const room = await this.getRoom(roomid);
		return room.itemInstances;
	}

	public async addItemToRoom(roomid: string, args: IRoomCreateItemRequest) {
		return await this.http
			.post(`${env.api.url}/rooms/${roomid}/iteminstances`, args)
			.toPromise();
	}

	public async removeItemFromRoom(roomid: string, iteminstanceid: string) {
		return await this.http
			.delete(`${env.api.url}/rooms/${roomid}/iteminstances/${iteminstanceid}`)
			.toPromise();
	}

	/* ##### Connections ##### */
	public async getConnectionsForArea(areaid: string) {
		return await this.http
			.get<IConnection[]>(`${env.api.url}/connections`, {
				params: {
					areaId: areaid,
				},
			})
			.toPromise();
	}

	public async getConnectionsForRoom(roomid: string) {
		return await this.http
			.get<IConnection[]>(`${env.api.url}/connections`, {
				params: {
					roomId: roomid,
				},
			})
			.toPromise();
	}

	public async getConnection(connid: string) {
		return await this.http
			.get<IConnection>(`${env.api.url}/connections/${connid}`)
			.toPromise();
	}

	public async createConnection(connection: IConnectionCreateRequest) {
		return await this.http
			.post<IConnectionCreateResponse>(
				`${env.api.url}/connections`,
				connection
			)
			.toPromise();
	}

	public async deleteConnection(connid: string) {
		return await this.http
			.delete<IBaseResponse>(`${env.api.url}/connections/${connid}`)
			.toPromise();
	}
}
