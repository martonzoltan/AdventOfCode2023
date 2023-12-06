using Core;

List<long> seeds = new();
List<Map> seedSoil = new();
List<Map> soilFertilizer = new();
List<Map> fertilizerWater = new();
List<Map> waterLight = new();
List<Map> lightTemperature = new();
List<Map> temperatureHumidity = new();
List<Map> humidityLocation = new();

var input = InputHelper.GetInput().ToList();
Parse();

// Part1();
Part2();

return;

void Parse()
{
    List<Map>? currentList = null;

    foreach (string line in input)
    {
        if (line.Contains(':'))
        {
            switch (line[..line.IndexOf(':')].Trim())
            {
                case "seeds":
                    currentList = null;
                    var seedNumbers = line[(line.IndexOf(':') + 1)..].Trim().Split(' ');
                    seeds.AddRange(seedNumbers.Select(number => Convert.ToInt64(number)));
                    break;

                case "seed-to-soil map":
                    currentList = seedSoil;
                    break;

                case "soil-to-fertilizer map":
                    currentList = soilFertilizer;
                    break;

                case "fertilizer-to-water map":
                    currentList = fertilizerWater;
                    break;

                case "water-to-light map":
                    currentList = waterLight;
                    break;

                case "light-to-temperature map":
                    currentList = lightTemperature;
                    break;

                case "temperature-to-humidity map":
                    currentList = temperatureHumidity;
                    break;

                case "humidity-to-location map":
                    currentList = humidityLocation;
                    break;
            }
        }
        else if (currentList != null)
        {
            string[] mapNumbers = line.Split(' ');
            if (mapNumbers.Length == 3)
            {
                currentList.Add(new Map(
                    Convert.ToInt64(mapNumbers[0]),
                    Convert.ToInt64(mapNumbers[1]),
                    Convert.ToInt64(mapNumbers[2])
                ));
            }
        }
    }
}

long GetDestinationForSource(long seed, List<Map> map)
{
    foreach (var soil in map)
    {
        if (seed >= soil.Source && seed < soil.Source + soil.Length)
        {
            return soil.Destination + seed - soil.Source;
        }
    }

    return seed;
}

void Part1()
{
    List<long> locations = new();
    foreach (var seed in seeds)
    {
        var soil = GetDestinationForSource(seed, seedSoil);
        var fertilizer = GetDestinationForSource(soil, soilFertilizer);
        var water = GetDestinationForSource(fertilizer, fertilizerWater);
        var light = GetDestinationForSource(water, waterLight);
        var temperature = GetDestinationForSource(light, lightTemperature);
        var humidity = GetDestinationForSource(temperature, temperatureHumidity);
        var location = GetDestinationForSource(humidity, humidityLocation);
        locations.Add(location);
    }

    Console.WriteLine(locations.Min());
}

void Part2()
{
    List<long> locations = new();
    for (var i = 0; i < seeds.Count; i += 2)
    {
        Console.Write($"{i} started");
        var j = 0;
        for (var seed = seeds[i]; seed < seeds[i] + seeds[i + 1]; seed++)
        {
            var soil = GetDestinationForSource(seed, seedSoil);
            var fertilizer = GetDestinationForSource(soil, soilFertilizer);
            var water = GetDestinationForSource(fertilizer, fertilizerWater);
            var light = GetDestinationForSource(water, waterLight);
            var temperature = GetDestinationForSource(light, lightTemperature);
            var humidity = GetDestinationForSource(temperature, temperatureHumidity);
            var location = GetDestinationForSource(humidity, humidityLocation);
            locations.Add(location);
            j++;
            if (j % 10_000_000 == 0)
            {
                Console.Write(".");
            }
        }

        Console.WriteLine($"{i} finished");
    }

    Console.WriteLine(locations.Min());
}

internal record Map(long Destination, long Source, long Length);