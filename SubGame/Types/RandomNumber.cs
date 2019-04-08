using System;
using System.Security.Cryptography;

namespace SubGame.Types
{
    //How to generate better random values than Random() by using RNGCryptoServiceProvider
    //https://scottlilly.com/create-better-random-numbers-in-c/
    public static class RandomNumber
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int Between(int aMinimumValue, int aMaximumValue)
        {
            byte[] myRandomNumber = new byte[1];

            _generator.GetBytes(myRandomNumber);
            double tempAsciiValueOfRandomCharacter = Convert.ToDouble(myRandomNumber[0]);
            double tempMultiplier = Math.Max(0, (tempAsciiValueOfRandomCharacter / 255d) - 0.00000000001d);
            int myRange = aMaximumValue - aMinimumValue + 1;
            double myRandomValueInRange = Math.Floor(tempMultiplier * myRange);

            return (int)(aMinimumValue + myRandomValueInRange);
        }
    }
}
