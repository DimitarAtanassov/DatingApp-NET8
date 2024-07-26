using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    
    // We are inject IOptions because this is how we get access to our cloudinary settings
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);  // This way we can access cloudinary functionallity with _cloudinary throughout the class.
    }
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        // Check if we have a file
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                
                // Formating photo
                Transformation = new Transformation()
                    .Height(500).Width(500).Crop("fill").Gravity("face"),
                
                // Folder to store the photo
                Folder = "da-net8"

            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult; 
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicid)
    {
        var deleteParams = new DeletionParams(publicid);

        return await _cloudinary.DestroyAsync(deleteParams);


    }
}