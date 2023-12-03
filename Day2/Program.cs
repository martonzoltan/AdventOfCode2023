using Core;

var input = InputHelper.GetInput();
// Part1();
Part2();

void Part1()
{
    int sum = 0;
    foreach (var line in input)
    {
        var goodLine = true;
        var splitLine = line.Split(":");
        var game = splitLine[0].Split(" ")[1].Trim();
        foreach (var draws in splitLine[1].Split(";"))
        {
            var drawSplit = draws.Split(",");
            foreach (var colorSplit in drawSplit)
            {
                var colorAmount = Convert.ToInt32(colorSplit.Trim().Split(" ")[0].Trim());
                var color = colorSplit.Trim().Split(" ")[1].Trim();
                if (!IsColorPossible(colorAmount, color))
                {
                    goodLine = false;
                    break;
                }
            }

            if (!goodLine) break;
        }

        if (goodLine)
        {
            sum += Convert.ToInt32(game);
        }
    }

    Console.WriteLine(sum);

    bool IsColorPossible(int colorAmount, string color)
    {
        switch (color)
        {
            case "blue":
                if (colorAmount > 14)
                    return false;
                break;
            case "red":
                if (colorAmount > 12)
                    return false;
                break;
            case "green":
                if (colorAmount > 13)
                    return false;
                break;
        }

        return true;
    }
}

void Part2()
{
    int sum = 0;
    foreach (var line in input)
    {
        var splitLine = line.Split(":");
        var game = splitLine[0].Split(" ")[1].Trim();
        var listOfDraws = new List<Draw>();
        foreach (var draws in splitLine[1].Split(";"))
        {
            var drawSplit = draws.Split(",");
            var draw = new Draw();
            foreach (var colorSplit in drawSplit)
            {
                var colorAmount = Convert.ToInt32(colorSplit.Trim().Split(" ")[0].Trim());
                var color = colorSplit.Trim().Split(" ")[1].Trim();
                ClassifyColor(colorAmount, color, draw);
            }

            listOfDraws.Add(draw);
        }

        var minBlue = listOfDraws.Where(x=>x.Blue > 0).Max(x => x.Blue);
        var minRed = listOfDraws.Where(x=>x.Red > 0).Max(x => x.Red);
        var minGreen = listOfDraws.Where(x=>x.Green > 0).Max(x => x.Green);
        sum += Convert.ToInt32(minBlue * minRed * minGreen);
    }

    Console.WriteLine(sum);

    void ClassifyColor(int colorAmount, string color, Draw draw)
    {
        switch (color)
        {
            case "blue":
                draw.Blue = colorAmount;
                break;
            case "red":
                draw.Red = colorAmount;
                break;
            case "green":
                draw.Green = colorAmount;
                break;
        }
    }
}

internal class Draw
{
    public int Red { get; set; }
    public int Blue { get; set; }
    public int Green { get; set; }
}