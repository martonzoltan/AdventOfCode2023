using System.Text;
using Core;

var input = InputHelper.GetMatrixInput();

//Part1();
Part2();

void Part1()
{
    for (var i = 0; i < input.GetLength(0); i++)
    {
        for (var j = 0; j < input.GetLength(1); j++)
        {
            if (input[i, j] == 'O')
            {
                input[i, j] = '.';
                FindNewPosition(i, j);
            }
        }
    }

    Console.WriteLine(GetTotalWeight());
}

void Part2()
{
    Dictionary<int, (int cycle, int weight)> previousMaps = new();
    var repeatsAt = -1;
    var firstRepeatIndex = -1;
    var patternLength = -1;
    const int totalCycle = 1_000_000_000;
    for (var cycle = 0; cycle < totalCycle; cycle++)
    {
        for (var tilting = 0; tilting < 4; tilting++)
        {
            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                {
                    if (input[i, j] == 'O')
                    {
                        input[i, j] = '.';
                        FindNewPosition(i, j);
                    }
                }
            }

            RotateMatrix90Degrees();
        }

        var mapHash = GetUniqueHash();
        if (!previousMaps.TryGetValue(mapHash, out (int cycle, int weight) previousCycle))
        {
            previousMaps.Add(mapHash, (cycle, GetTotalWeight()));
        }
        else
        {
            if (repeatsAt == -1)
            {
                repeatsAt = cycle;
                firstRepeatIndex = previousCycle.cycle;
            }
            else if (previousCycle.cycle == firstRepeatIndex)
            {
                patternLength = cycle - repeatsAt;
                break;
            }
        }
    }

    var iteration = firstRepeatIndex + (totalCycle - repeatsAt) % patternLength;
    var finalWeight = previousMaps.FirstOrDefault(x => x.Value.cycle == iteration - 1).Value.weight;

    Console.WriteLine(finalWeight);
}

void FindNewPosition(int row, int column)
{
    var movementBlocked = false;
    for (var i = row; i >= 0; i--)
    {
        if (input[i, column] == '#' || input[i, column] == 'O')
        {
            input[i + 1, column] = 'O';
            movementBlocked = true;
            break;
        }
    }

    if (!movementBlocked)
    {
        input[0, column] = 'O';
    }
}

void RotateMatrix90Degrees()
{
    var rowCount = input.GetLength(0);
    var columnCount = input.GetLength(1);
    var rotatedMatrix = new char[columnCount, rowCount];

    for (var row = 0; row < rowCount; row++)
    {
        for (var column = 0; column < columnCount; column++)
        {
            rotatedMatrix[column, (rowCount - 1) - row] = input[row, column];
        }
    }

    input = rotatedMatrix;
}

int GetUniqueHash()
{
    var sb = new StringBuilder();
    for (var i = 0; i < input.GetLength(0); i++)
    {
        for (var j = 0; j < input.GetLength(1); j++)
        {
            sb.Append(input[i, j]);
        }
    }

    return sb.ToString().GetHashCode();
}

int GetTotalWeight()
{
    var weight = 0;
    for (var i = 0; i < input.GetLength(0); i++)
    {
        for (var j = 0; j < input.GetLength(1); j++)
        {
            if (input[i, j] == 'O')
            {
                weight += input.GetLength(1) - i;
            }
        }
    }

    return weight;
}