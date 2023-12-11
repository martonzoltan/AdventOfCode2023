using Core;

var map = InputHelper.GetInput().ToList();
HashSet<Point> visited = [];

var directions = new Dictionary<char, Point[]>
{
    {'F', new[] {new Point {X = 1, Y = 0}, new Point {X = 0, Y = 1}}},
    {'7', new[] {new Point {X = -1, Y = 0}, new Point {X = 0, Y = 1}}},
    {'J', new[] {new Point {X = -1, Y = 0}, new Point {X = 0, Y = -1}}},
    {'L', new[] {new Point {X = 1, Y = 0}, new Point {X = 0, Y = -1}}},
    {'|', new[] {new Point {X = 0, Y = 1}, new Point {X = 0, Y = -1}}},
    {'-', new[] {new Point {X = 1, Y = 0}, new Point {X = -1, Y = 0}}}
};

 Part1();
//Part2();

return;

void Part1()
{
    Point startPoint = FindStartingPoint();
    var maxPoint = FindFurthestPoint(startPoint);
    Console.WriteLine(maxPoint);
}

void Part2()
{
    map = ExpandGrid();
    Point startPoint = FindStartingPoint();
    FindFurthestPoint(startPoint);
    var count = FloodFill();
    Console.WriteLine(count);
    return;

    List<string> ExpandGrid()
    {
        List<string> expandedGrid = [];
        foreach (var row in map)
        {
            var topRow = "";
            var bottomRow = "";
            var middleRow = "";
            foreach (var cell in row)
            {
                if (cell == '.')
                {
                    topRow += "...";
                    bottomRow += "...";
                    middleRow += "...";
                }
                else
                {
                    switch (cell)
                    {
                        case 'L':
                            topRow += "#|#";
                            middleRow += $"#{cell}-";
                            bottomRow += "###";
                            break;
                        case 'F':
                            topRow += "###";
                            middleRow += $"#{cell}-";
                            bottomRow += "#|#";
                            break;
                        case '7':
                            topRow += "###";
                            middleRow += $"-{cell}#";
                            bottomRow += "#|#";
                            break;
                        case 'J':
                            topRow += "#|#";
                            middleRow += $"-{cell}#";
                            bottomRow += "###";
                            break;
                        case '|':
                            topRow += "#|#";
                            middleRow += $"#{cell}#";
                            bottomRow += "#|#";
                            break;
                        case '-':
                            topRow += "###";
                            middleRow += $"-{cell}-";
                            bottomRow += "###";
                            break;
                        default:
                            topRow += "#|#";
                            middleRow += $"#{cell}#";
                            bottomRow += "#|#";
                            break;
                    }
                }
            }

            expandedGrid.Add(topRow);
            expandedGrid.Add(middleRow);
            expandedGrid.Add(bottomRow);
        }

        return expandedGrid;
    }

    int FloodFill()
    {
        var rows = map.Count;
        var cols = map[0].Length;
        var gridArray = map.Select(s => s.ToArray()).ToArray();

        var queue = new Queue<Point>();
        var start = new Point {X = 0, Y = 0};
        gridArray[start.Y][start.X] = 'O';
        queue.Enqueue(start);

        while (queue.Any())
        {
            Point point = queue.Dequeue();

            foreach (Point dir in directions.SelectMany(kvp => kvp.Value).Distinct())
            {
                var newX = point.X + dir.X;
                var newY = point.Y + dir.Y;

                if (newX >= 0 && newX < cols && newY >= 0 && newY < rows &&
                    (gridArray[newY][newX] == '.' || gridArray[newY][newX] == '#'))
                {
                    gridArray[newY][newX] = 'O';
                    visited.Add(new Point {X = newX, Y = newY});
                    queue.Enqueue(new Point {X = newX, Y = newY});
                }
            }
        }

        // count inside tiles
        var groundCount = 0;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                if (gridArray[i][j] == '.')
                {
                    gridArray[i][j] = 'I';
                    groundCount++;
                }
            }
        }

        // because of the expansion all ground is present 9 times
        groundCount /= 9;

        var pipeCount = 0;
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var cellVisited = visited.Contains(new Point {X = j, Y = i});
                if (!cellVisited && "F7JL|-".Contains(gridArray[i][j]))
                {
                    if (!HasZeroNeighbor(gridArray, i, j))
                    {
                        gridArray[i][j] = 'I';
                        pipeCount++;
                    }
                    else
                    {
                        gridArray[i][j] = '0';
                    }
                }
            }
        }

        // because of expansion all pipes are extended with 2 extra positions
        pipeCount /= 3;
        return pipeCount + groundCount;
    }

    static bool HasZeroNeighbor(IReadOnlyList<char[]> gridArray, int i, int j)
    {
        int[] directionRow = {0, 1, 0, -1};
        int[] directionCol = {1, 0, -1, 0};

        for (var k = 0; k < 4; k++)
        {
            var newRow = i + directionRow[k];
            var newCol = j + directionCol[k];

            if (newRow >= 0 && newRow < gridArray.Count &&
                newCol >= 0 && newCol < gridArray[0].Length &&
                gridArray[newRow][newCol] == 'O')
            {
                return true;
            }
        }

        return false;
    }
}


int FindFurthestPoint(Point start)
{
    var distances = new Dictionary<Point, int>
    {
        [start] = 0
    };

    var queue = new Queue<Point>();
    queue.Enqueue(start);
    var maxDistance = 0;

    while (queue.Count > 0)
    {
        Point current = queue.Dequeue();
        if (visited.Add(current))
        {
            if (map[current.Y][current.X] != '.' && map[current.Y][current.X] != '#')
            {
                var neighbors = GetNeighbors(current);
                foreach (Point neighbor in neighbors.Where(neighbor => !visited.Contains(neighbor)))
                {
                    distances[neighbor] = distances[current] + 1;
                    maxDistance = Math.Max(maxDistance, distances[neighbor]);
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

    return maxDistance;
}

IEnumerable<Point> GetNeighbors(Point point)
{
    var neighbors = new List<Point>();
    var currentPipeValue = ' ';
    currentPipeValue = map[point.Y][point.X] == 'S' ? '|' : map[point.Y][point.X];
    var possibleDirections = directions[currentPipeValue];
    foreach (Point dir in possibleDirections)
    {
        var newX = point.X + dir.X;
        var newY = point.Y + dir.Y;
        if (newX >= 0 && newX < map[0].Length && newY >= 0 && newY < map.Count)
        {
            neighbors.Add(new Point {X = newX, Y = newY});
        }
    }

    return neighbors;
}

Point FindStartingPoint()
{
    for (var y = 0; y < map.Count; y++)
    {
        for (var x = 0; x < map[y].Length; x++)
        {
            if (map[y][x] == 'S')
            {
                return new Point {X = x, Y = y};
            }
        }
    }

    return new Point();
}

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var p = (Point) obj;
        return X == p.X && Y == p.Y;
    }

    public override int GetHashCode()
    {
        return X ^ Y;
    }
}