import { Movie } from "../models/movie";
import { Actor } from "../models/actor";
import { Director } from "../models/director";

export class DefaultPhoto{

addDefaultPhotoMovie(movie : Movie)
  {
    movie.poster = "assets/black-image.jpg";
   }
  addDefaultPhotoActor(actor : Actor)
  {
    actor.photo = "assets/person.jpg";
   }
   addDefaultPhotoDirector(director : Director)
   {
     director.photo = "assets/person.jpg";
    }
}
  