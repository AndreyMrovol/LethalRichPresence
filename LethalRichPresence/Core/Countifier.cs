using System.Collections.Generic;
namespace LethalRichPresence;

public class Countifiers
{
  public static Dictionary<int, string> countifiers = new()
    {
        {1, "st"},
        {2, "nd"},
        {3, "rd"},
        {4, "th"},
        {5, "th"},
        {6, "th"},
        {7, "th"},
        {8, "th"},
        {9, "th"},
        {10, "th"},
        {11, "th"},
        {12, "th"},
        {13, "th"},
        {14, "th"},
        {15, "th"},
        {16, "th"},
        {17, "th"},
        {18, "th"},
        {19, "th"},
        {20, "th"},
    };

  public static string Countifier(int number)
  {
    int lastDigit = number % 10;
    if (lastDigit > 3 || lastDigit == 0)
    {
      return number + "th";
    }
    else
    {
      return number + countifiers[lastDigit];
    }
  }

}