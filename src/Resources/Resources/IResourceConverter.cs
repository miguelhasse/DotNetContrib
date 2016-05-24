namespace Hasseware.Resources
{
    public interface IResourceConverter
    {
        object ConvertFromStore(object value, string mimeType);

        object ConvertToStore(object value, out string mimeType);
    }
}
