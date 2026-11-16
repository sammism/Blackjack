
namespace Blackjack;

public class Hand
{
    private readonly List<Card> cards = new();

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public int GetValue()
    {
        int total = 0;
        int aceCount = 0;

        foreach (Card card in cards)
        {
            total += card.Value;

            if (card.Value == 11)
            {
                aceCount++;
            }
        }

        // Change Aces from 11 to 1 when necessary.
        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }

    public bool IsBlackjack()
    {
        return cards.Count == 2 && GetValue() == 21;
    }

    public bool IsBusted()
    {
        return GetValue() > 21;
    }

    public void DisplayHand()
    {
        foreach (Card card in cards)
        {
            Console.WriteLine($"  {card}");
        }

        Console.WriteLine($"  Total: {GetValue()}");
    }

    public void DisplayHiddenDealerHand()
    {
        if (cards.Count == 0)
            return;

        Console.WriteLine($"  {cards[0]}");
        Console.WriteLine("  [Hidden Card]");
    }

    public void Clear()
    {
        cards.Clear();
    }
}
