using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Repositories
{
	public class FileForm
	{
		public FileForm() { }
		public IFormFile UploadFile { get; set; }
	}
}
