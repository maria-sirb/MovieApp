import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { MoviesModule } from "./movies/movies.module";
import { ActorsModule } from './actors/actors.module';
import { DirectorsModule } from './directors/directors.module';
import {HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { SearchComponent } from './search/search.component';
import { AddComponent } from './add/add.component';
import { FormsModule } from '@angular/forms';
import { AllFormsModule } from './forms/all-forms.module';


@NgModule({
    declarations: [
        AppComponent,
        MenuComponent,
        SearchComponent,
        AddComponent
       
    ],
    providers: [],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        MoviesModule,
        ActorsModule,
        DirectorsModule,
        HttpClientModule,
        AppRoutingModule,
        FormsModule,
        AllFormsModule

    ]
})
export class AppModule { }
