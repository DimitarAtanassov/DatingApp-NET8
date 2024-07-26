using CloudinaryDotNet.Actions;

namespace API.Interfaces;

public interface IPhotoService
{
    // Return a task of type ImagineUploadResut, 
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoAsync(string publicid); // We need the publicid to be able to delete the photo from cloudinary
}
