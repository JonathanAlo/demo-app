import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styles: [
  ]
})
export class SidebarComponent {
  menuItems: any[] = [];

  ngOnInit(){
    this.menuItems = [
      {icon: 'table', text: 'Table', link: '/'},
      {icon: 'lock', text: 'Encryptation', link: 'admin-files'},
    ]
  }
}
