using Core;

var input = InputHelper.GetMatrixInput();
var energizedFields = new bool[input.GetLength(0), input.GetLength(1)];
HashSet<(int x, int y, Direction dir)> visited = [];

Part1();
Part2();

return;

void Part1()
{
    energizedFields[0, 0] = true;
    MoveRay(new Ray(0, 0, GetStartingPositionDirection(0, 0, Direction.Right)));

    var sum = 0;
    for (var i = 0; i < energizedFields.GetLength(0); i++)
    {
        for (var j = 0; j < energizedFields.GetLength(1); j++)
        {
            if (energizedFields[i, j])
                sum++;
        }
    }

    Console.Write(sum);
}

void Part2()
{
    var sum = 0;

    for (var i = 0; i < energizedFields.GetLength(0); i++)
    {
        energizedFields = new bool[input.GetLength(0), input.GetLength(1)];
        var localSum = 0;
        visited = [];

        energizedFields[0, i] = true;
        var startingDir = GetStartingPositionDirection(0, i, Direction.Down);
        MoveRay(new Ray(0, i, startingDir));

        for (var ii = 0; ii < energizedFields.GetLength(0); ii++)
        {
            for (var jj = 0; jj < energizedFields.GetLength(1); jj++)
            {
                if (energizedFields[ii, jj])
                    localSum++;
            }
        }

        if (localSum > sum)
        {
            sum = localSum;
        }
    }

    for (var i = 0; i < energizedFields.GetLength(0); i++)
    {
        energizedFields = new bool[input.GetLength(0), input.GetLength(1)];
        var localSum = 0;
        visited = [];

        energizedFields[i, energizedFields.GetLength(0) - 1] = true;
        var startingDir = GetStartingPositionDirection(i, energizedFields.GetLength(0) - 1, Direction.Left);
        MoveRay(new Ray(i, energizedFields.GetLength(0) - 1, startingDir));

        for (var ii = 0; ii < energizedFields.GetLength(0); ii++)
        {
            for (var jj = 0; jj < energizedFields.GetLength(1); jj++)
            {
                if (energizedFields[ii, jj])
                    localSum++;
            }
        }

        if (localSum > sum)
        {
            sum = localSum;
        }
    }

    for (var i = input.GetLength(0) - 1; i >= 0; i--)
    {
        energizedFields = new bool[input.GetLength(0), input.GetLength(1)];
        var localSum = 0;
        visited = [];

        energizedFields[input.GetLength(0) - 1, i] = true;
        var startingDir = GetStartingPositionDirection(input.GetLength(0) - 1, i, Direction.Up);
        MoveRay(new Ray(input.GetLength(0) - 1, i, startingDir));

        for (var ii = 0; ii < energizedFields.GetLength(0); ii++)
        {
            for (var jj = 0; jj < energizedFields.GetLength(1); jj++)
            {
                if (energizedFields[ii, jj])
                    localSum++;
            }
        }

        if (localSum > sum)
        {
            sum = localSum;
        }
    }

    for (int i = input.GetLength(0) - 1; i >= 0; i--)
    {
        energizedFields = new bool[input.GetLength(0), input.GetLength(1)];
        var localSum = 0;
        visited = [];

        energizedFields[i, 0] = true;
        var startingDir = GetStartingPositionDirection(i, 0, Direction.Right);
        MoveRay(new Ray(i, 0, startingDir));

        for (var ii = 0; ii < energizedFields.GetLength(0); ii++)
        {
            for (var jj = 0; jj < energizedFields.GetLength(1); jj++)
            {
                if (energizedFields[ii, jj])
                    localSum++;
            }
        }

        if (localSum > sum)
        {
            sum = localSum;
        }
    }

    Console.Write(sum);
}

