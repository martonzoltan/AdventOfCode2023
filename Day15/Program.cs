using Core;

var input = InputHelper.GetInput().FirstOrDefault();
if (input is null) throw new ArgumentException();

//Part1();
Part2();

return;

void Part1()
{
    var steps = input.Split(",");
    var sum = 0;
    foreach (var step in steps)
    {
        sum += GetHashValue(step);
    }

    Console.WriteLine(sum);
}

void Part2()
{
    var steps = input.Split(",");
    var boxes = new Dictionary<int, List<Lens>>();
    foreach (var step in steps)
    {
        string label;
        if (step.Split('=').Length == 2)
        {
            label = step.Split('=')[0];
            var focalLength = int.Parse(step.Split('=')[1]);
            var lensToAdd = new Lens(label, focalLength);
            var boxPosition = GetHashValue(label);
            if (!boxes.ContainsKey(boxPosition))
            {
                boxes[boxPosition] = [];
            }

            var boxToUpdate = boxes[boxPosition].FirstOrDefault(x => x.Label == label);
            if (boxToUpdate is null)
            {
                boxes[boxPosition].Add(lensToAdd);
            }
            else
            {
                boxToUpdate.FocalLength = lensToAdd.FocalLength;
            }
        }
        else
        {
            label = step.TrimEnd('-');
            var boxPosition = GetHashValue(label);
            if (boxes.ContainsKey(boxPosition))
            {
                var boxToRemove = boxes[boxPosition].FirstOrDefault(x => x.Label == label);
                if (boxToRemove is not null)
                {
                    boxes[boxPosition].Remove(boxToRemove);
                }
            }
        }
    }

    var sum = 0;
    foreach (var box in boxes)
    {
        var boxValue = 0;
        var i = 1;
        foreach (var lens in box.Value)
        {
            boxValue += (1 + box.Key) * i * lens.FocalLength;
            i++;
        }

        sum += boxValue;
    }

    Console.WriteLine(sum);
}

int GetHashValue(string step)
{
    var hash = 0;
    foreach (var character in step)
    {
        hash += character;
        hash *= 17;
        hash %= 256;
    }

    return hash;
}

internal sealed class Lens(string Label, int FocalLength)
{
    public string Label { get; } = Label;
    public int FocalLength { get; set; } = FocalLength;
}