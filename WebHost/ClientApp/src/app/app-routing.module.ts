import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateDeveloperComponent } from './developers/create-developer.component'
import { HomeComponent } from './home/home.component';
import { CreateProjectComponent } from './projects/create-project.component';
import { ViewDeveloperComponent } from './developers/view-developer.component';
import { ListDevelopersComponent } from './developers/list-developers.component';
import { AllDevelopersComponent } from './developers/all-developers.component';

const routes: Routes = [
  {path:'developers/create',component: CreateDeveloperComponent},
  {path:'developer/:id',component:ViewDeveloperComponent},
  {path:'',component: HomeComponent},
  {path:'projects/create',component: CreateProjectComponent},
  {path:'developers',component: ListDevelopersComponent},
  {path:'developers/all',component: AllDevelopersComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
