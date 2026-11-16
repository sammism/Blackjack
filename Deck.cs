
namespace Blackjack;

public class Deck
{
    private readonly List<Card> cards = new();
    private int currentCard = 0;

    public Deck()
    {
        string[] suits =
        {
            "Hearts", "Diamonds", "Clubs", "Spades"
        };

        string[] ranks =
        {
            "2", "3", "4", "5", "6", "7",
            "8", "9", "10", "Jack",
            "Queen", "King", "Ace"
        };

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                int value;

                if (rank == "Ace")
                {
                    value = 11;
                }
                else if (rank == "Jack" ||
                         rank == "Queen" ||
                         rank == "King")
                {
                    value = 10;
                }
                else
                {
                    value = int.Parse(rank);
                }

                cards.Add(new Card(suit, rank, value));
            }
        }

        Shuffle();
    }

    public void Shuffle()
    {
        Random.Shared.Shuffle(cards.ToArray());

        // Shuffle the actual deck.
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = Random.Shared.Next(i + 1);

            (cards[i], cards[j]) = (cards[j], cards[i]);
        }

        currentCard = 0;
    }

    public Card DealCard()
    {
        if (currentCard >= cards.Count)
        {
            throw new InvalidOperationException(
                "No cards left in the deck.");
        }

        return cards[currentCard++];
    }
}
