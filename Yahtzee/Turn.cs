using System;

namespace ProjectProg4
{

    /// <summary>
    /// Creates a turn for the user who plays Yahtzee. It saves the user's score and played combination for the turn. Also, contains the 5 dices that the user will play, as well as checks combination those dices can give.
    /// </summary>
    public class Turn
    {
        public const int DiceNumber = 5;
        public const int MaxNumberOfRolls = 2;

        //played Turn fields
        #region feilds
        private int _score;
        private ScoringCombination combinationPlayed;
        #endregion

        //current Turn fields
        private static Dice[] _dice;
        private static int attemptNumberForDiceThrowing;

        #region constructor
        static Turn()
        {
            InitializeDice();
            attemptNumberForDiceThrowing = 0;
        }

        public Turn(int score_, int rolls_, ScoringCombination combinationPlayed_)//I use feilds because I know that I pass only the right values
        {
            _score = score_;
            combinationPlayed = combinationPlayed_;
        }
        #endregion

        #region properties
        

        //read only bc score doesn't change after Turn is created
        /// <summary>
        /// Returns the score that a user got by playing certain combinations
        /// <returns>A 32-bit positive integer, representing the score.</returns>
        public int Score
        {
            get { return _score; }
            
        }


        /// <summary>
        /// Returns the combination played by the user 
        /// </summary>
        /// <returns>A ScoringCombination, representing the combinaiton choosed by the player.</returns>        
        public ScoringCombination CombinationPlayed
        {
            get { return combinationPlayed; }
        }

        /// <summary>
        /// Returns the dice array that contains the dices
        /// </summary>
        /// <returns>A Dice[], representing the dices a player rolled.</returns>   
        public static Dice[] Dices //currently rolled combination for current turn
        {
            get { return _dice; }
        }


        /// <summary>
        /// Returns the attempt number for rolling the dice
        /// </summary>
        /// <returns>A 32-bit positive integer, representing the amount of times a player decided to roll the dice during a turn.</returns>   
        public static int AttemptNumberForDiceThrowing
        {
            get
            {
                return attemptNumberForDiceThrowing;
            }
            set
            {
                attemptNumberForDiceThrowing = value;
            }

        }

        /// <summary>
        /// Returns the dice numbers contained in an array
        /// </summary>
        /// <returns>A integer array, representing the dices' values.</returns> 
        public static int[] DiceNumbers //read-only property to get the dice numbers for no code repetition
        {
            get
            {
                int[] diceNumbers = new int[DiceNumber];

                for (int i = 0; i < DiceNumber; i++)
                {
                    diceNumbers[i] = _dice[i].Number; //get the number of the dice
                }

                return diceNumbers;
            }
        }

        /// <summary>
        /// Returns the sum of dice values
        /// </summary>
        /// <returns>A 32-bit positive integer, representing the sum of dice values.</returns> 
        public static int SumOfDiceValues //decided to make it a property bc no arguments.
        {
            get
            {
                int sumOfDiceValues = 0;
                foreach (int diceNumber in DiceNumbers)
                    sumOfDiceValues += diceNumber;
                return sumOfDiceValues;
            }
        }
        #endregion

        #region methods

        /// <summary>
        /// Initializes the dice array that contains 5 dices
        /// </summary>
        public static void InitializeDice() //creates new dice objects
        {
            _dice = new Dice[DiceNumber];

            for (int i = 0; i < DiceNumber; i++)
            {
                _dice[i] = new Dice(); //dice[i] represents one dice in the array of dices
            }
        }


        /// <summary>
        /// Counts the number of dices that have a certain number on them
        /// </summary>
        /// <param name="num">The non-negative integer to be potentially found in the dices.</param>
        /// <returns>A 32-bit positive integer, representing the amount of dices containing a specific number.</returns>
        public static int GetCountOf(int num)   
        {
            int[] diceNumbers = DiceNumbers;
            int count = 0;

            for (int i = 0; i < diceNumbers.Length; i++)
            {
                if (diceNumbers[i] == num)
                {
                    count++;
                }
            }

            return count;
        }


        /// <summary>
        /// Calculates the score for combinations such as Ones,Twos,Threes,Fours,Fives and Sixes.
        /// </summary>
        /// <param name="num">The non-negative integer to be used in calculating the score.</param>
        /// <returns>A 32-bit positive integer, representing the score the user can get by choosing one of the first six combinations.</returns>     
        public static int GetScoreForSingleUnits(int num) //get as many ones/twos/threes/fours/fives/sixes as possible
        {
            int score = GetCountOf(num) * num;
            return score;
        }


        /// <summary>
        /// Checks if the Three Of a Kind combination is available to the user according to their dices
        /// </summary>
        /// <returns>A bool, representing whether or not Three Of a Kind is an available combination.</returns>
        public static bool CheckThreeOfAKind() //Get three dice with the same number. Points are the sum all dice (not just the three of a kind). 
        {

            return HasNumOfaKind(3);
        }

