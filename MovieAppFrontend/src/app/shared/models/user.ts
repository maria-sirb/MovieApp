export interface User{
    userId : number,
    username : string,
    email? : string,
    role? : string,
    password? : string,
    token? : string,
    imageName? : string,
    imageSource? : string,
    imageFile? : File
}