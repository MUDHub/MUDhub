import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatedGamesComponent } from './created-games.component';

describe('CreatedGamesComponent', () => {
	let component: CreatedGamesComponent;
	let fixture: ComponentFixture<CreatedGamesComponent>;

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [CreatedGamesComponent],
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(CreatedGamesComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
