using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MahlukHidup.Backend.Services;

public interface IPhotoUploadService
{
    Task<string> UploadDiseasePhotoAsync(IFormFile file, Guid companyId, string companyName);
    Task<bool> DeletePhotoAsync(string photoUrl);
}
