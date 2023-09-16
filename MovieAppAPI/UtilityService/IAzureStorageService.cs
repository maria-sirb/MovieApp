namespace MovieAppAPI.UtilityService
{
    public interface IAzureStorageService
    {
        string UploadImage(IFormFile imageFile);
        void DeleteImage(string imageName);
        string GetFileUrl(string imageName);
    }
}
