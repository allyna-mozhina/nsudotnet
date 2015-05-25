using System.IO;
using System.Reflection;
using System.Web.WebPages;

namespace Mozhina.Nsudotnet.Rss2Email.Mocks
{
    public class VirtualPathFactoryMock : IVirtualPathFactory
    {
        public static FileInfo FileFromVirtualPath(string virtualPath)
        {
            if (virtualPath.StartsWith("~")) virtualPath = virtualPath.Substring(1);

            var a = Assembly.GetExecutingAssembly();
            string cb = a.CodeBase;
            if (cb.StartsWith("file:///"))
            {
                cb = cb.Substring("file:///".Length);                
            }

            FileInfo fi = new FileInfo(cb);
            var viewsDir = new DirectoryInfo(string.Format("{0}\\Views", fi.Directory.FullName));

            if (!viewsDir.Exists)
            {
                viewsDir = new DirectoryInfo(string.Format("{0}\\Views", fi.Directory.Parent.Parent.FullName));                
            }

            virtualPath = virtualPath.Replace("/", "\\");
            string fullName = string.Format("{0}\\{1}", viewsDir.FullName, virtualPath.Substring("\\Views\\".Length));

            return new FileInfo(fullName);
        }

        public bool Exists(string virtualPath)
        {
            var fi = FileFromVirtualPath(virtualPath);
            return fi.Exists;
        }

/*
        public const string TemplatesTypeTemplate = "Mozhina.Nsudotnet.Rss2Email.Views._Views_Mail_{0}_cshtml";
*/

        public object CreateInstance(string virtualPath)
        {
/*
            string master = string.Format(TemplatesTypeTemplate, "_Layout");
            var masterType = Assembly.GetExecutingAssembly().GetType(master);
            return Activator.CreateInstance(masterType);
*/
            return null;
        }
    }
}
