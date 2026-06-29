using System.Text;

namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {

        if (string.IsNullOrWhiteSpace(input))
        return false;

        string LowerInput = input.ToLower();
        StringBuilder sb = new StringBuilder(LowerInput.Length);

        foreach (char ch in LowerInput)
        {
            if (!Char.IsPunctuation(ch) && !Char.IsWhiteSpace(ch))
            {
                sb.Append(ch);
            }
        }

        string FinalInput = sb.ToString();
        char [] chars = FinalInput.ToArray();
        Array.Reverse(chars);
        string ReversedInput = new string(chars);

        if (FinalInput != ReversedInput)
        {
            return false;
        }
        return true;
        
    }
}
