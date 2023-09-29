import { Movie } from "./movie";
export interface Actor{
 
    actorId : number,
    name : string,
    photo : string,
    born : Date,
    oscarWins : number,
    oscarNominations : number
    movieActors : Movie[];
}