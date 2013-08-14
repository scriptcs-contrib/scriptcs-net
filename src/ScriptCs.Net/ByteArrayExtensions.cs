namespace ScriptCs.Net
{
    public static class ByteArrayExtensions
    {
        public static string AsString(this byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes, 0, bytes.Length);
        }
    }
}
