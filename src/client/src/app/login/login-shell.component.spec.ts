import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginShellComponent } from './login-shell.component';

describe('LoginComponent', () => {
	let component: LoginShellComponent;
	let fixture: ComponentFixture<LoginShellComponent>;

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [ LoginShellComponent ]
		})
		.compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(LoginShellComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
