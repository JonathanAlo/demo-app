import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminPanelComponent } from './feature/admin/admin-panel/admin-panel.component';
import { AdminFilesComponent } from './feature/admin/admin-files/admin-files.component';

const routes: Routes = [
  { path: '', component: AdminPanelComponent },
  { path: 'admin-files', component: AdminFilesComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
