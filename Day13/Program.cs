using Core;

var input = InputHelper.GetListMatrixInput();

Part1();

void Part1()
{
    var rows = 0;
    var columns = 0;
    foreach (var matrix in input)
    {
        var rowMirroringCutoff = CheckRowMirroring(matrix);
        if (rowMirroringCutoff > 0)
        {
            rows += rowMirroringCutoff;
            continue;
        }

        var columnMirroringCutoff = CheckColumnMirroring(matrix);
        if (columnMirroringCutoff > 0)
        {
            columns += columnMirroringCutoff;
        }
    }

    Console.WriteLine(rows + columns);
}

int CheckRowMirroring(IReadOnlyList<string> matrix)
{
    var rowSeparator = -1;
    for (var i = 0; i < matrix.Count - 1; i++)
    {
        if (matrix[i] == matrix[i + 1])
        {
            rowSeparator = i;
            var row1 = rowSeparator + 2;
            var row2 = rowSeparator - 1;
            while (row1 < matrix.Count && row2 >= 0)
            {
                if (matrix[row1] != matrix[row2])
                {
                    rowSeparator = -1;
                    break;
                }

                row1++;
                row2--;
            }
        }

        if (rowSeparator != -1) return (rowSeparator + 1) * 100;
    }

    return 0;
}

int CheckColumnMirroring(IReadOnlyList<string> matrix)
{
    var width = matrix[0].Length;
    var columnSeparator = -1;
    for (var col = 0; col < width - 1; col++)
    {
        if (CompareColumns(matrix, col, col + 1))
        {
            columnSeparator = col;
            var column1 = columnSeparator + 2;
            var column2 = columnSeparator - 1;
            while (column1 < width && column2 >= 0)
            {
                if (!CompareColumns(matrix, column1, column2))
                {
                    columnSeparator = -1;
                    break;
                }

                column1++;
                column2--;
            }
        }

        if (columnSeparator != -1) return columnSeparator + 1;
    }

    return 0;
}

bool CompareColumns(IReadOnlyList<string> matrix, int column1, int column2)
{
    var currentColumn = "";
    var nextColumn = "";
    var height = matrix.Count;

    for (var row = 0; row < height; row++)
    {
        currentColumn += matrix[row][column1];
        nextColumn += matrix[row][column2];
    }

    if (currentColumn.Equals(nextColumn))
    {
        return true;
    }

    return false;
}