using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Api_ELearning.Services
{
    public interface IFileService
    {
        List<string> UploadFiles(IFormFileCollection files);
        string UploadFile(IFormFile file);
    }
}
