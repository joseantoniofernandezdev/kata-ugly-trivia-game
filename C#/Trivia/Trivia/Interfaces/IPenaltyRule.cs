using Trivia.Models;

namespace Trivia.Interfaces;

public interface IPenaltyRule
{
    bool CanAnswer(Player player, int roll);
}
