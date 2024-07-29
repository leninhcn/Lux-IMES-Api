using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.Quality
{
    public class UploadFileDto
    {
        public IFormFile? File { get; set; }
        public int Id { get; set; }
    }
}
