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

    public void Move(int roll)
    {
        Place += roll;
        if (Place > 11) Place -= 12;
    }

    public void SendToPenaltyBox()
    {
        IsInPenaltyBox = true;
    }

    public void GetOutOfPenaltyBox()
    {
        IsInPenaltyBox = false;
    }

    public void AddCoin()
    {
        Purse++;
    }

    public bool HasWon()
    {
        return Purse >= 6;
    }
}