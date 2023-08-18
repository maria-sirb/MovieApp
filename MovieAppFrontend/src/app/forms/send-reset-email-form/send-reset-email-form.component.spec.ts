import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendResetEmailFormComponent } from './send-reset-email-form.component';

describe('SendResetEmailFormComponent', () => {
  let component: SendResetEmailFormComponent;
  let fixture: ComponentFixture<SendResetEmailFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SendResetEmailFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SendResetEmailFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
