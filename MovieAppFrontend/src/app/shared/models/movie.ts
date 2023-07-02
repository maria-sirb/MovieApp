import { Genre } from "./genre";
import { Actor } from "./actor";
import { Role } from "./role";
import { Director } from "./director";
export interface Movie{
    movieId : number;
    title : string;
    imdbRating: number;
    runTime : number;
    releaseYear : number;
    summary: string;
    storyLine: string;
    oscarWins: number;
    oscarNominations: number;
    poster : string;
    director : Director;
    movieGenres : Genre[];
    movieActors: Actor[];
    roles : Map<number, string>;
}