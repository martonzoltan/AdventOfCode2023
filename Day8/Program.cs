using Core;

var input = InputHelper.GetInput().ToList();
const string commands = "RL";

//Part1();
Part2();

return;

void Part1()
{
    var nodes = BuildTreeFromText();
    var currentNode = nodes["AAA"];
    var steps = 0;
    while (true)
    {
        var command = commands[steps % commands.Length];
        currentNode = command == 'R' ? currentNode.Right : currentNode.Left;
        steps++;
        if (currentNode.Value != "ZZZ") continue;
        Console.WriteLine("Found");
        break;
    }

    Console.WriteLine(steps);
}

void Part2()
{
    var nodes = BuildTreeFromText();
    var currentNodes = nodes.Where(x => x.Key.EndsWith('A')).Select(x => x.Value).ToList();
    var allExitConditions = currentNodes.Select(CalculateSteps).ToList();

    var totalLcm = allExitConditions[0];
    for (var i = 1; i < allExitConditions.Count; i++)
    {
        totalLcm = MathHelper.LowestCommonMultiplier(totalLcm, allExitConditions[i]);
    }

    Console.WriteLine(totalLcm);
    return;

    long CalculateSteps(Node startNode)
    {
        long steps = 0;
        Node currentNode = startNode;
        var endReached = false;
        do
        {
            foreach (var command in commands)
            {
                steps++;

                currentNode = command == 'R' ? currentNode.Right : currentNode.Left;
                if (currentNode.Value.EndsWith('Z'))
                {
                    endReached = true;
                    break;
                }
            }
        } while (!endReached);

        return steps;
    }
}

Dictionary<string, Node> BuildTreeFromText()
{
    var relations = new Dictionary<string, Tuple<string, string>>();
    var nodes = new Dictionary<string, Node>();
    foreach (var line in input)
    {
        var parts = line.Split('=');
        var key = parts[0].Trim();
        var children = parts[1].Trim(' ', '(', ')').Split(',').Select(part => part.Trim()).ToList();
        relations.Add(key, new Tuple<string, string>(children[0], children[1]));
    }

    foreach (var relation in relations)
    {
        if (!nodes.ContainsKey(relation.Key))
        {
            nodes.Add(relation.Key, new Node(relation.Key));
        }

        Node node = nodes[relation.Key];

        if (!nodes.ContainsKey(relation.Value.Item1))
        {
            nodes.Add(relation.Value.Item1, new Node(relation.Value.Item1));
        }

        node.Left = nodes[relation.Value.Item1];

        if (!nodes.ContainsKey(relation.Value.Item2))
        {
            nodes.Add(relation.Value.Item2, new Node(relation.Value.Item2));
        }

        node.Right = nodes[relation.Value.Item2];
    }

    return nodes;
}

public class Node(string value)
{
    public string Value { get; set; } = value;
    public Node Left { get; set; }
    public Node Right { get; set; }
}