import { User } from "./user";

export interface Review{
    reviewId : number,
    rating : number,
    title? : string,
    text? : string,
    date : Date,
    user? : User
}