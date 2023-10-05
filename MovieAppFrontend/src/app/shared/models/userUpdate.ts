export interface UserUpdate {
    username : string,
    confirmPassword : string,
    deleteCurrentImage : boolean,
    imageFile? : File
}