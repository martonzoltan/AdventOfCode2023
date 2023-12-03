using System.Text.RegularExpressions;
using Core;

string[] numbersInWords = {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
var listOfInput = InputHelper.GetInput();

var totalSum = listOfInput.Sum(Part2);

Console.WriteLine(totalSum);
return;

int Part1(string inputString)
{
    var numbersInString = inputString.Where(char.IsDigit).ToArray();
    var firstNumber = numbersInString.Length != 0 ? numbersInString.First() : '0';
    var lastNumber = numbersInString.Length != 0 ? numbersInString.Last() : '0';

    var twoDigitString = new string(new[] {firstNumber, lastNumber});
    return int.Parse(twoDigitString);
}

int Part2(string input)
{
    var sortedNumbersInWords = numbersInWords.OrderByDescending(s => s.Length);
    var pattern = @"(\d|" + string.Join("|", sortedNumbersInWords) + ")";
    var firstOccurrence = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)
        .OfType<Match>()
        .Select(m => m.Value)
        .FirstOrDefault();

    var lastOccurrence = Regex.Matches(input, pattern, RegexOptions.RightToLeft)
        .OfType<Match>()
        .Select(m => m.Value)
        .FirstOrDefault();

    int firstNumber = 0;
    if (firstOccurrence != null)
    {
        if (int.TryParse(firstOccurrence, out var number))
        {
            firstNumber = number;
        }
        else
        {
            firstNumber = Array.IndexOf(numbersInWords, firstOccurrence) + 1;
        }
    }

    int lastNumber = 0;
    if (lastOccurrence != null)
    {
        if (int.TryParse(lastOccurrence, out var number))
        {
            lastNumber = number;
        }
        else
        {
            lastNumber = Array.IndexOf(numbersInWords, lastOccurrence) + 1;
        }
    }

    var twoDigitString = int.Parse($"{firstNumber}{lastNumber}");
    return twoDigitString;
}