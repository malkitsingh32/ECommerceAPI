namespace Application.Common.Helpers
{
    public static class Utils
    {
        // String Extension
        public static string ToStringEx(this object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString();
        }
        public static string TrimLastCharacter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return str.TrimEnd(str[str.Length - 1]);
            }
        }
    }
}
