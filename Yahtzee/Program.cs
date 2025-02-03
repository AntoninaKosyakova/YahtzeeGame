
/*
* Programming 2 Final Project - 1st Year 2nd Semester
* Created by: Antonina Kosyakova 2238996
* Tested by: Adesola Adeniyi
* Date: 2024-05-27
* The goal of this program is to do the following:
* Make a single player Yahtzee Game. Yahtzee is a dice game where you try to get certain combinations and score points.
*/


namespace ProjectProg4
{
    public enum ScoringCombination
    {
        Ones = 1,
        Twos = 2,
        Threes = 3,
        Fours = 4,
        Fives = 5,
        Sixes = 6,
        Three_of_a_Kind = 7,
        Four_of_a_Kind = 8,
        Small_Straight = 9,
        Large_Straight = 10,
        Full_House = 11,
        Chance = 12,
        Yahtzee = 13,
        Bonus_Yahtzee = 14

    };

    /// <summary>
    /// This program displays the board, calculates scores and displays the thrown dices. It also has the entry point: main.
    /// </summary>

    internal class Program
    {
        const int NumberOfTurns = 13;
        const int NumberOfLinesOnBoard = 14;
        const int MinDiceNum = 1;
        const int MaxDiceNum = 5;

        const int FullHousePoints = 25;
        const int SmallStraightPoints = 30;
        const int LargeStraightPoints = 40;
        const int YahtzeePoints = 50;
        const int BonusYahtzeePoints = 100;

        private static Turn[] playedTurns = new Turn[NumberOfTurns];
        private static int currentTurnNumber = 0;

        static string[] descriptions = new string[]
        {
            "Score the total of dice with the number 1",
            "Score the total of dice with the number 2",
            "Score the total of dice with the number 3",
            "Score the total of dice with the number 4",
            "Score the total of dice with the number 5",
            "Score the total of dice with the number 6",
            "Roll at least 3 of the same number, score the total of that number",
            "Roll at least 4 of the same number, score the total of that number",
            "Roll four dice in sequential order (1-2-3-4, 2-3-4-5, or 3-4-5-6), score 30",
            "Roll five dice in sequential order (1-2-3-4-5 or 2-3-4-5-6), score 40",
            "Roll three of one number and two of another (e.g. 5-5-5-2-2), score 25",
            "Score the total of all five dice",
            "Roll all five dice as the same number, score 50",
            "Only available if you already have a Yahtzee, score 100"
        };


        /// <summary>
        /// Entry point of the program
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                PrintGame();

                PlayGame();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

            

        }

        /// <summary>
        /// Prints the game's table
        /// </summary>
        public static void PrintGame()
        {
            const int CellWidth1 = 5;
            const int CellWidth2 = 16;
            const int CellWidth3 = 77;
            const int CellWidth4 = 14;
            const int CellWidth5 = 7;


            Console.WriteLine("Game Score Board Round " + (currentTurnNumber + 1));
            string dashes = "---------------------------------------------------------------------------------------------------------------------------";
            Console.WriteLine(dashes);
            Console.WriteLine($"{"N",-CellWidth1}|{"Type",-CellWidth2}|{"Description",-CellWidth3}|{"Possible Score",-CellWidth4}|{"Score",-CellWidth5}");
            Console.WriteLine(dashes);
            for (int i = 1; i <= NumberOfLinesOnBoard; i++)
            {
                ScoringCombination displayedCombination = (ScoringCombination)i;

                int possibleScore = 0;
                if (currentTurnNumber < NumberOfTurns)
                {
                    possibleScore = PossibleScore(displayedCombination);
                }

                ConsoleColor defaultColor = Console.ForegroundColor;
                if (possibleScore > 0)
                    Console.ForegroundColor = ConsoleColor.Yellow;


                Console.WriteLine($"{(int)displayedCombination,-CellWidth1}|{displayedCombination,CellWidth2}|{descriptions[i - 1],-CellWidth3}|{possibleScore,-CellWidth4}|{PlayedScore(displayedCombination),-CellWidth5}");
                Console.ForegroundColor = defaultColor;

                Console.WriteLine(dashes);
            }
            Console.WriteLine("Total:" + TotalScore());
            Console.WriteLine();

            if (currentTurnNumber < NumberOfTurns)
            {
                PrintDice();
                Console.WriteLine();
                PrintMainMenu();
            }


        }


