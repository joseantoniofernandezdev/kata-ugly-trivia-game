using Trivia.Enums;
using Trivia.Interfaces;
using Trivia.Models;

namespace Trivia
{
    public class Game
    {
        private readonly IGameOutput _output;
        private readonly IPenaltyRule _penaltyRule;
        private readonly PlayerQueue _players;
        private readonly QuestionDeck _questionDeck;
        private Player CurrentPlayer => _players.Current;
        private static readonly Category[] Categories = { Category.Pop, Category.Science, Category.Sports, Category.Rock };

        public Game(IGameOutput output, IPenaltyRule penaltyRule)
        {
            _output = output;
            _questionDeck = new QuestionDeck();
            _players = new PlayerQueue();
            _penaltyRule = penaltyRule;
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
            LogPlayerTurn(roll);

            bool canAnswer = _penaltyRule.CanAnswer(CurrentPlayer, roll);

            if (CurrentPlayer.IsInPenaltyBox)
                LogPenaltyBox(canAnswer);

            if (!canAnswer)
                return false;

            CurrentPlayer.Move(roll);
            LogPlayerPosition();
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
            return Categories[CurrentPlayer.Place % Categories.Length];
        }

        private void AdvanceToNextPlayer()
        {
            _players.Advance();
        }

        private void LogPlayerTurn(int roll)
        {
            var playerName = CurrentPlayer.Name;
            _output.WriteLine($"{playerName} is the current player");
            _output.WriteLine($"They have rolled a {roll}");
        }

        private void LogPenaltyBox(bool gotOut)
        {
            var playerName = CurrentPlayer.Name;
            _output.WriteLine(gotOut
                ? $"{playerName} is getting out of the penalty box"
                : $"{playerName} is not getting out of the penalty box");
        }

        private void LogPlayerPosition()
        {
            var playerName = CurrentPlayer.Name;
            _output.WriteLine($"{playerName}'s new location is {CurrentPlayer.Place}");
            _output.WriteLine($"The category is {CurrentCategory()}");
        }
    }
}