using Core;

var input = InputHelper.GetInput().ToList();
Part1();
Part2();

void Part1()
{
    var sum = 0;
    var cards = GetCardData();
    foreach (Card card in cards)
    {
        var winnerCount = card.WinningNumbers.Intersect(card.CardNumbers).Count();
        if (winnerCount > 0)
        {
            sum += (int) Math.Pow(2, winnerCount - 1);
        }
    }

    Console.WriteLine(sum);
}

void Part2()
{
    var cards = GetCardData().ToList();
    var arr = Enumerable.Repeat(1, cards.Count).ToArray();
    foreach (Card card in cards)
    {
        var winnerCount = card.WinningNumbers.Intersect(card.CardNumbers).Count();
        var currentCardCount = arr[card.Id - 1];
        for (var i = card.Id; i < card.Id + winnerCount; i++)
        {
            if (i < cards.Count)
            {
                arr[i] += currentCardCount;
            }
        }
    }

    Console.WriteLine(arr.Sum());
}

IEnumerable<Card> GetCardData()
{
    var cards = new List<Card>();
    var i = 1;
    foreach (var cardLine in input)
    {
        var numbers = cardLine.Split(":")[1];
        var winningNumbers = numbers.Split("|")[0].Trim();
        var cardNumbers = numbers.Split("|")[1].Trim();
        cards.Add(new Card
        {
            Id = i,
            WinningNumbers = winningNumbers.Replace("  ", " ").Split(' ').Select(int.Parse).ToList(),
            CardNumbers = cardNumbers.Replace("  ", " ").Split(' ').Select(int.Parse).ToList(),
        });
        i++;
    }

    return cards;
}

internal class Card
{
    public int Id { get; init; }
    public List<int> WinningNumbers { get; init; } = new();
    public List<int> CardNumbers { get; init; } = new();
}