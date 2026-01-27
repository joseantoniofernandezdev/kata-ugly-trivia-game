namespace Trivia.Models;

public class Player
{
    public string Name { get; }
    public int Place { get; private set; }
    public int Purse { get; private set; }
    public bool IsInPenaltyBox { get; private set; }

    public Player(string name)
    {
        Name = name;
        Place = 0;
        Purse = 0;
        IsInPenaltyBox = false;
    }

    public override string ToString() => Name;

    public void Move(int roll)
    {
        Place = (Place + roll) % 12;
    }

    public void AddCoin() => Purse++;

    public bool HasWon() => Purse >= 6;

    public void SendToPenaltyBox() => IsInPenaltyBox = true;

    public void GetOutOfPenaltyBox() => IsInPenaltyBox = false;

    public bool TakeTurn(int roll, bool isGettingOutOfPenaltyBox)
    {
        if (IsInPenaltyBox && !isGettingOutOfPenaltyBox)
            return false;

        Move(roll);
        return true;
    }
}