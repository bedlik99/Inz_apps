using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.DTOs
{
    public class RegisteredUserDTO
    {
        public RegisteredUserDTO() { }
        public RegisteredUserDTO(string uniqueCode, string indexNr)
        {
            UniqueCode = uniqueCode;
            IndexNr = indexNr;
        }
        public string UniqueCode { get; set; }
        public string IndexNr { get; set; }


        public override string ToString()
        {
            return "{" + "uniqueCode='" + UniqueCode + '\'' +
                 ",indexNum='" + IndexNr + '\'' + '}';
        }
    }
}


