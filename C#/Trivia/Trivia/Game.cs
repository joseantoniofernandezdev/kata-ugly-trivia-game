using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private readonly List<Player> _players = new();
        private readonly Dictionary<Category, LinkedList<string>> _questions = new();
        private Player CurrentPlayer => _players[_currentPlayer];

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game()
        {
            _questions[Category.Pop] = new LinkedList<string>();
            _questions[Category.Science] = new LinkedList<string>();
            _questions[Category.Sports] = new LinkedList<string>();
            _questions[Category.Rock] = new LinkedList<string>();

            for (var i = 0; i < 50; i++)
            {
                _questions[Category.Pop].AddLast(CreatePopQuestion(i));
                _questions[Category.Science].AddLast(CreateScienceQuestion(i));
                _questions[Category.Sports].AddLast(CreateSportsQuestion(i));
                _questions[Category.Rock].AddLast(CreateRockQuestion(i));
            }
        }

        public bool Add(string playerName)
        {
            _players.Add(new Player(playerName));

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public bool WasCorrectlyAnswered()
        {
            if (CurrentPlayer.IsInPenaltyBox)
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    CurrentPlayer.Purse++;
                    Console.WriteLine(_players[_currentPlayer]
                            + " now has "
                            + CurrentPlayer.Purse
                            + " Gold Coins.");

                    var winner = DidPlayerWin();

                    AdvanceToNextPlayer();

                    return winner;
                }
                else
                {
                    AdvanceToNextPlayer();
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Answer was corrent!!!!");
                CurrentPlayer.Purse++;
                Console.WriteLine(_players[_currentPlayer]
                        + " now has "
                        + CurrentPlayer.Purse
                        + " Gold Coins.");

                var winner = DidPlayerWin();

                AdvanceToNextPlayer();

                return winner;
            }
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            CurrentPlayer.IsInPenaltyBox = true;

            AdvanceToNextPlayer();

            return true;
        }

        private int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (CurrentPlayer.IsInPenaltyBox)
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
                    MoveCurrentPlayer(roll);

                    Console.WriteLine(_players[_currentPlayer]
                            + "'s new location is "
                            + CurrentPlayer.Place);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                MoveCurrentPlayer(roll);

                Console.WriteLine(_players[_currentPlayer]
                        + "'s new location is "
                        + CurrentPlayer.Place);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            var category = CurrentCategory();

            var questions = _questions[category];

            Console.WriteLine(questions.First());

            questions.RemoveFirst();
        }

        private Category CurrentCategory()
        {
            if (CurrentPlayer.Place == 0) return Category.Pop;
            if (CurrentPlayer.Place == 4) return Category.Pop;
            if (CurrentPlayer.Place == 8) return Category.Pop;
            if (CurrentPlayer.Place == 1) return Category.Science;
            if (CurrentPlayer.Place == 5) return Category.Science;
            if (CurrentPlayer.Place == 9) return Category.Science;
            if (CurrentPlayer.Place == 2) return Category.Sports;
            if (CurrentPlayer.Place == 6) return Category.Sports;
            if (CurrentPlayer.Place == 10) return Category.Sports;
            return Category.Rock;
        }

        private bool DidPlayerWin()
        {
            return !(CurrentPlayer.Purse == 6);
        }

        private static string CreatePopQuestion(int index) => "Pop Question " + index;

        private static string CreateScienceQuestion(int index) => "Science Question " + index;

        private static string CreateSportsQuestion(int index) => "Sports Question " + index;

        private static string CreateRockQuestion(int index) => "Rock Question " + index;

        private void AdvanceToNextPlayer()
        {
            _currentPlayer++;
            if (_currentPlayer == _players.Count)
                _currentPlayer = 0;
        }

        private void MoveCurrentPlayer(int roll)
        {
            CurrentPlayer.Place += roll;
            if (CurrentPlayer.Place > 11)
                CurrentPlayer.Place -= 12;
        }
    }
}