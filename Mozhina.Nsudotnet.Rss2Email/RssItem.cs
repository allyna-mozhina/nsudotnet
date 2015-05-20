using System;
using System.Xml.Serialization;

namespace Mozhina.Nsudotnet.Rss2Email
{
    [Serializable]
    public class RssItem : IComparable<RssItem>
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("pubDate")]
        public string PubDate { get; set; }

        [XmlIgnore]
        public DateTime Date
        {
            get
            {
                DateTime date;
                DateTime.TryParse(PubDate, out date);
                return date;
            }
        }

        public int CompareTo(RssItem other)
        {
            return Date.CompareTo(other.Date);
        }
    }

}