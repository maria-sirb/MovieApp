import { Movie } from "../models/movie";
import { Actor } from "../models/actor";
import { Director } from "../models/director";
import { CastMember } from "../models/castMember";
import { User } from "../models/user";

export class DefaultPhoto{

  addDefaultPhotoPerson(event : Event)
  {
    (event.target as HTMLSourceElement).src = "assets/person.jpg";
  }
  addDefaultPhoto(event : Event)
  {
    (event.target as HTMLSourceElement).src = "assets/black-image.jpg";
  }
}
  