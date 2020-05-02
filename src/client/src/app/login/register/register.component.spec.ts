import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterComponent } from './register.component';
import { AuthService } from 'src/app/services/auth.service';

describe('RegisterComponent', () => {
	let component: RegisterComponent;
	let fixture: ComponentFixture<RegisterComponent>;

	/* AUTHSERVICE MOCK */
	let authServiceStub: Partial<AuthService>;
	authServiceStub = {
		async register() {

		}
	};

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [RegisterComponent],
			providers: [ { provide: AuthService, useValue: authServiceStub } ]
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(RegisterComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
