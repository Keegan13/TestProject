import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDeveloperComponent } from './create-developer.component';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AppRoutingModule } from '../app-routing.module';
import { AppModule } from '../app.module';

describe('CreateDeveloperComponent', () => {
  let component: CreateDeveloperComponent;
  let fixture: ComponentFixture<CreateDeveloperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports:[
        ReactiveFormsModule,
        AppRoutingModule,
        AppModule
      ],
      declarations: [  ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateDeveloperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
