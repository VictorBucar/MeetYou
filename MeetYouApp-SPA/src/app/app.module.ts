import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import {FormsModule} from '@angular/forms';
//  import { FullCalendarModule } from '@fullcalendar/angular'; // for FullCalendar

import { AppComponent } from './app.component';
import { ValueComponent } from './Value/Value.component';

@NgModule({
   declarations: [
      AppComponent,
      ValueComponent,
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      // FullCalendarModule
   ],
   providers: [
      // AuthService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
