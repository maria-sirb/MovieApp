import { Movie } from "./movie"
export interface Director{
 
    directorId : number,
    name : string,
    photo : string,
    born : Date,
    oscarWins : number,
    oscarNominations : number,
    movies : Movie[]

}