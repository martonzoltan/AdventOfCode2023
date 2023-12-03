namespace Core;

public static class InputHelper
{
    public static IEnumerable<string> GetInput()
    {
        var input = File.ReadAllLines(@"input.txt");
        return input;
    }

    public static char[,] GetMatrixInput()
    {
        var input = File.ReadAllLines(@"input.txt").ToList();
        var array = new char[input.Count, input.Count];
        var rows = 0;
        foreach (var line in input)
        {
            for (var i = 0; i < line.Length; i++)
            {
                array[rows, i] = line[i];
            }

            rows++;
        }

        return array;
    }
}