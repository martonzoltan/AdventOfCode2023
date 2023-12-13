using System.Text.RegularExpressions;
using Core;

var input = InputHelper.GetInput().ToList();
var count = 0;

// Part1();
Part2();

return;

void Part1()
{
    foreach (var line in input)
    {
        var catapults = line.Split(" ")[0];
        var positionList = line.Split(" ")[1].Split(",").Select(x => Convert.ToInt32(x)).ToList();
        BuildCombinations(catapults, positionList);
    }

    Console.WriteLine(count);
}

void Part2()
{
    foreach (var line in input)
    {
        Console.WriteLine(line);
        var catapults = line.Split(" ")[0];
        var positions = line.Split(" ")[1];
        var unfoldedCatapults = string.Empty;
        var unfoldedPositionsList = string.Empty;
        for (var i = 0; i < 5; i++)
        {
            unfoldedCatapults += '?' + catapults;
            unfoldedPositionsList += ',' + positions;
        }
        unfoldedCatapults = unfoldedCatapults[1..];
        unfoldedPositionsList = unfoldedPositionsList[1..];
        var positionList = unfoldedPositionsList.Split(",").Select(x => Convert.ToInt32(x)).ToList();
        BuildCombinations(unfoldedCatapults, positionList);
    }

    Console.WriteLine(count);
}

void BuildCombinations(string catapults, List<int> positionList)
{
    var questionMarkIndices = new List<int>();
    for (var i = 0; i < catapults.Length; i++)
    {
        if (catapults[i] == '?')
        {
            questionMarkIndices.Add(i);
        }
    }

    var totalElements = questionMarkIndices.Count;
    var maxBinary = (int) Math.Pow(2, totalElements);

    for (var i = 0; i < maxBinary; i++)
    {
        var binaryString = Convert.ToString(i, 2).PadLeft(totalElements, '0');
        var catapultArray = catapults.ToCharArray();

        for (var j = 0; j < binaryString.Length; j++)
        {
            if (binaryString[j] == '0')
            {
                catapultArray[questionMarkIndices[j]] = '.';
            }
            else
            {
                catapultArray[questionMarkIndices[j]] = '#';
            }
        }

        if (ValidateLine(catapultArray, positionList))
        {
            count++;
        }
    }
}

bool ValidateLine(char[] array, List<int> positionList)
{
    var simplifiedString = Regex.Replace(new string(array), @"\.{2,}", ".");
    var catapultParts = simplifiedString.Trim('.').Split(".");
    if (positionList.Count != catapultParts.Length) return false;
    for (var i = 0; i < catapultParts.Length; i++)
    {
        if (catapultParts[i].Length != positionList[i])
        {
            return false;
        }
    }

    return true;
}