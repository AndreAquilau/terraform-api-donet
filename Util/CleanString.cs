using System.Text.RegularExpressions;

namespace TerraformAPIDotnet.Util;

public class CleanString
{
    public string RemoveUnicodeANSI(string input)
    {
        // Define a expressão regular para remover os caracteres unicode e ANSI
        string unescaped = Regex.Unescape(input);
        Regex regex = new Regex(@"[^\x00-\x7F]|\u001b\[\d{1,2}m|[\x80-\xFF]");

        // Remove os caracteres unicode e ANSI da string de entrada
        string output = regex.Replace(unescaped, string.Empty).Trim();

        return output;
    }
}