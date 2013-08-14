namespace ScriptCs.Net
{
    using System.Linq;

    public static class StringExtensions
    {
        public static byte[] AsBytes(this string s)
        {
            return System.Text.Encoding.Default.GetBytes(s);
        }

        public static byte[] GetIpAddressBytes(this string address)
        {
            return address.Split('.').Select(byte.Parse).ToArray();
        }
    }
}