        /// <summary>
        /// Checks if the Four Of a Kind combination is available to the user according to their dices
        /// </summary>
        /// <returns>A bool, representing whether or not Four Of a Kind is an available combination.</returns>
        public static bool CheckFourOfAKind() //Get four dice with the same number. Points are the sum all dice (not just the four of a kind). 
        {
            return HasNumOfaKind(4);
        }


        /// <summary>
        /// Check if we rolled out specified quantity of same dice numbers
        /// </summary>
        /// <param name="quantity">The non-negative integer to be used as number of times we want to see the same dice number</param>
        /// <returns>A bool, representing whether or not we rolled out specified quantity of same dice numbers.</returns>        
        public static bool HasNumOfaKind(int quantity) 
        {
            return (GetCountOf(1) == quantity || GetCountOf(2) == quantity || GetCountOf(3) == quantity || GetCountOf(4) == quantity || GetCountOf(5) == quantity || GetCountOf(6) == quantity);
        }


        /// <summary>
        /// Checks if the Full House combination is available to the user according to their dices
        /// </summary>
        /// <returns>A bool, representing whether or not Full house is an available combination.</returns>
        public static bool CheckFullHouse() //Get three of a kind and a pair. Scores 25 points.
        {
            return (CheckThreeOfAKind() && HasNumOfaKind(2));
        }


        /// <summary>
        /// Checks if there is a certain number of sequential dice
        /// </summary>
        /// <param name="num">The non-negative integer to be used as the number of sequential dices we want.</param>        
        /// <returns>A bool, representing whether or not there is a certain amount of sequential dices.</returns>        
        public static bool HasConsequentNumberOf(int num) //Checks for small straight or large straight
        {
            int[] diceNumbers = DiceNumbers;
            Array.Sort(diceNumbers);

            int numberOfConsequentDiceValues = num;

            //Diff with index can be only one of {1, 2, 3} for small straight and {1,2} for large
            int firstPossibleDifferenceWithIndex = 1;
            int secondPossibleDifferenceWithIndex = 2;
            int thirdPossibleDifferenceWithIndex = 3;

            //number of dices whose difference with index is 1,2 or 3
            int withDifferenceOfOne = 0;
            int withDifferenceOfTwo = 0;
            int withDifferenceOfThree = 0;

            //calculates number of dices whose value follows index
            for (int i = 0; i < diceNumbers.Length; i++)
            {
                int differenceWithIndex = diceNumbers[i] - i;

                if (differenceWithIndex == firstPossibleDifferenceWithIndex)
                    withDifferenceOfOne++;

                if (differenceWithIndex == secondPossibleDifferenceWithIndex)
                    withDifferenceOfTwo++;

                if (differenceWithIndex == thirdPossibleDifferenceWithIndex)
                    withDifferenceOfThree++;
            }

            if (withDifferenceOfOne == numberOfConsequentDiceValues || withDifferenceOfTwo == numberOfConsequentDiceValues || withDifferenceOfThree == numberOfConsequentDiceValues)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Checks if the Small Straight combination is available to the user according to their dices
        /// </summary>
        /// <returns>A bool, representing whether or not Small Straight is an available combination.</returns>
        public static bool CheckSmallStraight() //Get four sequential dice, 1,2,3,4 or 2,3,4,5 or 3,4,5,6. Scores 30 points.
        {
            return HasConsequentNumberOf(4);
        }


        /// <summary>
        /// Checks if the Large Straight combination is available to the user according to their dices
        /// </summary>
        /// <returns>A bool, representing whether or not Large Straight is an available combination.</returns>
        public static bool CheckLargeStraight() //Get four sequential dice, 1,2,3,4 or 2,3,4,5 or 3,4,5,6. Scores 30 points.
        {
            return HasConsequentNumberOf(5);
        }


        /// <summary>
        /// Checks if the Chance combination is available to the user according to their dices
        /// </summary>
        /// <returns>A bool, representing whether or not Chance is an available combination. Although, it is always available for the first time use.</returns>
        public static bool CheckChance() //You can put anything into chance, it's basically like a garbage can when you don't have anything else you can use the dice for. The score is simply the sum of the dice.
        {
            return (SumOfDiceValues != 0);
        }


        /// <summary>
        /// Checks if the Yahtzee combination is available to the user according to their dices
        /// </summary>
        /// <returns>A bool, representing whether or not Yahtzee is an available combination.</returns>
        public static bool CheckYahtzee()// Five of a kind. Scores 50 points. You can optionally get multiple Yahtzees, see below for details.
        {
            return HasNumOfaKind(5);
        }

        
        
        #endregion



    }
}
