using Trivia.Enums;
using Trivia.Interfaces;
using Trivia.Models;

namespace Trivia
{
    public class Game
    {
        private readonly IGameOutput _gameOutput;
        private readonly IPenaltyRule _penaltyRule;
        private readonly PlayerQueue _players;
        private readonly QuestionDeck _questionDeck;
        private Player CurrentPlayer => _players.Current;
        private static readonly Category[] Categories = { Category.Pop, Category.Science, Category.Sports, Category.Rock };

        public Game(IGameOutput output, IPenaltyRule penaltyRule)
        {
            _gameOutput = output;
            _questionDeck = new QuestionDeck();
            _players = new PlayerQueue();
            _penaltyRule = penaltyRule;
        }

        public bool Add(string playerName)
        {
            _players.Add(new Player(playerName));

            _gameOutput.WriteLine(playerName + " was added");

            _gameOutput.WriteLine("They are player number " + _players.Count);

            return true;
        }

        public bool WasCorrectlyAnswered()
        {
            _gameOutput.WriteLine("Answer was corrent!!!!");

            CurrentPlayer.AddCoin();

            _gameOutput.WriteLine($"{CurrentPlayer} now has {CurrentPlayer.Purse} Gold Coins.");

            var winner = !CurrentPlayer.HasWon();

            AdvanceToNextPlayer();

            return winner;
        }

        public bool WrongAnswer()
        {
            _gameOutput.WriteLine("Question was incorrectly answered");

            _gameOutput.WriteLine(CurrentPlayer + " was sent to the penalty box");

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

            _gameOutput.WriteLine(question);
        }

        private Category CurrentCategory() => Categories[CurrentPlayer.Place % Categories.Length];

        private void AdvanceToNextPlayer() => _players.Advance();

        private void LogPlayerTurn(int roll)
        {
            var playerName = CurrentPlayer.Name;

            _gameOutput.WriteLine($"{playerName} is the current player");

            _gameOutput.WriteLine($"They have rolled a {roll}");
        }

        private void LogPenaltyBox(bool gotOut)
        {
            var playerName = CurrentPlayer.Name;

            _gameOutput.WriteLine(gotOut
                ? $"{playerName} is getting out of the penalty box"
                : $"{playerName} is not getting out of the penalty box");
        }

        private void LogPlayerPosition()
        {
            var playerName = CurrentPlayer.Name;

            _gameOutput.WriteLine($"{playerName}'s new location is {CurrentPlayer.Place}");

            _gameOutput.WriteLine($"The category is {CurrentCategory()}");
        }
    }
}