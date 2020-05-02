import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import { AuthService } from 'src/app/services/auth.service';

describe('LoginComponent', () => {
	let component: LoginComponent;
	let fixture: ComponentFixture<LoginComponent>;

	/* AUTHSERVICE MOCK */
	let authServiceStub: Partial<AuthService>;
	authServiceStub = {
		async login() {

		}
	};

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [LoginComponent],
			providers: [ { provide: AuthService, useValue: authServiceStub } ]
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
