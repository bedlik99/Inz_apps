using Moq;
using ServerAPI.Repositories;
using ServerAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_WebAPI.Tests.Common
{
    public class TestBase : IDisposable
    {
        protected readonly ServerDBContext _context;
        protected readonly Mock<ServerDBContext> _contextMock;
        protected readonly EmailSettings _emailSettings;

        public TestBase()
        {
            _contextMock = ServerDBContextFactory.Create();
            _context = _contextMock.Object;
        }
        public void Dispose()
        {
            ServerDBContextFactory.Destroy(_context);
        }
    }
}
