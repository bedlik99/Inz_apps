using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Settings
{
	public class CryptographySettings
	{
		public byte[] Key { get; set; }
		public byte[] IV { get; set; }
		public string WritableChars { get; set; }
	}
}
