using Mastermind.Settings;

namespace Mastermind.Model
{
    public class CodePins
    {
        public Pin[] Array;

        public CodePins(int pinsToGuess)
        {
            Array = new Pin[pinsToGuess]; 
        }
    }
}
