import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetComponent } from './reset.component';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';

describe('ResetComponent', () => {
	let component: ResetComponent;
	let fixture: ComponentFixture<ResetComponent>;

	/* AUTHSERVICE MOCK */
	let authServiceStub: Partial<AuthService>;
	authServiceStub = {
		async reset() {
		}
	};

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [ResetComponent],
			providers: [ { provide: AuthService, useValue: authServiceStub } ]
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(ResetComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});
});
