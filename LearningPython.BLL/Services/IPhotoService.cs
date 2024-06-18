using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace LearningPython.BLL.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<ImageUploadResult> AddStandartedPhotoAsync(IFormFile file);
        Task<ImageUploadResult> AddPhotoWithCustomTransformationAsync(IFormFile file, int h, int w);
        Task<RawUploadResult> AddJsAsync(IFormFile file);
        Task<RawUploadResult> AddRawAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}