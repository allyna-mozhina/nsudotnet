using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Mozhina.Nsudotnet.Rss2Email
{
    [Serializable]
    public class RssChannel
    {
        [XmlElement("item")]
        public List<RssItem> Items { get; set; }
    }
}