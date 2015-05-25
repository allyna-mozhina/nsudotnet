using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Mozhina.Nsudotnet.Rss2Email.Mocks
{
    public class HttpContextMock : HttpContextBase
    {
        private HttpRequestMock _request;
        private IDictionary _items;
        private HttpResponseBase _response ;
        public HttpContextMock()
        {
            _request = new HttpRequestMock();
            _items = new Dictionary<object, object>();
            _response = new HttpResponseMock();
        }

        public override IDictionary Items
        {
            get { return _items; }
        }
        
        public override HttpRequestBase Request
        {
            get { return _request; }
        }

        public override HttpResponseBase Response
        {
            get { return _response; }
        }

    }

    public class HttpResponseMock : HttpResponseBase
    {
        public HttpResponseMock()
        {
            
        }

        private HttpCookieCollection _cookies = new HttpCookieCollection();


        public override HttpCookieCollection Cookies
        {
            get { return _cookies; }
        }

        
    }
}
