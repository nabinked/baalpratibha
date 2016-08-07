using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BaalPratibha.Logic
{
    public class ImageProcessing
    {
        private readonly IHostingEnvironment _environment;
        private readonly AppSettings _settings;

        public ImageProcessing(IOptions<AppSettings> options, IHostingEnvironment environment)
        {
            _environment = environment;
            _settings = options.Value;
        }

        public string GetImage(long userId)
        {
            var fullPath = _settings.NotFoundImagePath;

            var folder = _environment.WebRootPath + _settings.UserImagesFolder;
            var files = Directory.GetFiles(folder, userId + "-*", SearchOption.TopDirectoryOnly);
            if (files.Length > 0)
            {

                string fileName = Path.GetFileName(files[0]);
                fullPath = Path.Combine("/", _settings.UserImagesFolder, Path.GetFileName(fileName));
            }
            return fullPath.Replace("\\", "/");
        }

        public async Task<bool> AddImage(IFormFile file, long userId)
        {
            var pathForSaving = _environment.WebRootPath + _settings.UserImagesFolder;

            if (file.Length > 0)
            {
                var dateTimePrint = "-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
                var newfileName = userId + dateTimePrint + Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(Path.Combine(pathForSaving, newfileName), FileMode.Create))
                {

                    await file.CopyToAsync(fileStream);
                    var oldFiles =
                        Directory.GetFiles(pathForSaving, userId + "-*", SearchOption.TopDirectoryOnly);

                    foreach (var oldFile in oldFiles)
                    {
                        if (Path.GetFileName(oldFile) != newfileName)
                        {
                            File.Delete(oldFile);
                        };
                    }
                    return true;
                }
            }
            return false;

        }

        public const int ImageMinimumBytes = 512;

        public bool IsImage(IFormFile file)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (file.ContentType.ToLower() != "image/jpg" &&
                        file.ContentType.ToLower() != "image/jpeg" &&
                        file.ContentType.ToLower() != "image/pjpeg" &&
                        file.ContentType.ToLower() != "image/gif" &&
                        file.ContentType.ToLower() != "image/x-png" &&
                        file.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
                && Path.GetExtension(file.FileName).ToLower() != ".png"
                && Path.GetExtension(file.FileName).ToLower() != ".gif"
                && Path.GetExtension(file.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!file.OpenReadStream().CanRead)
                {
                    return false;
                }

                if (file.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                file.OpenReadStream().Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
