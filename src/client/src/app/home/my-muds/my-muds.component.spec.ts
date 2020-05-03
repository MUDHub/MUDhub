import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyMudsComponent } from './my-muds.component';

describe('MyMudsComponent', () => {
	let component: MyMudsComponent;
	let fixture: ComponentFixture<MyMudsComponent>;

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [MyMudsComponent],
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(MyMudsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});
});
