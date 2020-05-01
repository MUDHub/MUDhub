import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeShellComponent } from './home-shell.component';

describe('HomeShellComponent', () => {
	let component: HomeShellComponent;
	let fixture: ComponentFixture<HomeShellComponent>;

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [HomeShellComponent],
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(HomeShellComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});
});
