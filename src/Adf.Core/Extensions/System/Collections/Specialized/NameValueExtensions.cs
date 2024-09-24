namespace System.Collections.Specialized
{
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="coll">
        /// The coll.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultVal">
        /// The default val.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T GetValue<T>(this NameValueCollection coll, string key, T defaultVal = default(T))
        {
            return coll[key].To(defaultVal);
        }
    }
}
