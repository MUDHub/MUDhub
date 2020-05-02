import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import { AuthService } from 'src/app/services/auth.service';
import { Router, NavigationExtras } from '@angular/router';

describe('LoginComponent', () => {
	let component: LoginComponent;
	let fixture: ComponentFixture<LoginComponent>;

	/* AUTHSERVICE MOCK */
	let authServiceStub: Partial<AuthService>;
	authServiceStub = {
		async login() {
			return true;
		},
	};

	let routerStub: Partial<Router>;
	routerStub = {
		navigate(commands: any[], extras?: NavigationExtras) {
			return Promise.resolve(true);
		},
	};

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [LoginComponent],
			providers: [
				{ provide: AuthService, useValue: authServiceStub },
				{ provide: Router, useValue: routerStub },
			],
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(LoginComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
