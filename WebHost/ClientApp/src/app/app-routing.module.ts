import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateDeveloperComponent } from './developers/create-developer.component'
import { HomeComponent } from './home/home.component';
import { CreateProjectComponent } from './projects/create-project.component';
import { DeveloperComponent } from './developers/developer.component';
import { AllDevelopersComponent } from './developers/all-developers.component';
import { ProjectComponent } from './projects/project.component';
import { AllProjectsComponent } from './projects/all-projects.component';
import { ActiveProjectsComponent } from './projects/active-projects.component';
import { UpcommingProjectsComponent } from './projects/upcomming-projects.component';
import { CompletedProjectsComponent } from './projects/completed-projects.component';

const routes: Routes = [
  {path:'developers/create',component: CreateDeveloperComponent},
  {path:'projects/create',component: CreateProjectComponent},

  {path:'project/:id',component:ProjectComponent},
  {path:'developer/:id',component:DeveloperComponent},

  {path:'',component: HomeComponent},
 
  {path:'developers/all',component: AllDevelopersComponent},

  {path:'projects/all',component:AllProjectsComponent},
  {path:'projects/completed',component:CompletedProjectsComponent},
  {path:'projects/upcomming',component:UpcommingProjectsComponent},
  {path:'projects/active',component:ActiveProjectsComponent},
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
