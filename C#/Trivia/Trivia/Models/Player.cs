namespace Trivia.Models;

public class Player
{
    public string Name { get; }
    public int Place { get; set; }
    public int Purse { get; set; }
    public bool IsInPenaltyBox { get; set; }

    public Player(string name)
    {
        Name = name;
        Place = 0;
        Purse = 0;
        IsInPenaltyBox = false;
    }

    public override string ToString()
    {
        return Name;
    }
}