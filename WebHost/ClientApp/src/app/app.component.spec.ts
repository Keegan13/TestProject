import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { AppModule } from './app.module';
import { CreateDeveloperComponent } from './developers/create-developer.component';
import { CreateProjectComponent } from './projects/create-project.component';
import { Project } from './models/Project';
import { ProjectComponent } from './projects/project.component';
import { DeveloperComponent } from './developers/developer.component';
import { ListDevelopersComponent } from './developers/list-developers.component';
import { ListProjectsComponent } from './projects/list-projects.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ProjectStatusComponent } from './project-status/project-status.component';
import { ListControlsComponent } from './list-controls/list-controls.component';
import { AssignButtonComponent } from './assign-button/assign-button.component';
import { RouterModule } from '@angular/router';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        AppModule
        
      ],
      declarations: [

      ]
    
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'ClientApp'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('ClientApp');
  });

  // it('should render title in a h1 tag', () => {
  //   const fixture = TestBed.createComponent(AppComponent);
  //   fixture.detectChanges();
  //   const compiled = fixture.debugElement.nativeElement;
  //   expect(compiled.querySelector('h1').textContent).toContain('Welcome to ClientApp!');
  // });
});
