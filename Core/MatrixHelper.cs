namespace Core;

public class MatrixHelper
{
    public static bool IsInsideMatrix(char[,] matrix, int i, int j)
    {
        return i >= 0 && j >= 0 && i < matrix.GetLength(0) && j < matrix.GetLength(1);
    }

    public static void PrintMatrix(char[,] matrix)
    {
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j]);
            }

            Console.WriteLine();
        }
    }
}