using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.DTOs
{
	public class StudentCreateDto
	{
		// need to think what input will be given by student
		public string IndexNum { get; set; }

		public bool IsCheater { get; set; }

		public string AccessCode { get; set; }

		public int Points { get; set; }

		public int Mark { get; set; }
		//public DateTime FinishDate { get; set; }

		public string StartDate { get; set; }

		public string FinishDate { get; set; }
	}
}