void MoveRay(Ray ray)
{
    if (!visited.Add((ray.X, ray.Y, ray.Direction)))
    {
        return;
    }

    switch (ray.Direction)
    {
        case Direction.Left:
            ray.Y--;
            if (MatrixHelper.IsInsideMatrix(input, ray.X, ray.Y))
            {
                energizedFields[ray.X, ray.Y] = true;
                switch (input[ray.X, ray.Y])
                {
                    case '.':
                    case '-':
                        break;
                    case '/':
                        ray.Direction = Direction.Down;
                        break;
                    case '\\':
                        ray.Direction = Direction.Up;
                        break;
                    case '|':
                        MoveRay(new Ray(ray.X, ray.Y, Direction.Down));
                        ray.Direction = Direction.Up;
                        break;
                }
            }
            else
            {
                return;
            }

            break;
        case Direction.Down:
            ray.X++;
            if (MatrixHelper.IsInsideMatrix(input, ray.X, ray.Y))
            {
                energizedFields[ray.X, ray.Y] = true;
                switch (input[ray.X, ray.Y])
                {
                    case '.':
                    case '|':
                        break;
                    case '/':
                        ray.Direction = Direction.Left;
                        break;
                    case '\\':
                        ray.Direction = Direction.Right;
                        break;
                    case '-':
                        MoveRay(new Ray(ray.X, ray.Y, Direction.Right));
                        ray.Direction = Direction.Left;
                        break;
                }
            }
            else
            {
                return;
            }

            break;
        case Direction.Right:
            ray.Y++;
            if (MatrixHelper.IsInsideMatrix(input, ray.X, ray.Y))
            {
                energizedFields[ray.X, ray.Y] = true;
                switch (input[ray.X, ray.Y])
                {
                    case '.':
                    case '-':
                        break;
                    case '/':
                        ray.Direction = Direction.Up;
                        break;
                    case '\\':
                        ray.Direction = Direction.Down;
                        break;
                    case '|':
                        MoveRay(new Ray(ray.X, ray.Y, Direction.Down));
                        ray.Direction = Direction.Up;
                        break;
                }
            }
            else
            {
                return;
            }

            break;
        case Direction.Up:
            ray.X--;
            if (MatrixHelper.IsInsideMatrix(input, ray.X, ray.Y))
            {
                energizedFields[ray.X, ray.Y] = true;
                switch (input[ray.X, ray.Y])
                {
                    case '.':
                    case '|':
                        break;
                    case '/':
                        ray.Direction = Direction.Right;
                        break;
                    case '\\':
                        ray.Direction = Direction.Left;
                        break;
                    case '-':
                        MoveRay(new Ray(ray.X, ray.Y, Direction.Right));
                        ray.Direction = Direction.Left;
                        break;
                }
            }
            else
            {
                return;
            }

            break;
    }

    MoveRay(new Ray(ray.X, ray.Y, ray.Direction));
}

Direction GetStartingPositionDirection(int i, int j, Direction initDirection)
{
    switch (initDirection)
    {
        case Direction.Left:
            switch (input[i, j])
            {
                case '.':
                case '-':
                    return Direction.Left;
                case '/':
                    return Direction.Down;
                case '\\':
                    return Direction.Up;
                case '|':
                    return Direction.Down;
            }

            break;
        case Direction.Down:
            switch (input[i, j])
            {
                case '.':
                case '|':
                    return Direction.Down;
                case '/':
                    return Direction.Left;
                case '\\':
                    return Direction.Right;
                case '-':
                    return Direction.Left;
            }

            break;
        case Direction.Right:
            switch (input[i, j])
            {
                case '.':
                case '-':
                    return Direction.Right;
                case '/':
                    return Direction.Up;
                case '\\':
                    return Direction.Down;
                case '|':
                    return Direction.Up;
            }

            break;
        case Direction.Up:
            switch (input[i, j])
            {
                case '.':
                case '|':
                    return Direction.Up;
                case '/':
                    return Direction.Right;
                case '\\':
                    return Direction.Left;
                case '-':
                    return Direction.Right;
            }

            break;
    }

    return Direction.Right;
}

internal sealed class Ray(int x, int y, Direction direction)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public Direction Direction { get; set; } = direction;
}

internal enum Direction
{
    Left,
    Down,
    Right,
    Up,
}