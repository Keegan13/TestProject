import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";
import { ReactiveFormsModule} from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CreateDeveloperComponent } from './developers/create-developer.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { from } from 'rxjs';
import { HomeComponent } from './home/home.component';
import { UpcommingProjectsComponent } from './upcomming-projects/upcomming-projects.component';
import { ListProjectsComponent } from './projects/list-projects.component';
import { CreateProjectComponent } from './projects/create-project.component';
import {HttpClientModule} from '@angular/common/http';
import { APP_BASE_HREF } from '@angular/common';
import { DeveloperComponent } from './developers/developer.component';
import { ListDevelopersComponent } from './developers/list-developers.component';
import { AllDevelopersComponent } from './developers/all-developers.component';
import { AssignComponent } from './assign/assign.component';
import { ProjectComponent } from './projects/project.component';
import { SearchComponentComponent } from './search-component/search-component.component';
import { AllProjectsComponent } from './projects/all-projects.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { EditDeveloperComponent } from './edit-developer/edit-developer.component';
import { AssignButtonComponent } from './assign-button/assign-button.component';


@NgModule({
  declarations: [
    AppComponent,
    CreateDeveloperComponent,
    NavBarComponent,
    HomeComponent,
    UpcommingProjectsComponent,
    ListProjectsComponent,
    CreateProjectComponent,
    DeveloperComponent,
    ListDevelopersComponent,
    AllDevelopersComponent,
    AssignComponent,
    ProjectComponent,
    SearchComponentComponent,
    AllProjectsComponent,
    EditDeveloperComponent,
    AssignButtonComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot()
  ],
  providers: [{provide: APP_BASE_HREF,useValue:'/'}],
  bootstrap: [AppComponent]
})
export class AppModule { }
