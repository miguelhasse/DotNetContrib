using System.IO;
using System.Web.Hosting;

namespace System.ServiceModel.Composition.Hosting
{
    /// <summary>
    /// Defines a virtual file for a dynamic service.
    /// </summary>
    internal sealed class ComposedServiceVirtualFile<T> : VirtualFile where T : IHostedService
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="ComposedServiceVirtualFile"/>.
        /// </summary>
        /// <param name="virtualFile">The virtual file path.</param>
        public ComposedServiceVirtualFile(string virtualFile)
            : base(virtualFile)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the name of the service from the virtual file path.
        /// </summary>
        /// <param name="virtualFile">The virtual file path.</param>
        /// <returns>The service name.</returns>
        private static string GetName(string virtualFile)
        {
            string name = virtualFile.Substring(virtualFile.LastIndexOf("/") + 1);
            return name.Substring(0, name.LastIndexOf("."));
        }

        /// <summary>
        /// Opens the stream containing the virtual file content.
        /// </summary>
        /// <returns>The stream.</returns>
        public override Stream Open()
        {
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(string.Format("<%@ ServiceHost Language=\"C#\" Debug=\"true\" Service=\"{0}\" Factory=\"{1}\" %>",
                    GetName(VirtualPath), typeof(ComposedServiceHostFactory<T>).FullName));
            }
            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}