using Trivia.Interfaces;
using Trivia.Models;

namespace Trivia.Rules;

public class PenaltyRule : IPenaltyRule
{
    public bool CanAnswer(Player player, int roll)
    {
        if (!player.IsInPenaltyBox)
            return true;

        return roll % 2 != 0;
    }
}
