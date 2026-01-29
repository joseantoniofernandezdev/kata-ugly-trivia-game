using System.Collections.Generic;

namespace Trivia.Models;

public class PlayerQueue
{
    private readonly List<Player> _players = new();
    private int _currentIndex;

    public Player Current => _players[_currentIndex];

    public int Count => _players.Count;

    public void Add(Player player)
    {
        _players.Add(player);
    }

    public void Advance()
    {
        _currentIndex = (_currentIndex + 1) % _players.Count;
    }
}