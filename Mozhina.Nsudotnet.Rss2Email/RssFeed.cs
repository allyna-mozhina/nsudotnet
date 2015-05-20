using System;
using System.Xml.Serialization;

namespace Mozhina.Nsudotnet.Rss2Email
{
    [Serializable]
    [XmlRoot("rss")]
    public class RssFeed
    {
        [XmlElement("channel")]
        public RssChannel Channel { get; set; } 
         
    }
}