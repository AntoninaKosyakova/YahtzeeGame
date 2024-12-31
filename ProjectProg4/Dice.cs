using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectProg4
{

    /// <summary>
    /// Creates a dice and then specifies if it is held or not.
    /// </summary>
    public class Dice
    {
        #region fields
        private int number;
        private bool hold;
        #endregion

        #region properties

        /// <summary>
        /// Returns the number of the dice.
        /// </summary>       
        /// <returns>A 32-bit positive integer, representing the number of the dice.</returns>        
        public int Number
        {
            get { return number; }

        }

        /// <summary>
        /// Tells whether or not a dice is held.
        /// </summary>       
        /// <returns>A bool, representing whether or not a dice is held.</returns>
        public bool Hold
        {
            get { return hold; }
            set { hold = value; }
        }

        #endregion

        #region constructor
        public Dice()
        {
            hold = false;
            Roll();
        }

        #endregion


        #region methods

        /// <summary>
        /// Assigns random value to the dice
        /// </summary>
        /// <returns>A 32-bit positive integer, representing the number value of the dice.</returns>
        /// <exception cref="InvalidOperationException">
        /// If someone decided to roll a held dice.
        /// </exception>
        public void Roll()
        {
            if (hold)
                throw new InvalidOperationException("Held dices can't be rolled");//if another programmer decided to call this class with held dices
            
            const int MaxRoll = 6;
            const int MinRoll = 1;

            Random random = new Random();
            int randomNumber = random.Next(MinRoll, MaxRoll);
            number = randomNumber;
            
        }

        /// <summary>
        /// Overrides ToString(). Gets the number of the dice as string.
        /// </summary>       
        /// <returns>A 3string, representing the number of the dice.</returns>       
        public override string ToString()
        {
            return $"{number}";
        }

        #endregion
    }
}
