using System;
using System.IO;
using static System.Console;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GuessTheNumber
{
    class Program
    {
        static string highScoreFile = "HighScoreBoard.txt";
        static string playerName;

        static void ShowTitleScreen()
        {
            WriteLine(@"
             ____  _       _   _       ____              _
            |  _ \(_) __ _(_)_| |_    |  _ \ _   _  ___ | |
            | | | | |/ _` | |_|_|_|   | | | | | | |/ _ \| |
            | |_| | | (_| | | | |_    | |_| | |_| |  __/| |
            |____/|_|\__, |_| \__/    |____/ \__,_|\____|_|
                      |__/
            ");

            WriteLine("Welcome to Digit Duel!!");
            WriteLine("Press the Enter button to continue!");
            ReadLine();
        }

        static void HighScore()
        {
            WriteLine("===High Scoreboard===");

            if (File.Exists(highScoreFile))
            {
                string[] data = File.ReadAllLines(highScoreFile);
                foreach (string line in data)
                {
                    WriteLine(line);
                }
            }

            else
            {
                WriteLine("No high scores available yet.");
            }

            WriteLine("\nPress Enter to return to the menu.");
            ReadLine();
        }

        static void GiveHint(int guess, int numGuess)
        {
            int diffNum = Math.Abs(guess - numGuess);

            if (diffNum <= 5)
            {
                WriteLine("You're very close! Keep guessing.");
            }

            if (diffNum <= 10)
            {
                WriteLine("Getting warmer. Continue!");
            }

            else
            {
                WriteLine("You're still cold. Try more guesses.");
            }
        }

        static void StartGame()
        {
            Random number = new Random();
            bool playAgain = true;
            int totalScore = 0;
            int round = 1;
            int maxRange = 100;
            int maxTries = 10;
            int numGuess;
            int userGuess = -1;
            int userTries = 0;
            string input;
            bool isValid;
            int highScore = 0;
            string filePath = "Scoreboard.txt";
            bool correctGuess = false;

            while (playAgain)
            {
                Console.Clear();
                WriteLine($"=== ROUND {round} ===");

                int attemptsThisRound = 0;

                int difficultyMultiplier = 1;

                WriteLine("Select Difficulty Level:");
                WriteLine("1. Easy (Range: 0-50, 10 Tries Allowed)");
                WriteLine("2. Medium (Range: 0-100, 7 Tries Allowed)");
                WriteLine("3. Hard (Range: 0-200, 5 Tries Allowed)");
                Write("Select your option and press the Enter button: ");
                int difficulty = int.Parse(ReadLine());

                switch (difficulty)
                {
                    case 1:
                        maxRange = 50;
                        maxTries = 10;
                        difficultyMultiplier = 1;
                        break;

                    case 2:
                        maxRange = 100;
                        maxTries = 7;
                        difficultyMultiplier = 2;
                        break;

                    case 3:
                        maxRange = 200;
                        maxTries = 5;
                        difficultyMultiplier = 3;
                        break;

                    default:
                        WriteLine("Invalid difficulty level. Defaulting to Medium level.");
                        maxRange = 100;
                        maxTries = 7;
                        difficultyMultiplier = 2;
                        break;
                }

                numGuess = number.Next(0, maxRange);

                WriteLine($"Guess a number between 0 and {maxRange}:");

                while (userTries < maxTries)
                {
                    Write("Your guess is: ");
                    input = ReadLine();
                    isValid = int.TryParse(input, out userGuess);

                    if (!isValid)
                    {
                        WriteLine("Invalid input. Please enter a number");
                        continue;
                    }

                    attemptsThisRound++;

                    if (userGuess > numGuess)
                    {
                        WriteLine("Number is too high!!");
                        GiveHint(userGuess, numGuess);
                    }

                    else if (userGuess < numGuess)
                    {
                        WriteLine("Number is too low!!");
                        GiveHint(userGuess, numGuess);
                    }

                    else
                    {
                        int userScore = maxTries - attemptsThisRound + 1 * difficultyMultiplier;
                        WriteLine($"Correct! You guessed it in {attemptsThisRound} attempts.");
                        WriteLine($"Your score is: {userScore}!");
                        correctGuess = true;

                        if (userScore > highScore)
                        {
                            highScore = userScore;
                            File.WriteAllText(filePath, highScore.ToString());
                            WriteLine("New High Score!");
                        }

                        break;
                    }

                    if (!correctGuess)
                    {
                        WriteLine($"Sorry, you have reached the maximum number of tries. The number was {numGuess}.");
                        break;
                    }
                    
                    //if (attemptsThisRound == 3 && userGuess != numGuess)
                    //{
                    //    WriteLine("Would you like a hint? (y/n)");
                    //    string userRequest = ReadLine().ToLower();

                    //    if (userRequest == "y")
                    //    {
                    //        int hint = number.Next(1, 3);

                    //        if (hint == 1)
                    //        {
                    //            if (numGuess % 2 == 0)
                    //            {
                    //                WriteLine("Hint: Number is even.");
                    //            }

                    //            else
                    //            {
                    //                WriteLine("Hint: Number is odd.");
                    //            }
                    //        }

                    //        else
                    //        {
                    //            int mid = maxRange / 2;
                    //            if (numGuess > mid)
                    //            {
                    //                WriteLine($"Hint: Number is greater than {mid}.");
                    //            }

                    //            else
                    //            {
                    //                WriteLine($"Hint: Number is less than or equal to {mid}.");
                    //            }
                    //        }
                    //    }
                    //}
                }

                WriteLine($"\nTotal Score: {totalScore}.");

                HighScore();

                WriteLine($"Final High Score: {highScore}");

                Write("\nDo you want to play again? (y/n): ");
                string response = ReadLine().ToLower();

                if (response != "y")
                {
                    playAgain = false;
                    WriteLine("Thanks for playing!");
                }
            }
            
        }
        static void Main()
        {
            ShowTitleScreen();

            Write("Enter your name: ");
            playerName = ReadLine();

            while (true)
            {
                Console.Clear();

                WriteLine("===MAIN MENU===");
                WriteLine("1. Start Game");
                WriteLine("2. View High Score");
                WriteLine("3. Exit");
                Write("Choose an option and press enter to continue: ");
                string choice = ReadLine();

                if (choice == "1")
                {
                    StartGame();
                }

                if (choice == "2")
                {
                    HighScore();
                }

                if (choice == "3")
                {
                    WriteLine("Thank you for playing Digit Duel! Goodbye...");
                    break;
                }

                else
                {
                    WriteLine("Invalid number. Please select a number from the menu above and press enter.");
                    ReadLine();
                }
            }

            
        }
    }
}