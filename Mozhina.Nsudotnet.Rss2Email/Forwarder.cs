using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Timers;
using System.Xml;
using System.Xml.Serialization;
using Mozhina.Nsudotnet.Rss2Email.Mail;
using Timer = System.Timers.Timer;

namespace Mozhina.Nsudotnet.Rss2Email
{
    public class Forwarder
    {
        public Uri FeedUri { get; set; }

        public MailAddress Sender { get; set; } 

        public MailAddress Recipient { get; set; }

        private SmtpClient _smtpClient;
        
        private XmlSerializer _feedSerializer;

        private Timer _timer;

        private DateTime _lastCheck = DateTime.MinValue;
        
        private DateTime _lastSent = DateTime.MinValue;

        private Mutex _smtpGuard;

        public Forwarder(string sender, string recipient, string feedUri)
        {
            Sender = new MailAddress(sender);
            Recipient = new MailAddress(recipient);
            FeedUri = new Uri(feedUri);

            _smtpClient = new SmtpClient()
            {
                Host = "smtp.yandex.ru",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(Sender.User, "nsudotnet12201"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 15000
            };

            _feedSerializer = new XmlSerializer(typeof (RssFeed));

            _timer = new Timer(600); //check every 10 minutes
            _timer.Elapsed += RetrieveFeedItems;

            _smtpGuard = new Mutex();

            InitDates();

            AppDomain.CurrentDomain.ProcessExit += SaveDates;
        }

        private void InitDates()
        {
            _lastCheck = Properties.Rss2Email.Default.lastCheck;
            _lastSent = Properties.Rss2Email.Default.lastSent;
        }

        public void Enable(bool enabled)
        {
            _timer.Enabled = enabled;
        }

        public void RetrieveFeedItems(object source, ElapsedEventArgs e)
        {
            var request = (HttpWebRequest) WebRequest.Create(FeedUri);
            request.IfModifiedSince = _lastCheck;

            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    _lastCheck = response.LastModified;

                    using (var input = response.GetResponseStream())
                    {
                        if (input == null)
                            return;

                        using (var feedReader = XmlReader.Create(input))
                        {
                            RssFeed feed = (RssFeed) _feedSerializer.Deserialize(feedReader);
                            List<RssItem> items = feed.Channel.Items;

                            items.Sort();

                            if (items.Last().Date <= _lastSent)
                                return;

                            _smtpGuard.WaitOne();

                            if (items.Last().Date <= _lastSent)
                                return;

                            foreach (var item in items)
                            {
                                if (item.Date > _lastSent)
                                {
                                    SendItem(item, Recipient);

                                    _lastSent = item.Date;
                                }
                            }

                            _smtpGuard.ReleaseMutex();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                var response = (HttpWebResponse) ex.Response;
                if (response.StatusCode != HttpStatusCode.NotModified)
                    throw;
            }
            catch (SmtpException ex)
            {

            }
        }

        private void SendItem(RssItem item, MailAddress mailto)
        {
            string body = "<a href='" + item.Link + "'>" + item.Title + "</a><br>" + item.Description;

            using (var htmlBody = AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html")))
            {
                using (var message = new MailMessage
                {
                    From = new MailAddress("dotnetfwd@yandex.ru"),
                    To = {mailto},
                    Subject = item.Title,
                    Body = item.Link,
                    AlternateViews = {htmlBody}
                })
                {
                    _smtpClient.Send(message);
                }
            }
        }

        private void SaveDates(object sender, EventArgs e)
        {
            Properties.Rss2Email.Default.lastCheck = _lastCheck;
            Properties.Rss2Email.Default.lastSent = _lastSent;

            Properties.Rss2Email.Default.Save();
        }

        public static void Main()
        {
            Forwarder fwd = new Forwarder("dotnetfwd@yandex.ru", "dotnetfwd@yandex.ru", "http://fit.nsu.ru/component/ninjarsssyndicator/?feed_id=1&format=raw");
            
            fwd.Enable(true);

            Console.ReadKey();
        }
    }
}