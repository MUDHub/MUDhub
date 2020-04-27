import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatPublicComponent } from './chat-public.component';

describe('ChatPublicComponent', () => {
	let component: ChatPublicComponent;
	let fixture: ComponentFixture<ChatPublicComponent>;

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [ChatPublicComponent],
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(ChatPublicComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
