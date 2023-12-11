using System.Drawing;
using Core;

var map = InputHelper.GetInput().ToList();
List<Point> points = [];
//Part1();
Part2();

return;

void Part1()
{
    const int emptyDistance = 2;
    var weights = SetDistance(emptyDistance);
    var sum = GetDistances(weights, emptyDistance);
    Console.WriteLine(sum);
}

void Part2()
{
    const int emptyDistance = 1_000_000;
    var weights = SetDistance(emptyDistance);
    var sum = GetDistances(weights, emptyDistance);
    Console.WriteLine(sum);
}

long GetDistances(int[][] weights, int emptyDistance)
{
    var sum = 0L;
    foreach (var startingPoint in points)
    {
        for (var i = points.IndexOf(startingPoint); i < points.Count; i++)
        {
            var endPoint = points[i];
            long result = Math.Abs(startingPoint.X - endPoint.X) + Math.Abs(startingPoint.Y - endPoint.Y);

            var minX = Math.Min(startingPoint.X, endPoint.X);
            var maxX = Math.Max(startingPoint.X, endPoint.X);
            var additionalX = weights.Select(row => row.Skip(minX).Take(maxX - minX + 1))
                .FirstOrDefault()!.Count(x => x != 1);
            result += (emptyDistance - 1) * additionalX;

            var minY = Math.Min(startingPoint.Y, endPoint.Y);
            var maxY = Math.Max(startingPoint.Y, endPoint.Y);
            var additionalY = weights.Skip(minY).Take(maxY - minY + 1)
                .Count(row => !row.Contains(1));
            result += (emptyDistance - 1) * additionalY;
            sum += result;
        }
    }

    return sum;
}


int[][] SetDistance(int distance)
{
    var rows = map.Count;
    var columns = map[0].Length;
    var weightedMap = new int[rows][];

    for (var i = 0; i < rows; i++)
    {
        weightedMap[i] = new int[columns];
        for (var j = 0; j < columns; j++)
        {
            if (map[i][j] == '#')
            {
                weightedMap[i][j] = 1;
                points.Add(new Point(j, i));
            }
            else if (!map[i].Contains('#') || !map.Any(row => row[j] == '#'))
            {
                weightedMap[i][j] = distance;
            }
            else
            {
                weightedMap[i][j] = 1;
            }
        }
    }

    return weightedMap;
}