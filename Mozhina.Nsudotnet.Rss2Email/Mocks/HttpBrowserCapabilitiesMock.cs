using System.Web;

namespace Mozhina.Nsudotnet.Rss2Email.Mocks
{
    public class HttpBrowserCapabilitiesMock : HttpBrowserCapabilitiesBase
    {
        public override bool IsMobileDevice
        {
            get
            {
                return false;
            }
        }
    }
}