using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Collections.Generic;

namespace Api_ELearning.Services
{
    public class FileService : IFileService
    {
        private IHostingEnvironment _hostingEnv;
        public FileService(IHostingEnvironment env)
        {
            _hostingEnv = env;
        }

        public List<string> UploadFiles(IFormFileCollection files)
        {
            List<string> Files = new List<string>();
            foreach (var file in files)
            {
                var nowDate = DateTime.Now.ToString("dd_MM_yyyy");
                var nowTime = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
                var filename = nowTime
                    + ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                var urls = "/FilesUploaded/" + nowDate;
                Files.Add(urls+",");
                filename = _hostingEnv.WebRootPath + $@"\FilesUploaded\{filename}";
                using (FileStream fs = File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            return Files;
        }
        public string UploadFile(IFormFile file)
        {
            var nowDate = DateTime.Now.ToString("dd_MM_yyyy");
            var nowTime = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
            var filename = nowTime
                + ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName
                            .Trim('"');
            var urls = "/FilesUploaded/" + filename;
            filename = _hostingEnv.WebRootPath + $@"\FilesUploaded\{filename}";
            using (FileStream fs = System.IO.File.Create(filename))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            return urls;
        }
    }
}
