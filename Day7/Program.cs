using Core;

var input = InputHelper.GetInput();
List<PokerHand> pokerHands = new();
Parse();

//Part1();
Part2();

return;

void Parse()
{
    foreach (var hand in input)
    {
        pokerHands.Add(new PokerHand(hand.Split(" ")[0], Convert.ToInt32(hand.Split(" ")[1])));
    }
}

void Part1()
{
    foreach (PokerHand pokerHand in pokerHands)
    {
        ClassifyCard(pokerHand);
    }

    var sum = 0;
    pokerHands = pokerHands.OrderByDescending(hand => hand, new HandComparer()).ToList();
    for (var i = 0; i < pokerHands.Count; i++)
    {
        sum += pokerHands[i].Value * (i + 1);
    }

    Console.WriteLine(sum);
    return;

    void ClassifyCard(PokerHand hand)
    {
        if (hand.Cards.GroupBy(c => c).Any(g => g.Count() == 5))
        {
            hand.Strength = PokerStrength.FiveOfAKind;
            return;
        }

        if (hand.Cards.GroupBy(c => c).Any(g => g.Count() == 4))
        {
            hand.Strength = PokerStrength.FourOfAKind;
            return;
        }

        var groups = hand.Cards.GroupBy(c => c).ToList();
        if (groups.Count == 2)
        {
            var counts = groups.Select(g => g.Count()).OrderBy(c => c).ToList();
            if (counts[0] == 2 && counts[1] == 3)
            {
                hand.Strength = PokerStrength.FullHouse;
                return;
            }
        }
        else
        {
            var counts = groups.Select(g => g.Count()).OrderBy(c => c).ToList();
            if (counts.Any(x => x == 3))
            {
                hand.Strength = PokerStrength.ThreeOfAKind;
                return;
            }
        }

        var pairs = hand.Cards.GroupBy(c => c).Count(g => g.Count() == 2);
        switch (pairs)
        {
            case 2:
                hand.Strength = PokerStrength.TwoPairs;
                return;
            case 1:
                hand.Strength = PokerStrength.OnePair;
                return;
            default:
                hand.Strength = PokerStrength.HighCard;
                break;
        }
    }
}

void Part2()
{
    foreach (PokerHand pokerHand in pokerHands)
    {
        ClassifyCard(pokerHand);
    }

    var sum = 0;
    pokerHands = pokerHands.OrderByDescending(hand => hand, new HandComparer()).ToList();
    for (var i = 0; i < pokerHands.Count; i++)
    {
        sum += pokerHands[i].Value * (i + 1);
        Console.WriteLine($"{pokerHands[i].Cards} \t {pokerHands[i].Strength}");
    }

    Console.WriteLine(sum);
    return;

    void ClassifyCard(PokerHand hand)
    {
        var jokerCount = hand.Cards.Count(c => c == 'J');
        if (jokerCount == 5)
        {
            hand.Strength = PokerStrength.FiveOfAKind;
            return;
        }

        var cardGroups = hand.Cards.Where(c => c != 'J').GroupBy(c => c);
        var mostFrequentCard = cardGroups.OrderByDescending(g => g.Count()).First().Key;
        var count = hand.Cards.Count(c => c == mostFrequentCard || c == 'J');
        switch (count)
        {
            case 5:
                hand.Strength = PokerStrength.FiveOfAKind;
                return;
            case 4:
                hand.Strength = PokerStrength.FourOfAKind;
                return;
        }

        var groups = hand.Cards.Where(c => c != 'J').GroupBy(c => c).ToList();
        var mostFrequentCardGroup = groups.OrderByDescending(g => g.Count()).First();
        var secondMostFrequentCardGroup = groups.FirstOrDefault(g => g.Key != mostFrequentCardGroup.Key);
        if (mostFrequentCardGroup.Count() == 3 && secondMostFrequentCardGroup?.Count() == 2)
        {
            hand.Strength = PokerStrength.FullHouse;
            return;
        }

        var hasThreeOfAKind = mostFrequentCardGroup.Count() + jokerCount >= 3;
        if (mostFrequentCardGroup.Count() + jokerCount >= 3 && secondMostFrequentCardGroup?.Count() == 2 ||
            mostFrequentCardGroup.Count() == 3 && secondMostFrequentCardGroup?.Count() + jokerCount >= 2)
        {
            hand.Strength = PokerStrength.FullHouse;
            return;
        }

        if (hasThreeOfAKind)
        {
            hand.Strength = PokerStrength.ThreeOfAKind;
            return;
        }

        var pairs = hand.Cards.GroupBy(c => c).Count(g => g.Count() == 2);
        if (jokerCount > 0)
        {
            pairs++;
        }

        switch (pairs)
        {
            case 2:
                hand.Strength = PokerStrength.TwoPairs;
                return;
            case 1:
                hand.Strength = PokerStrength.OnePair;
                return;
            default:
                hand.Strength = PokerStrength.HighCard;
                break;
        }
    }
}

internal sealed class PokerHand(string cards, int value)
{
    public string Cards { get; set; } = cards;
    public int Value { get; set; } = value;
    public PokerStrength Strength { get; set; }
}

internal enum PokerStrength
{
    HighCard,
    OnePair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}

class HandComparer : IComparer<PokerHand>
{
    private static readonly Dictionary<char, int> CardValueOrder = new()
    {
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'J', 0},
        {'T', 9},
        {'9', 8},
        {'8', 7},
        {'7', 6},
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1}
    };

    public int Compare(PokerHand x, PokerHand y)
    {
        var strengthComparison = y.Strength.CompareTo(x.Strength);
        if (strengthComparison != 0)
        {
            return strengthComparison;
        }

        var cardsX = x.Cards.ToList();
        var cardsY = y.Cards.ToList();

        for (int i = 0; i < cardsX.Count; i++)
        {
            var cardComparison = CardValueOrder[cardsY[i]].CompareTo(CardValueOrder[cardsX[i]]);
            if (cardComparison != 0)
            {
                return cardComparison;
            }
        }

        return 0;
    }
}