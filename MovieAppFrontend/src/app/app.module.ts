import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { MoviesModule } from "./movies/movies.module";
import { ActorsModule } from './actors/actors.module';
import { DirectorsModule } from './directors/directors.module';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { SearchComponent } from './search/search.component';
import { AddComponent } from './add/add.component';
import { FormsModule } from '@angular/forms';
import { AllFormsModule } from './forms/all-forms.module';
import { FooterComponent } from './footer/footer.component';
import { TokenInterceptor } from './shared/interceptors/token.interceptor';


@NgModule({
    declarations: [
        AppComponent,
        MenuComponent,
        SearchComponent,
        AddComponent,
        FooterComponent
       
    ],
    providers: [{
        provide : HTTP_INTERCEPTORS,
        useClass : TokenInterceptor,
        multi : true
    }],
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
