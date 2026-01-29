using ApprovalTests;
using ApprovalTests.Reporters;
using System;
using System.IO;
using Trivia;
using Trivia.Adapters;
using Trivia.Rules;
using Xunit;

namespace Tests;

[UseReporter(typeof(DiffReporter))]
public class GameRunnerShould
{
    [Fact]
    public void Check_game_runner_console_output()
    {
        string gameOutput = null;
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            var aGame = new Game(new ConsoleGameOutput(), new PenaltyRule());
            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            var gameRunner = new GameRunner();
            gameRunner.Run(aGame, new RandomMock());

            gameOutput = sw.ToString();
            sw.Flush();
        }

        Approvals.Verify(gameOutput);
    }

    private class RandomMock : Random
    {
        public override int Next(int maxValue)
        {
            return 2;
        }
    }
}