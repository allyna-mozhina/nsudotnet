using System.Web;

namespace Mozhina.Nsudotnet.Rss2Email.Mocks
{
    public class HttpResponseMock : HttpResponseBase
    {
        private HttpCookieCollection _cookies = new HttpCookieCollection();

        public HttpResponseMock()
        {

        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }


    }

}