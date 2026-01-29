using Trivia.Enums;
using Trivia.Interfaces;
using Trivia.Models;

namespace Trivia
{
    public class Game
    {
        private readonly IGameOutput _output;
        private readonly PlayerQueue _players;
        private readonly QuestionDeck _questionDeck;
        private Player CurrentPlayer => _players.Current;

        private static readonly Category[] CategoriesByPlace =
        {
            Category.Pop,
            Category.Science,
            Category.Sports,
            Category.Rock,
            Category.Pop,
            Category.Science,
            Category.Sports,
            Category.Rock,
            Category.Pop,
            Category.Science,
            Category.Sports,
            Category.Rock
        };

        public Game(IGameOutput output)
        {
            _output = output;
            _questionDeck = new QuestionDeck();
            _players = new PlayerQueue();
        }

        public bool Add(string playerName)
        {
            _players.Add(new Player(playerName));

            _output.WriteLine(playerName + " was added");
            _output.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public bool WasCorrectlyAnswered()
        {
            _output.WriteLine("Answer was corrent!!!!");
            CurrentPlayer.AddCoin();
            _output.WriteLine($"{CurrentPlayer} now has {CurrentPlayer.Purse} Gold Coins.");

            var winner = !CurrentPlayer.HasWon();
            AdvanceToNextPlayer();

            return winner;
        }

        public bool WrongAnswer()
        {
            _output.WriteLine("Question was incorrectly answered");
            _output.WriteLine(CurrentPlayer + " was sent to the penalty box");
            CurrentPlayer.SendToPenaltyBox();

            AdvanceToNextPlayer();

            return true;
        }

        public bool Roll(int roll)
        {
            var playerName = CurrentPlayer.Name;

            _output.WriteLine($"{playerName} is the current player");
            _output.WriteLine($"They have rolled a {roll}");

            if (CurrentPlayer.IsInPenaltyBox)
            {
                if (roll % 2 == 0)
                {
                    _output.WriteLine($"{playerName} is not getting out of the penalty box");
                    return false;
                }

                _output.WriteLine($"{playerName} is getting out of the penalty box");
            }

            CurrentPlayer.Move(roll);

            _output.WriteLine($"{playerName}'s new location is {CurrentPlayer.Place}");
            _output.WriteLine($"The category is {CurrentCategory()}");

            AskQuestion();
            return true;
        }

        private void AskQuestion()
        {
            var category = CurrentCategory();
            var question = _questionDeck.NextQuestion(category);
            _output.WriteLine(question);
        }

        private Category CurrentCategory()
        {
            return CategoriesByPlace[CurrentPlayer.Place];
        }

        private void AdvanceToNextPlayer()
        {
            _players.Advance();
        }
    }
}