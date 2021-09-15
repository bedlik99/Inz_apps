using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Models
{
	public class Student
	{	
		[Key]
		[Required]
		public int Id { get; set; }

		[Required]
		[MaxLength(6)]
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
