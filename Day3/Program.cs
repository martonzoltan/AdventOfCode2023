using System.Text;
using Core;

var matrix = InputHelper.GetMatrixInput();
// Part1();
Part2();

void Part1()
{
    var total = 0;
    for (var i = 0; i < matrix.GetLength(0); i++)
    {
        var numberStr = "";
        var partOfSchematic = false;
        for (var j = 0; j < matrix.GetLength(1); j++)
        {
            if (char.IsDigit(matrix[i, j]))
            {
                numberStr += matrix[i, j].ToString();
                if (!partOfSchematic)
                {
                    partOfSchematic = IsAdjacentSymbol(i, j);
                }
            }
            else
            {
                if (partOfSchematic)
                {
                    total += Convert.ToInt32(numberStr);
                }

                numberStr = "";
                partOfSchematic = false;
            }
        }

        if (partOfSchematic)
        {
            total += Convert.ToInt32(numberStr);
        }
    }

    Console.WriteLine(total);

    bool IsAdjacentSymbol(int i, int j)
    {
        var isSymbol = false;
        // Represents direction (up, upright, right, downright, down, downleft, left, upleft)
        int[] dRow = {-1, -1, 0, 1, 1, 1, 0, -1};
        int[] dColumn = {0, 1, 1, 1, 0, -1, -1, -1};
        for (var k = 0; k < 8; k++)
        {
            var adjRow = i + dRow[k];
            var adjCol = j + dColumn[k];

            if (adjRow >= 0 && adjRow < matrix.GetLength(0) && adjCol >= 0 && adjCol < matrix.GetLength(1))
            {
                if (matrix[adjRow, adjCol] != '.' && !char.IsLetterOrDigit(matrix[adjRow, adjCol]))
                {
                    isSymbol = true;
                }
            }
        }

        return isSymbol;
    }
}

void Part2()
{
    var total = 0;
    for (var i = 0; i < matrix.GetLength(0); i++)
    {
        for (var j = 0; j < matrix.GetLength(1); j++)
        {
            if (matrix[i, j] == '*')
            {
                total += GetGearScore(i, j);
            }
        }
    }

    Console.WriteLine(total);

    int GetGearScore(int i, int j)
    {
        var gearBoxValue = 0;
        List<(int i, int j)> listOfCoordinates = new();

        // Represents direction (up, upright, right, downright, down, downleft, left, upleft)
        int[] dRow = {-1, -1, 0, 1, 1, 1, 0, -1};
        int[] dColumn = {0, 1, 1, 1, 0, -1, -1, -1};
        for (var k = 0; k < 8; k++)
        {
            var adjRow = i + dRow[k];
            var adjCol = j + dColumn[k];

            if (adjRow >= 0 && adjRow < matrix.GetLength(0) && adjCol >= 0 && adjCol < matrix.GetLength(1))
            {
                if (char.IsDigit(matrix[adjRow, adjCol]))
                {
                    listOfCoordinates.Add((adjRow, adjCol));
                }
            }
        }

        listOfCoordinates = FilterAdjacentCoordinates(listOfCoordinates);
        if (listOfCoordinates.Count == 2)
        {
            gearBoxValue =
                listOfCoordinates.Aggregate(1, (current, line) => current * GetNumberForPosition(line.i, line.j));
        }

        return gearBoxValue;
    }

    int GetNumberForPosition(int i, int j)
    {
        var n = matrix.GetLength(1);
        var start = j;
        while (start > 0 && char.IsDigit(matrix[i, start - 1]))
        {
            start--;
        }

        var end = j;
        while (end < n - 1 && char.IsDigit(matrix[i, end + 1]))
        {
            end++;
        }

        var numberAsString = new StringBuilder();
        for (var k = start; k <= end; k++)
        {
            numberAsString.Append(matrix[i, k]);
        }

        return int.TryParse(numberAsString.ToString(), out var result) ? result : 0;
    }

    List<(int i, int j)> FilterAdjacentCoordinates(List<(int i, int j)> listOfCoordinates)
    {
        listOfCoordinates = listOfCoordinates.OrderBy(coord => coord.i).ThenBy(coord => coord.j).ToList();

        List<(int i, int j)> filteredList = new()
        {
            listOfCoordinates.First()
        };

        for (var index = 1; index < listOfCoordinates.Count; index++)
        {
            // Retrieve current coordinate and previous one
            var current = listOfCoordinates[index];
            var previous = listOfCoordinates[index - 1];

            // If the coordinates are from the same row and are not adjacent
            if (current.i != previous.i || Math.Abs(current.j - previous.j) != 1)
            {
                filteredList.Add(current);
            }
        }

        return filteredList;
    }
}