        /// <summary>
        /// Displays the 5 dices
        /// </summary>
        private static void PrintDice()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.WriteLine($"[1] +-------+ [2] +-------+ [3] +-------+ [4] +-------+ [5] +-------+ ");
            for (int i = 0; i < Turn.Dices.Length; i++)
            {
                if (Turn.Dices[i].Hold)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Write($"    |   {Turn.Dices[i].ToString()}   | ");
                Console.ForegroundColor = defaultColor;
            }
            Console.WriteLine("\n    +-------+     +-------+     +-------+     +-------+     +-------+");

        }


        /// <summary>
        /// Displays the actions a player can make during his game round
        /// </summary>
        public static void PrintMainMenu()
        {
            Console.WriteLine("Enter");
            if (Turn.AttemptNumberForDiceThrowing < Turn.MaxNumberOfRolls)
            {
                Console.WriteLine("H to hold");
                Console.WriteLine("U to Unhold");
                Console.WriteLine($"R to Roll: Number of Rolls Left: {Turn.MaxNumberOfRolls - Turn.AttemptNumberForDiceThrowing}");
            }
            Console.WriteLine("P to Play a Type");
            Console.WriteLine("E to Exit");
        }


        /// <summary>
        /// This method asks the user for their choice from the main menu and then executes different actions like holding dice, unholding dice, rolling, playing a combination and exiting according to what have been chosen.
        /// </summary>
        private static void PlayGame()
        {

            string choice = "";

            while (choice != "E" && choice != "e" && currentTurnNumber < NumberOfTurns)
            {
                choice = Console.ReadLine();
                if (choice == "H" || choice == "h")
                {
                    int diceToHold = GetNumber("What dices do you want to hold?", MinDiceNum, MaxDiceNum);
                    Turn.Dices[diceToHold - 1].Hold = true;
                    PrintGame();

                }
                else if (choice == "U" || choice == "u")
                {
                    int diceToUnhold = GetNumber("What dices do you want to hold?", MinDiceNum, MaxDiceNum);
                    Turn.Dices[diceToUnhold - 1].Hold = false;
                    PrintGame();
                }
                else if (choice == "R" || choice == "r")
                {


                    if (Turn.AttemptNumberForDiceThrowing < Turn.MaxNumberOfRolls)
                    {
                        RollDice();
                        Turn.AttemptNumberForDiceThrowing++;
                        PrintGame();
                    }
                    else
                    {
                        Console.WriteLine("No more Rolls left");
                    }

                }
                else if (choice == "P" || choice == "p")
                {
                    AskForCombinationAndFiniliseTurn();

                    PrintGame();
                }
                else if (choice == "e" || choice == "E")
                {
                    Console.WriteLine("Have a good day.");
                }
                else
                {

                    Console.WriteLine("You should choose one of the letters from the menu above");

                }

            }

        }

        /// <summary>
        /// This method asks the user for the combination they want to play and then creates an instance of class Turn that is saved in an array and finilises the turn by rolling the dices and resetting the roll attempts to 0
        /// </summary>
        private static void AskForCombinationAndFiniliseTurn()
        {
            const int MinCombination = 1;
            const int MaxCombination = 14;

            ScoringCombination chosenCombination = (ScoringCombination)GetNumber("What combination do you want to choose?", MinCombination, MaxCombination);
            playedTurns[currentTurnNumber] = new Turn(PossibleScore(chosenCombination), Turn.AttemptNumberForDiceThrowing, chosenCombination);

            currentTurnNumber++;
            RollDice();
            Turn.AttemptNumberForDiceThrowing = 0;

        }

        /// <summary>
        /// This method rolls the dice that aren't held
        /// </summary>
        
        private static void RollDice()
        {
            for (int i = 0; i < Turn.Dices.Length; i++)
            {
                if (!Turn.Dices[i].Hold)
                {
                    Turn.Dices[i].Roll();
                }

            }
        }

        /// <summary>
        /// Calculates and returns the possible score for a certain combination 
        /// </summary>
        /// <param name="combination">one of the possible scoring combination in the game.</param>
        /// <returns>A 32-bit positive integer, representing score that the player can get if he chooses a certain combination.</returns>
        
        private static int PossibleScore(ScoringCombination combination)
        {
            if (AlreadyPlayed(combination))
            {
                return 0;
            }

            int possibleScore = 0;

            switch (combination)
            {
                case ScoringCombination.Ones:
                    possibleScore = Turn.GetScoreForSingleUnits(1);
                    break;

                case ScoringCombination.Twos:
                    possibleScore = Turn.GetScoreForSingleUnits(2);
                    break;

                case ScoringCombination.Threes:
                    possibleScore = Turn.GetScoreForSingleUnits(3);
                    break;

                case ScoringCombination.Fours:
                    possibleScore = Turn.GetScoreForSingleUnits(4);
                    break;

                case ScoringCombination.Fives:
                    possibleScore = Turn.GetScoreForSingleUnits(5);
                    break;

                case ScoringCombination.Sixes:
                    possibleScore = Turn.GetScoreForSingleUnits(6);
                    break;

                case ScoringCombination.Three_of_a_Kind:
                    if (Turn.CheckThreeOfAKind())
                        possibleScore = Turn.SumOfDiceValues;
                    break;

                case ScoringCombination.Four_of_a_Kind:
                    if (Turn.CheckFourOfAKind())
                        possibleScore = Turn.SumOfDiceValues;
                    break;

                case ScoringCombination.Small_Straight:
                    if (Turn.CheckSmallStraight())
                        possibleScore = SmallStraightPoints;
                    break;

                case ScoringCombination.Large_Straight:
                    if (Turn.CheckLargeStraight())
                        possibleScore = LargeStraightPoints;
                    break;

                case ScoringCombination.Full_House:
                    if (Turn.CheckFullHouse())
                        possibleScore = FullHousePoints;
                    break;

                case ScoringCombination.Chance:
                    if (Turn.CheckChance())
                        possibleScore = Turn.SumOfDiceValues;
                    break;

                case ScoringCombination.Yahtzee:
                    if (Turn.CheckYahtzee() && !AlreadyHasYahtzee())
                        possibleScore = YahtzeePoints;
                    break;

                case ScoringCombination.Bonus_Yahtzee:
                    if (Turn.CheckYahtzee() && AlreadyHasYahtzee())
                        possibleScore = BonusYahtzeePoints;
                    break;

                default:
                    Console.WriteLine("Unknown scoring combination.");
                    break;
            }

            return possibleScore;
        }


        /// <summary>
        /// Returns the actual score that a player got by choosing to play a certain combination
        /// </summary>
        /// <param name="combination">one of the possible scoring combination in the game.</param>
        /// <returns>A 32-bit positive integer, representing the user's score for a certain combination.</returns>
        
        private static int PlayedScore(ScoringCombination combination)
        {
            for (int i = 0; i < currentTurnNumber; i++)
            {
                if (playedTurns[i].CombinationPlayed == combination)
                {
                    return playedTurns[i].Score;
                }
            }

            return 0;

        }


        /// <summary>
        /// Returns the total score you accumulated in your played turns
        /// </summary>
        /// <returns>A 32-bit positive integer, representing the total score.</returns>
        private static int TotalScore()
        {
            int totalScore = 0;
            for (int i = 0; i < currentTurnNumber; i++)
            {
                totalScore += playedTurns[i].Score;
            }

            return totalScore;

        }


        /// <summary>
        /// Finds out wether the combination has been played or not in the previous turns
        /// </summary>
        /// <param name="combination">one of the possible scoring combination in the game.</param>
        /// <returns>A bool, representing whether or not the combination has been already played.</returns>
        private static bool AlreadyPlayed(ScoringCombination combination)
        {
            for (int i = 0; i < currentTurnNumber; i++)
            {
                if (playedTurns[i].CombinationPlayed == combination)
                {
                    return true;
                }
            }

            return false;

        }


        /// <summary>
        /// Finds out wether Yahtzee combination has been played or not in the previous turns
        /// </summary>
        /// <returns>A bool, representing whether or not Yahtzee has been already played previously.</returns>
        private static bool AlreadyHasYahtzee()
        {
            return AlreadyPlayed(ScoringCombination.Yahtzee);
        }


        /// <summary>
        /// Validates the user's input and asks them to reenter it if there is an error.
        /// </summary>
        /// <param name="msg">The string that needs to be displayed for the user on the console.</param>
        /// <param name="max">The first integer to be used as the maximum value a user can enter correctly.</param>
        /// <param name="min">The second integer to be used as the minimum value a user can enter correctly.</param>
        /// <returns>A 32-bit positive integer, representing the number of a combination</returns>
        public static int GetNumber(string msg, int min, int max)
        {
            int number;
            Console.WriteLine(msg);
            bool valid = int.TryParse(Console.ReadLine(), out number);
            while (!valid || number < min || number > max)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Error invalid number: {msg}");
                Console.ForegroundColor = ConsoleColor.White;
                valid = int.TryParse(Console.ReadLine(), out number);
            }
            return number;
        }
    }
}
