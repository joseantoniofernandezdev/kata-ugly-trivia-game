using System.Collections.Generic;
using Trivia.Enums;
using Trivia.Interfaces;
using Trivia.Models;

namespace Trivia
{
    public class Game
    {
        private readonly IGameOutput _output;
        private readonly List<Player> _players = new();
        private Player CurrentPlayer => _players[_currentPlayer];
        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;
        private readonly QuestionDeck _questionDeck;

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
            if (CurrentPlayer.IsInPenaltyBox && !_isGettingOutOfPenaltyBox)
            {
                AdvanceToNextPlayer();
                return true;
            }

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

        public void Roll(int roll)
        {
            var playerName = CurrentPlayer.Name;
            _output.WriteLine($"{playerName} is the current player");
            _output.WriteLine($"They have rolled a {roll}");

            bool moved = CurrentPlayer.TakeTurn(roll, _isGettingOutOfPenaltyBox);

            if (CurrentPlayer.IsInPenaltyBox && !moved)
            {
                _output.WriteLine($"{playerName} is not getting out of the penalty box");
                _isGettingOutOfPenaltyBox = false;
                return;
            }

            if (CurrentPlayer.IsInPenaltyBox && moved)
            {
                _output.WriteLine($"{playerName} is getting out of the penalty box");
                _isGettingOutOfPenaltyBox = true;
            }

            _output.WriteLine($"{playerName}'s new location is {CurrentPlayer.Place}");
            _output.WriteLine($"The category is {CurrentCategory()}");
            AskQuestion();
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
            _currentPlayer++;
            if (_currentPlayer == _players.Count)
                _currentPlayer = 0;
        }
    }
}