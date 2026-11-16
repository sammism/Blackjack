
namespace Blackjack;

public class BlackjackGame
{
    private const int STARTING_MONEY = 100;
    private const int MINIMUM_BET = 10;
    private const int FREE_MONEY = 25;

    private readonly Deck deck = new();
    private readonly Hand playerHand = new();
    private readonly Hand dealerHand = new();

    private int balance = STARTING_MONEY;
    private int currentBet = 0;

    public void Start()
    {
        Console.WriteLine("Welcome to Blackjack\n");

        bool playing = true;

        while (playing)
        {
            if (balance < MINIMUM_BET)
            {
                balance += FREE_MONEY;

                Console.WriteLine();
                Console.WriteLine("You have run out of playable money!");
                Console.WriteLine($"You received ${FREE_MONEY} for free.");
                Console.WriteLine($"New balance: ${balance}");
            }

            Console.WriteLine();
            Console.WriteLine($"Current balance: ${balance}");

            currentBet = GetBet();

            playerHand.Clear();
            dealerHand.Clear();

            deck.Shuffle();

            PlayRound();

            Console.WriteLine();
            Console.WriteLine("Round complete!");
            Console.WriteLine($"Your balance: ${balance}");

            if (balance < MINIMUM_BET)
            {
                Console.WriteLine(
                    "You will receive $25 before the next round.");
            }

            playing = AskToPlayAgain();
        }

        Console.WriteLine("\nGame Over!");
        Console.WriteLine($"Final balance: ${balance}");
    }

    private int GetBet()
    {
        while (true)
        {
            Console.Write("Enter your bet (minimum $10): $");

            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int bet))
            {
                Console.WriteLine(
                    "Please enter a valid whole number.");
                continue;
            }

            if (bet < MINIMUM_BET)
            {
                Console.WriteLine(
                    "Your bet must be at least $10.");
            }
            else if (bet > balance)
            {
                Console.WriteLine(
                    "You cannot bet more than your balance.");
            }
            else
            {
                balance -= bet;
                return bet;
            }
        }
    }

    private void PlayRound()
    {
        Console.WriteLine();
        Console.WriteLine("Dealing cards...");

        playerHand.AddCard(deck.DealCard());
        dealerHand.AddCard(deck.DealCard());

        playerHand.AddCard(deck.DealCard());
        dealerHand.AddCard(deck.DealCard());

        DisplayHands(false);

        if (dealerHand.IsBlackjack())
        {
            Console.WriteLine();
            Console.WriteLine("The dealer has Blackjack!");

            DisplayHands(true);

            Console.WriteLine("You lose this round.");
            return;
        }

        if (playerHand.IsBlackjack())
        {
            Console.WriteLine();
            Console.WriteLine("That's a Blackjack! You win!");

            int winnings = (int)Math.Round(
                currentBet * 1.5,
                MidpointRounding.AwayFromZero);

            balance += currentBet + winnings;

            Console.WriteLine($"Blackjack payout: ${winnings}");
            return;
        }

        PlayerTurn();

        if (playerHand.IsBusted())
        {
            Console.WriteLine();
            Console.WriteLine("Your hand busted.");
            return;
        }

        DealerTurn();

        ResolveRound();
    }

    private void PlayerTurn()
    {
        bool turnFinished = false;

        while (!turnFinished)
        {
            Console.WriteLine();
            Console.WriteLine("Your hand:");
            playerHand.DisplayHand();

            Console.WriteLine();
            Console.WriteLine("1. Hit");
            Console.WriteLine("2. Stand");
            Console.Write("Choose an action: ");

            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Please enter 1 or 2.");
                continue;
            }

            if (choice == 1)
            {
                Card card = deck.DealCard();

                playerHand.AddCard(card);

                Console.WriteLine();
                Console.WriteLine($"You drew: {card}");

                if (playerHand.IsBusted())
                {
                    Console.WriteLine(
                        $"Your hand value is {playerHand.GetValue()}");

                    Console.WriteLine("That's a bust! You went over 21.");

                    turnFinished = true;
                }
            }
            else if (choice == 2)
            {
                Console.WriteLine(
                    $"You stand with {playerHand.GetValue()}.");

                turnFinished = true;
            }
            else
            {
                Console.WriteLine(
                    "Invalid choice. Select 1 or 2.");
            }
        }
    }

    // Handles the dealer's turn.
    private void DealerTurn()
    {
        Console.WriteLine();
        Console.WriteLine(
            "Dealer reveals their hidden card!");

        DisplayHands(true);

        while (dealerHand.GetValue() < 17)
        {
            Card card = deck.DealCard();

            dealerHand.AddCard(card);

            Console.WriteLine();
            Console.WriteLine($"Dealer draws: {card}");

            Console.WriteLine(
                $"Dealer total: {dealerHand.GetValue()}");
        }

        Console.WriteLine();
        Console.WriteLine(
            $"Dealer stands with {dealerHand.GetValue()}.");
    }

    private void ResolveRound()
    {
        int playerTotal = playerHand.GetValue();
        int dealerTotal = dealerHand.GetValue();

        Console.WriteLine();
        Console.WriteLine("==============================");
        Console.WriteLine("         ROUND RESULTS");
        Console.WriteLine("==============================");

        Console.WriteLine($"Your total: {playerTotal}");
        Console.WriteLine($"Dealer total: {dealerTotal}");

        if (dealerHand.IsBusted())
        {
            Console.WriteLine("Dealer busted! You win!");

            balance += currentBet * 2;
        }
        else if (playerTotal > dealerTotal)
        {
            Console.WriteLine("You win!");

            balance += currentBet * 2;
        }
        else if (playerTotal < dealerTotal)
        {
            Console.WriteLine("Dealer wins. You lose.");
        }
        else
        {
            Console.WriteLine(
                "It's a tie! Your bet is returned.");

            balance += currentBet;
        }
    }

    private void DisplayHands(bool revealDealer)
    {
        Console.WriteLine();
        Console.WriteLine("Your hand:");

        playerHand.DisplayHand();

        Console.WriteLine();
        Console.WriteLine("Dealer's hand:");

        if (revealDealer)
        {
            dealerHand.DisplayHand();
        }
        else
        {
            dealerHand.DisplayHiddenDealerHand();
        }
    }

    private bool AskToPlayAgain()
    {
        while (true)
        {
            Console.Write(
                "Would you like to play again? (y/n): ");

            string? answer = Console.ReadLine()?.Trim();

            if (string.Equals(
                answer, "y",
                StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(
                answer, "n",
                StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Console.WriteLine("Please enter y or n.");
        }
    }
}
