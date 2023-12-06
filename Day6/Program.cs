List<(int Time, int Distance)> races = new()
{
    (71530, 940200)
};

SolveRaces();

return;

void SolveRaces()
{
    var result = 1;
    foreach (var (time, distance) in races)
    {
        var beatTheRecord = 0;
        for (var i = 1; i < time; i++)
        {
            var timeLeftToTravel = time - i;
            var distanceTraveled = i * timeLeftToTravel;
            if (distanceTraveled > distance)
            {
                beatTheRecord++;
            }
        }

        if (beatTheRecord <= 0) continue;
        result *= beatTheRecord;
    }

    Console.WriteLine(result);
}