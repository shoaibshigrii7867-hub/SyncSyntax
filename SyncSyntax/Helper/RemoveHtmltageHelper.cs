

using System.Text.RegularExpressions;

namespace SyncSyntax.Helper
{
    public static class RemoveHtmltageHelper
    { 
      public static string RemoveHtmlTags(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            // Regular expression to match HTML tags
            string pattern = "<.*?>";
            return Regex.Replace(input, pattern, string.Empty);
        }
    }
}
