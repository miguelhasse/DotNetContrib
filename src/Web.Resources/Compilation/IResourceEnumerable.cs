using System.Collections;
using System.Globalization;

namespace Hasseware.Web.Compilation
{
    /// <summary>
    /// Exposes an enumerator for a given culture, which supports a simple iteration over a non-generic collection.
    /// </summary>
    public interface IResourceEnumerable
    {
        IDictionaryEnumerator GetEnumerator(CultureInfo culture);
    }
}
