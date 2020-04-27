import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMudComponent } from './edit-mud.component';

describe('EditMudComponent', () => {
  let component: EditMudComponent;
  let fixture: ComponentFixture<EditMudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditMudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
