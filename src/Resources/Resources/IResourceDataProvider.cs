using System.Collections.Generic;
using System.Globalization;

namespace Hasseware.Resources
{
    public interface IResourceDataProvider
    {
        /// <summary>
        /// Returns a fully normalized list of resources that contains the most specific
        /// locale version for the culture provided.
        ///                 
        /// This means that this routine walks the resource hierarchy and returns
        /// the most specific value in this order: de-ch, de, invariant.
        /// </summary>
        IDictionary<string, object> Get(string resourceSet, CultureInfo culture);

        /// <summary>
        /// Returns an object from the Resources. Attempts to convert the object to its
        /// original type. Use this for any non-string types. Useful for binary resources
        /// like images, icons etc.
        /// 
        /// While this method can be used with strings, GetResourceString()
        /// is much more efficient.
        /// </summary>
        object Get(string key, string resourceSet, CultureInfo culture);

        /// <summary>
        /// Adds or updates a resources in a collection.
        /// </summary>
        void Save(IDictionary<string, object> resources, string resourceSet, CultureInfo culture);

        /// <summary>
        /// Adds or updates a resource if it exists, if it doesn't one is created.
        /// </summary>
        void Save(string key, object value, string resourceSet, CultureInfo culture);

        /// <summary>
        /// Deletes a specific resource if a key is specified, otherwise the whole ResourceSet is deleted.
        /// If an empty culture is passed the entire group is removed (ie. all locales).
        /// </summary>
        void Delete(string key, string resourceSet, CultureInfo culture);
    }
}
