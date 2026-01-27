using System;

namespace Trivia
{
    public class GameRunner
    {
        private static bool _notAWinner;

        public static void Main(string[] args)
        {
            var aGame = new Game();
            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            new GameRunner().Run(aGame, new Random());
        }

        public void Run(Game aGame, Random random)
        {
            do
            {
                aGame.Roll(random.Next(5) + 1);
                _notAWinner = random.Next(9) == 7 ? aGame.WrongAnswer() : aGame.WasCorrectlyAnswered();
            } while (_notAWinner);
        }
    }
}