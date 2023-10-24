import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from 'src/assets/material.module';
import { ShellModule } from '../shell/shell.module';

import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminFilesComponent } from './admin-files/admin-files.component';

@NgModule({
  declarations: [
    AdminPanelComponent,
    AdminFilesComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ShellModule
  ],
  exports: [
    
  ]
})
export class AdminModule { }
