using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.DTOs
{
	public class StudentReadDto
	{
		public int Id { get; set; }
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
