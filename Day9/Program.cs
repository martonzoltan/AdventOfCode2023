using Core;

var input = InputHelper.GetInput().ToList();

//Part1();
Part2();

void Part1()
{
    var sum = 0;
    foreach (var reading in input)
    {
        var readingList = reading.Split(" ").Select(int.Parse).ToList();
        var extrapolatedResults = GetExtrapolatedResults(readingList);
        for (var i = extrapolatedResults.Count - 2; i >= 0; i--)
        {
            var previousExtrapolationEnd = extrapolatedResults[i + 1].Last();
            var currentLasItem = extrapolatedResults[i].Last();
            var newReading = previousExtrapolationEnd + currentLasItem;
            extrapolatedResults[i].Add(newReading);
        }

        sum += extrapolatedResults[0].Last();
    }

    Console.WriteLine(sum);
}

void Part2()
{
    var sum = 0;
    foreach (var reading in input)
    {
        List<int> readingList = reading.Split(" ").Select(int.Parse).ToList();
        var extrapolatedResults = GetExtrapolatedResults(readingList);
        for (var i = extrapolatedResults.Count - 2; i >= 0; i--)
        {
            var previousExtrapolationEnd = extrapolatedResults[i + 1].First();
            var currentLasItem = extrapolatedResults[i].First();
            var newReading = currentLasItem - previousExtrapolationEnd;
            extrapolatedResults[i].Insert(0, newReading);
        }

        sum += extrapolatedResults[0].First();
    }

    Console.WriteLine(sum);
}

List<List<int>> GetExtrapolatedResults(List<int> readingList)
{
    var allZeroDifferencesFound = false;
    var extrapolatedResults = new List<List<int>> {readingList};
    var currentExtrapolation = 0;
    while (!allZeroDifferencesFound)
    {
        extrapolatedResults.Add([]);
        for (var i = 0; i < extrapolatedResults[currentExtrapolation].Count - 1; i++)
        {
            var extrapolatedValue = extrapolatedResults[currentExtrapolation][i + 1] -
                                    extrapolatedResults[currentExtrapolation][i];
            extrapolatedResults[currentExtrapolation + 1].Add(extrapolatedValue);
        }

        allZeroDifferencesFound = extrapolatedResults[currentExtrapolation + 1].All(number => number == 0);
        currentExtrapolation++;
    }

    return extrapolatedResults;
}