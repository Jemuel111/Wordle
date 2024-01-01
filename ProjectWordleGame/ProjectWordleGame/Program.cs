using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    struct Player
    {
        public string Name;
        public int Score;
        public TimeSpan Time;
    }

    static List<Player> easyLeaderboard = new List<Player>();
    static List<Player> mediumLeaderboard = new List<Player>();
    static List<Player> hardLeaderboard = new List<Player>();

    static Dictionary<string, string[]> difficultyLevels = new Dictionary<string, string[]>
    {
        { "easy", new string[] { "Apple", "Mango", "Peach", "Grape", "Lemon", "Melon", "Dates", "Guava"} },
        { "medium", new string[] { "Tangelo", "Soursop", "Pommelo", "Satsuma", "Kumquat", "Apricot", "Avocado", "Mangoes" } },
        { "hard", new string[] { "Blackberry", "Strawberry", "Watermelon", "Gooseberry", "Breadfruit", "Cloudberry", "Mangosteen", "Cantaloupe", "Clementine", "Mangosteen" } }
    };

    static List<Player> leaderboard = new List<Player>();

    static void Main()
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            DisplayCenteredAsciiArt();

            Console.WriteLine("=== Simple Console Game ===");
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. Instructions");
            Console.WriteLine("3. High Scores");
            Console.WriteLine("4. Quit");

            Console.Write("Choose an option (1-4): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartGame();
                    break;
                case "2":
                    DisplayInstructions();
                    break;
                case "3":
                    DisplayLeaderboards();
                    break;
                case "4":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                    break;
            }
        }

        Console.WriteLine("Goodbye!");
    }

    static void DisplayCenteredAsciiArt()
    {
        string asciiArt = @"
██╗    ██╗ ██████╗ ██████╗ ██████╗ ██╗     ███████╗
██║    ██║██╔═══██╗██╔══██╗██╔══██╗██║     ██╔════╝
██║ █╗ ██║██║   ██║██████╔╝██║  ██║██║     █████╗  
██║███╗██║██║   ██║██╔══██╗██║  ██║██║     ██╔══╝  
╚███╔███╔╝╚██████╔╝██║  ██║██████╔╝███████╗███████╗
 ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚══════╝
";

        int consoleWidth = Console.WindowWidth;
        int consoleHeight = Console.WindowHeight;

        int artWidth = asciiArt.Split('\n')[0].Length;
        int artHeight = asciiArt.Split('\n').Length;

        int startX = Math.Max((consoleWidth - artWidth) / 2, 0);
        int startY = Math.Max((consoleHeight - artHeight) / 2, 0);

        Console.SetCursorPosition(startX, startY);
        Console.WriteLine(asciiArt);
    }

    static void StartGame()
    {
        bool playAgain = true;
        string[] selectedDifficulty = null;
        int maxAttempts = 0;

        while (playAgain)
        {
            Console.Clear();

            Console.WriteLine("Choose a difficulty level: ");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");
            Console.WriteLine("4. Return to Main Menu");
            Console.Write("Enter the number corresponding to your choice: ");

            string difficultyChoice = Console.ReadLine();

            switch (difficultyChoice)
            {
                case "1":
                    selectedDifficulty = difficultyLevels["easy"];
                    maxAttempts = 3;
                    break;
                case "2":
                    selectedDifficulty = difficultyLevels["medium"];
                    maxAttempts = 4;
                    break;
                case "3":
                    selectedDifficulty = difficultyLevels["hard"];
                    maxAttempts = 5;
                    break;
                case "4":
                    playAgain = false;
                    break;
                default:
                    Console.WriteLine("Invalid difficulty choice. Defaulting to easy.");
                    selectedDifficulty = difficultyLevels["easy"];
                    maxAttempts = 3;
                    break;
            }

            if (!playAgain)
            {
                break;
            }

            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

            int playerScore = 100;

            Console.WriteLine("\nGame started! Have fun!");

            Random random = new Random();
            string secretWord = selectedDifficulty[random.Next(selectedDifficulty.Length)];

            int attempts = 0;
            int scorePlayer = playerScore;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("\nWelcome to Wordle Game! Try to guess the secret fruit names. It has " + secretWord.Length + " letters.");


            while (attempts < maxAttempts)
            {
                DisplayWord(secretWord, attempts);

                Console.Write("Enter your guess: ");
                string guess = Console.ReadLine();

                if (guess.ToLower() == secretWord.ToLower())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Congratulations! You guessed the correct word.");
                    Console.ResetColor();
                    break;
                }

                if (guess.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Exiting the game...");
                    Console.ResetColor();
                    return;
                }

                DisplayFeedback(secretWord, guess);

                attempts++;
            }

            stopwatch.Stop();
            scorePlayer -= attempts * 10;

            if (attempts == maxAttempts)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sorry, you ran out of attempts. The correct word was: " + secretWord);
                Console.ResetColor();
            }

            switch (difficultyChoice)
            {
                case "1":
                    easyLeaderboard.Add(new Player { Name = playerName, Score = scorePlayer, Time = stopwatch.Elapsed });
                    break;
                case "2":
                    mediumLeaderboard.Add(new Player { Name = playerName, Score = scorePlayer, Time = stopwatch.Elapsed });
                    break;
                case "3":
                    hardLeaderboard.Add(new Player { Name = playerName, Score = scorePlayer, Time = stopwatch.Elapsed });
                    break;
                default:
                    Console.WriteLine("Invalid difficulty choice. Defaulting to easy.");
                    easyLeaderboard.Add(new Player { Name = playerName, Score = scorePlayer, Time = stopwatch.Elapsed });
                    break;
            }

            leaderboard.Add(new Player { Name = playerName, Score = scorePlayer, Time = stopwatch.Elapsed });

            Console.WriteLine("Game over! Your score: " + scorePlayer + ", Time: " + stopwatch.Elapsed.TotalSeconds.ToString("F2") + " seconds");


            Console.Write("Do you want to play again? (y/n): ");
            string playAgainChoice = Console.ReadLine();

            playAgain = (playAgainChoice.ToLower() == "y");
        }
    }

    static void DisplayLeaderboards()
    {
        Console.WriteLine("=== High Scores ===");
        Console.WriteLine("1. Easy Leaderboard");
        Console.WriteLine("2. Medium Leaderboard");
        Console.WriteLine("3. Hard Leaderboard");
        Console.Write("Choose a leaderboard (1-3): ");

        string leaderboardChoice = Console.ReadLine();

        switch (leaderboardChoice)
        {
            case "1":
                DisplayIndividualLeaderboard("Easy", easyLeaderboard);
                break;
            case "2":
                DisplayIndividualLeaderboard("Medium", mediumLeaderboard);
                break;
            case "3":
                DisplayIndividualLeaderboard("Hard", hardLeaderboard);
                break;
            default:
                Console.WriteLine("Invalid leaderboard choice.");
                break;
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void DisplayIndividualLeaderboard(string difficulty, List<Player> individualLeaderboard)
    {
        Console.WriteLine("===" + difficulty + "Leaderboard ===");

        individualLeaderboard.Sort((a, b) =>
        {
            int scoreComparison = b.Score.CompareTo(a.Score);
            return scoreComparison != 0 ? scoreComparison : a.Time.CompareTo(b.Time);
        });

        int count = Math.Min(5, individualLeaderboard.Count);
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine((i + 1) + ". " + individualLeaderboard[i].Name + " - " + individualLeaderboard[i].Score + ", Time: " + individualLeaderboard[i].Time.TotalSeconds.ToString("F2") + " seconds");

        }
    }

    static void DisplayInstructions()
    {
        Console.WriteLine("=== Game Instructions ===");
        Console.WriteLine("Welcome to the Wordle Game!");
        Console.WriteLine("Your goal is to guess the secret word, which is a fruit name.");
        Console.WriteLine("Here's how the game works:");
        Console.WriteLine("1. Choose a difficulty level: Easy, Medium, or Hard.");
        Console.WriteLine("2. Enter your name and start guessing!");
        Console.WriteLine("3. You have a maximum of 3 attempts in Easy and 4 attempts in Medium and 5 attempts in the Hard to guess the word.");
        Console.WriteLine("4. After each guess, you will be notified if the letter is in the word.");
        Console.WriteLine("5. The game ends when you guess the word correctly, run out of attempts, or choose to exit.");
        Console.WriteLine("6. If you want to exit the while playing you can type exit ");
        Console.WriteLine("7. Your score is based on the number of attempts and time taken to complete the game.");
        Console.WriteLine("8. The faster you guess correctly, the higher you'll be on the leaderboard!");
        Console.WriteLine("Enjoy the game!");
        Console.ReadLine();
    }

    static void DisplayWord(string secretWord, int attempts)
    {
        Console.WriteLine();
        for (int i = 0; i < secretWord.Length; i++)
        {
            char letter = secretWord[i];

            if (attempts > 0 && secretWord.IndexOf(letter, 0, attempts) != -1)
            {
                Console.Write(letter + " ");
            }
            else
            {
                Console.Write("_ ");
            }
        }
        Console.WriteLine();
    }

    static void DisplayFeedback(string secretWord, string guess)
    {
        Console.WriteLine("Feedback: ");
        for (int i = 0; i < secretWord.Length; i++)
        {

            if (secretWord[i] == guess[i])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(guess[i] + " ");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("_ ");
                Console.ResetColor();
            }
        }
        Console.WriteLine();
    }
}
