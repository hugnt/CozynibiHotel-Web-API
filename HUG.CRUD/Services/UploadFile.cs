using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUG.CRUD.Services
{
    public class UploadFile
    {
        private readonly string _filePath;
        public UploadFile(string filePath)
        {
            _filePath = filePath;
        }
        public async Task<ResponseModel> UploadImage(List<IFormFile> files, string subPath)
        {
            var imagePath = Path.Combine(_filePath, subPath);

            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = file.FileName;
                        var filePath = Path.Combine(imagePath, fileName);

                        if (System.IO.File.Exists(filePath)) continue;

                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                return new ResponseModel(500, "Somthing went wrong when uploading files"+e);
            }
            
            return new ResponseModel(200, "Uploaded successfully");
        }
    }
}
