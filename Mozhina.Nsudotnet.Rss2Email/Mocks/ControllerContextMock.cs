using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mozhina.Nsudotnet.Rss2Email.Mocks
{
    public class ControllerContextMock : ControllerContext
    {
        private readonly HttpContextBase _httpContextBase;

        public ControllerContextMock(HttpContextBase httpContextBase)
        {
            _httpContextBase = httpContextBase;
        }

        public override HttpContextBase HttpContext { get { return _httpContextBase; }}
    }
}
