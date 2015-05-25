using System.Web;
using Mozhina.Nsudotnet.Rss2Email.Mail.Mocks;

namespace Mozhina.Nsudotnet.Rss2Email.Mocks
{
    class HttpRequestMock : HttpRequestBase
    {
        private HttpCookieCollection _cookies = new HttpCookieCollection();

        private HttpBrowserCapabilitiesBase _bc = new HttpBrowserCapabilitiesMock();

        public override string MapPath(string virtualPath)
        {
           var fi = VirtualPathFactoryMock.FileFromVirtualPath(virtualPath);
           return fi.FullName;
        }

        public override bool IsLocal
        {
            get
            {
                return true;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }

        public override string UserAgent
        {
            get
            {
                return "";
            }
        }

        public override HttpBrowserCapabilitiesBase Browser
        {
            get
            {
                return _bc;
            }
        }
    }
}
