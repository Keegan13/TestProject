import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {BsDatepickerModule} from "ngx-bootstrap/datepicker";
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
import { ViewDeveloperComponent } from './developers/view-developer.component';
import { ListDevelopersComponent } from './developers/list-developers.component';
import { AllDevelopersComponent } from './developers/all-developers.component';

@NgModule({
  declarations: [
    AppComponent,
    CreateDeveloperComponent,
    NavBarComponent,
    HomeComponent,
    UpcommingProjectsComponent,
    ListProjectsComponent,
    CreateProjectComponent,
    ViewDeveloperComponent,
    ListDevelopersComponent,
    AllDevelopersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BsDatepickerModule.forRoot()
  ],
  providers: [{provide: APP_BASE_HREF,useValue:'/'}],
  bootstrap: [AppComponent]
})
export class AppModule { }
