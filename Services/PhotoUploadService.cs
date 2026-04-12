using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace MahlukHidup.Backend.Services;

public class PhotoUploadService : IPhotoUploadService
{
    private readonly IHostEnvironment _environment;
    private const string PlantVaultFolder = "PlantVault";
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    public PhotoUploadService(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadDiseasePhotoAsync(IFormFile file, Guid companyId, string companyName)
    {
        // Validate file
        ValidateFile(file);

        // Create dynamic folder path: PlantVault/{CompanyName}/{yyyy-MM-dd}/
        var dateFolder = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var safeCompanyName = MakeSafeFolderName(companyName);
        var relativePath = Path.Combine(PlantVaultFolder, safeCompanyName, dateFolder);
        
        var wwwrootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        var fullDirectoryPath = Path.Combine(wwwrootPath, relativePath);

        // Create directory if not exists
        Directory.CreateDirectory(fullDirectoryPath);

        // Generate unique filename
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"disease_{companyId:N}_{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(fullDirectoryPath, fileName);

        // Save file
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative URL path (forward slashes for URL)
        var urlPath = Path.Combine(relativePath, fileName).Replace("\\", "/");
        return urlPath;
    }

    public async Task<bool> DeletePhotoAsync(string photoUrl)
    {
        try
        {
            var wwwrootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            var fullPath = Path.Combine(wwwrootPath, photoUrl.Replace("/", Path.DirectorySeparatorChar.ToString()));
            
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                
                // Clean up empty folders
                var directory = Path.GetDirectoryName(fullPath);
                if (Directory.Exists(directory) && Directory.GetFiles(directory).Length == 0)
                {
                    Directory.Delete(directory);
                }
                
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }

    private void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File tidak boleh kosong.");
        }

        if (file.Length > MaxFileSize)
        {
            throw new ArgumentException("Ukuran file melebihi batas maksimal (10 MB).");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new ArgumentException($"Tipe file tidak didukung. Tipe yang diizinkan: {string.Join(", ", AllowedExtensions)}");
        }
    }

    private string MakeSafeFolderName(string name)
    {
        // Remove special characters and replace spaces with underscores
        var safeName = name.Trim();
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            safeName = safeName.Replace(c.ToString(), "");
        }
        safeName = safeName.Replace(" ", "_");
        return safeName;
    }
}
