import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateDeveloperComponent } from './developers/create-developer.component'
import { HomeComponent } from './home/home.component';
import { CreateProjectComponent } from './projects/create-project.component';

const routes: Routes = [
  {path:'developers/create',component: CreateDeveloperComponent},
  {path:'',component: HomeComponent},
  {path:'projects/create',component: CreateProjectComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
