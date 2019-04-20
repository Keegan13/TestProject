import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateDeveloperComponent } from './developers/create-developer.component'
import { HomeComponent } from './home/home.component';
import { CreateProjectComponent } from './projects/create-project.component';
import { DeveloperComponent } from './developers/developer.component';
import { ListDevelopersComponent } from './developers/list-developers.component';
import { AllDevelopersComponent } from './developers/all-developers.component';
import { ProjectComponent } from './projects/project.component';
import { AllProjectsComponent } from './projects/all-projects.component';
import { EditDeveloperComponent } from './edit-developer/edit-developer.component';

const routes: Routes = [
  {path:'developers/create',component: CreateDeveloperComponent},
  {path:'developer/:id',component:DeveloperComponent},
  {path:'',component: HomeComponent},
  {path:'projects/create',component: CreateProjectComponent},
  {path:'developers',component: ListDevelopersComponent},
  {path:'developers/all',component: AllDevelopersComponent},
  {path:'project/:id',component:ProjectComponent},
  {path:'projects/all',component:AllProjectsComponent},
  {path:'developers/edit/:id',component:EditDeveloperComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
