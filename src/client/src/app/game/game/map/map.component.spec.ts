import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapComponent } from './map.component';
import { IRoom } from 'src/app/model/IRoom';
import { RoomsService } from 'src/app/services/rooms.service';

describe('MapComponent', () => {
	let component: MapComponent;
	let fixture: ComponentFixture<MapComponent>;
	let roomsServiceStub: Partial<RoomsService>;

	roomsServiceStub = {
		getRoomsForArea() {
			return [
				{
					name: 'TestRoom 1',
					position: { x: 0, y: 0},
					connections: { e: true, n: true }
				},
				{
					name: 'TestRoom 2',
					position: { x: 1, y: 0},
					connections: { w: true }
				},
				{
					name: 'TestRoom 3',
					position: { x: 0, y: 1},
					connections: { s: true }
				},
			];
		}
	};



	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [MapComponent],
			providers: [ { provide: RoomsService, useValue: roomsServiceStub } ]
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(MapComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});




	it('should create', () => {
		expect(component).toBeTruthy();
	});

	it('should return correct dimensions', () => {
		expect(component.width).toBe(2);
		expect(component.height).toBe(2);
	});
});
