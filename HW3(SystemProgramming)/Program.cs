using System.Text;

namespace HW3_SystemProgramming_
{
    internal class Program
    {
        static void FillFile()
        {
            Card c1 = new Card("5239-1515-1440-1225", "John Johnson", 450.5m, "2343");
            Card c2 = new Card("4169-1410-5545-2200", "Jane Johnson", 1250.5m, "4411");
            Card c3 = new Card("3782-8224-6310-0057", "Bob Smith", 3200.0m, "9921");
            Card c4 = new Card("6011-1111-1111-1117", "Alice Brown", 780.75m, "5567");
            Card c5 = new Card("5500-0055-5555-5559", "Michael Davis", 5000.0m, "1234");
            FileHelper.WriteJsonSerializer(c1, c2, c3, c4, c5);
        }
        static Card[] ReadFromFile()
        {
            var cards = FileHelper.ReadJsonSerializer();
            if (cards == null)
            {
                throw new ArgumentNullException("Error reading from file; null");
            }
            return cards;
        }
        static void TransferAnimation(decimal amount, string destination)
        {
            StringBuilder animationDashes = new StringBuilder("---");
            int counter = 0;
            decimal totalTransferred = 0.0m;
            decimal amountTemp = amount;
            while (amountTemp > 0)
            {
                Console.Clear();
                totalTransferred += amount * 0.1m;
                Console.WriteLine($"{amountTemp -= (amount * 0.1m)}{animationDashes.Append("-")}{totalTransferred}");
                counter++;
                Thread.Sleep(5000); // for test purposes
            }
        }
        static void MoneyTransfer(Card card)
        {
            Console.Clear();
            Card? updatedCard = ReadFromFile().FirstOrDefault(c => c.Pan == card.Pan);
            Console.WriteLine($"Available amount:{updatedCard.Balance}");
            Console.WriteLine("Enter amount($): ");
            decimal amount = decimal.Parse(Console.ReadLine() ?? "0");
            if (card.Balance < amount)
            {
                "Insufficient Balance".ShowErrorMessage();
                Thread.Sleep(1000);
                MoneyTransfer(card);
            }
            Console.WriteLine("Enter destination card NO: ");
            string? destination = Console.ReadLine();

            if (destination == null && destination?.Length != 16 || destination.Length != 19)
            {
                $"Try again".ShowErrorMessage();
                Thread.Sleep(1000);
                MoneyTransfer(card);
            }
            if (destination.Contains("-"))
            {
                destination = destination.Replace("-", "");
            }
            TransferAnimation(amount, destination ?? "0");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully transferred");
            Console.ResetColor();
            updatedCard.Balance -= amount;
            FileHelper.UpdateCard(updatedCard);
        }
        static void ValidationSuccessful(Card card)
        {
            Console.Clear();
            Console.WriteLine("[B] Show Balance");
            Console.WriteLine("[D] Card Details");
            Console.WriteLine("[T] Transfer to Another Card");
            var choice = Console.ReadKey();
            switch (choice.Key)
            {
                case ConsoleKey.B:
                    {
                        Console.Clear();
                        Console.WriteLine($"\bAvailable Balance: ${card.Balance}");
                        break;
                    }
                case ConsoleKey.D:
                    {
                        Console.Clear();
                        StringBuilder cardToShow = new StringBuilder();
                        for (int i = 0; i < card.Pan.Length; i += 4)
                        {
                            string fourNums = card.Pan.Substring(i, 4);
                            if (i >= card.Pan.Length - 4)
                                cardToShow.Append(fourNums);
                            else
                                cardToShow.Append(fourNums + "-");

                        }
                        Console.WriteLine($"\bDetails: \n\tCardNO: {cardToShow}\n\tOwnerName: {card.OwnerName}");
                        break;
                    }
                case ConsoleKey.T:
                    {
                        Console.WriteLine("\bEnter PIN: ");
                        string pinInput = Console.ReadLine();
                        string mutexName = card.Pan;
                        bool isSecondWindow;
                        var m1 = new Mutex(false, mutexName, out isSecondWindow);
                        if (!isSecondWindow)
                        {
                            Console.WriteLine("Another transfer is in progress");
                        }
                        if (pinInput == card.Pin)
                        {
                            using (var m = new Mutex(false, mutexName))
                            {
                                if (m.WaitOne())
                                {
                                    
                                    MoneyTransfer(card);
                                }
                                m.ReleaseMutex();
                            }
                        }
                        else
                        {
                            "Incorrect PIN".ShowErrorMessage();

                            "Returning Card".ShowLoadingAnimation();

                            Environment.Exit(0);
                        }

                        break;
                    }
                default:
                    {
                        ValidationSuccessful(card);
                        break;
                    }
            }
        LeaveOrStay:
            Console.WriteLine("[E] Get Card Back and Leave");
            Console.WriteLine("[C] Continue");
            choice = Console.ReadKey();
            if (choice.Key == ConsoleKey.E)
            {
                "Returning Card".ShowLoadingAnimation();
                Environment.Exit(0);
            }
            else if (choice.Key == ConsoleKey.C)
                ValidationSuccessful(card);
            else
            {
                "TryAgain".ShowErrorMessage();
                Thread.Sleep(500);
                Console.Clear();
                goto LeaveOrStay;
            }

        }


        static void Start()
        {
            string initialText = "Acquiring card data";
            //initialText.ShowLoadingAnimation();
            Console.WriteLine("[E] to exit");
            Console.WriteLine("[I] to continue");
            var choice = Console.ReadKey();
            switch (choice.Key)
            {
                case ConsoleKey.E:
                    {
                        Environment.Exit(0);
                        break;
                    }
                case ConsoleKey.I:
                    {
                        Card[]? cards = null;
                        Thread ReadFileThread = new Thread(() =>
                        {
                            cards = ReadFromFile();
                        });
                        try
                        {
                            ReadFileThread.Start();
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ShowErrorMessage();
                            Environment.Exit(0);
                        }
                    CardPanInputStart:
                        ReadFileThread.Join();
                        Console.Clear();
                        Console.WriteLine("Enter last 4 numbers of your card: ");
                        string? last4Nums = Console.ReadLine();
                        if (last4Nums == null)
                        {
                            throw new ArgumentNullException("Invalid Input(NULL)");
                        }
                        if (last4Nums.Length != 4)
                        {
                            throw new ArgumentException("Invalid Input(Length must be exactly equal to 4)");
                        }
                        if (cards == null)
                        {
                            "Reading From File Error".ShowErrorMessage();
                            Environment.Exit(0);
                        }
                        Card? card = cards?.FirstOrDefault(c => c.Pan.EndsWith(last4Nums));
                        if (card == null)
                        {
                            "Try again".ShowErrorMessage();
                            Thread.Sleep(500);
                            goto CardPanInputStart;
                        }
                        ValidationSuccessful(card);
                        break;
                    }
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {
            try
            {
                Start();

            }
            catch (Exception ex)
            {

                ex.Message.ShowErrorMessage();
                Thread.Sleep(1000);
                "Returning Card".ShowLoadingAnimation();
            }
            //FillFile();
        }
    }
}
