import { Movie } from "./movie"
export interface Genre{
    genreId : number,
    name : string,
    movieGenre : Movie[]
}