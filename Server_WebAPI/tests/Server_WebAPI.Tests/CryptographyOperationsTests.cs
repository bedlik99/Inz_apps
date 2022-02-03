using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Server_WebAPI.Tests
{
    public class CryptographyTestingTests
    {
        public CryptographyTestingTests()
        {

        }

        [Fact]
        public void EncryptForTests_ForGivenString_CompleteOperation()
        {
            //arrange

            var input = "{\"email\":\"01143845@pw.edu.pl\",\"uniqueCode\":\"i8`ugD4=\"}";

            //act
            var result = CryptographyTesting.Program.EncryptionForTests(input);

            //assert
            Assert.Equal(input, result);
        }

    }
}
