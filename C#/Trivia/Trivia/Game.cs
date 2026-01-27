using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];
        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly List<string> _players = new();
        private readonly Dictionary<Category, LinkedList<string>> _questions = new();

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
            _players.Add(playerName);

            var playerIndex = HowManyPlayers() - 1;

            _places[playerIndex] = 0;
            _purses[playerIndex] = 0;
            _inPenaltyBox[playerIndex] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    _purses[_currentPlayer]++;
                    Console.WriteLine(_players[_currentPlayer]
                            + " now has "
                            + _purses[_currentPlayer]
                            + " Gold Coins.");

                    var winner = DidPlayerWin();

                    AdvanceToNextPlayer();

                    return winner;
                }
                else
                {
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Answer was corrent!!!!");
                _purses[_currentPlayer]++;
                Console.WriteLine(_players[_currentPlayer]
                        + " now has "
                        + _purses[_currentPlayer]
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
            _inPenaltyBox[_currentPlayer] = true;

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

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
                    MoveCurrentPlayer(roll);

                    Console.WriteLine(_players[_currentPlayer]
                            + "'s new location is "
                            + _places[_currentPlayer]);
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
                        + _places[_currentPlayer]);
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
            if (_places[_currentPlayer] == 0) return Category.Pop;
            if (_places[_currentPlayer] == 4) return Category.Pop;
            if (_places[_currentPlayer] == 8) return Category.Pop;
            if (_places[_currentPlayer] == 1) return Category.Science;
            if (_places[_currentPlayer] == 5) return Category.Science;
            if (_places[_currentPlayer] == 9) return Category.Science;
            if (_places[_currentPlayer] == 2) return Category.Sports;
            if (_places[_currentPlayer] == 6) return Category.Sports;
            if (_places[_currentPlayer] == 10) return Category.Sports;
            return Category.Rock;
        }

        private bool DidPlayerWin()
        {
            return !(_purses[_currentPlayer] == 6);
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
            _places[_currentPlayer] += roll;
            if (_places[_currentPlayer] > 11)
                _places[_currentPlayer] -= 12;
        }
    }
}