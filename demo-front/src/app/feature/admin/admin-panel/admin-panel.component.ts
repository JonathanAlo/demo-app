import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { User } from 'src/app/data-access/interfaces/user';
import { DataService } from 'src/app/data-access/services/data.service';
declare var $: any;//use jQuery

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  //imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule],
})
export class AdminPanelComponent {
  displayedColumns: string[] = ['#', 'name', 'email', 'role', 'created_at', 'actions'];
  dataSource: MatTableDataSource<User>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  key: boolean = false;

  constructor(private _getUsers: DataService) {
    // Create 100 users
    const users = Array.from({ length: 100 }, (_, k) => createNewUser(k + 1));

    // Assign the data to the data source for the table to render
    this.dataSource = new MatTableDataSource(users);
  }

  ngOnInit(){
    this.getUsersApi();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  getUsersApi(){
    const users = this._getUsers.UserService().subscribe((res: any) => {
      console.log(res)
    });
    console.log(users);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  action(action: boolean){
    (action)? this.key = true : this.key = false;
  }

  modalMannager(nModa: string, action: string){
    $('#'+nModa).modal(action);
  }
}

/** Builds and returns a new User. */
function createNewUser(id: number): User {
  const firstname =
    NAMES[Math.round(Math.random() * (NAMES.length - 1))] +
    ' ' +
    NAMES[Math.round(Math.random() * (NAMES.length - 1))].charAt(0) +
    '.';

  return {
    id: id.toString(),
    uuid: 'adewpermpo12mmc',
    firstname: firstname,
    lastname: firstname,
    email: 'user@company.com',
    role: ROLE[Math.round(Math.random() * (ROLE.length - 1))],
    created_at: '12/12/12'
  };
}

/** Constants used to fill up our data base. */
const ROLE: string[] = [
  'Administrtor',
  'Employee',
  'Human Resources',
];
const NAMES: string[] = [
  'Maia',
  'Asher',
  'Olivia',
  'Atticus',
  'Amelia',
  'Jack',
  'Charlotte',
  'Theodore',
  'Isla',
  'Oliver',
  'Isabella',
  'Jasper',
  'Cora',
  'Levi',
  'Violet',
  'Arthur',
  'Mia',
  'Thomas',
  'Elizabeth',
];
