using Moq;
using Trivia.Interfaces;
using Trivia.Models;
using Xunit;

namespace Trivia.Unit;

public class GameShould
{
    private readonly Mock<IGameOutput> _outputMock;
    private readonly Mock<IPenaltyRule> _penaltyRuleMock;
    private readonly Game _sut;

    public GameShould()
    {
        _outputMock = new Mock<IGameOutput>();
        _penaltyRuleMock = new Mock<IPenaltyRule>();
        _sut = new Game(_outputMock.Object, _penaltyRuleMock.Object);
    }

    [Fact]
    public void AddPlayer_IncreasesPlayerCount_AndLogs()
    {
        // Arrange
        var playerName = "TestPlayer1";

        // Act
        var result = _sut.Add(playerName);

        // Assert
        Assert.True(result);
        AssertLogContains($"{playerName} was added");
        AssertLogContains("They are player number 1");
    }

    [Fact]
    public void Roll_PlayerInPenalty_CannotAnswer_LogsCorrectly()
    {
        // Arrange
        _sut.Add("TestPlayer2");
        _sut.WrongAnswer();
        _penaltyRuleMock
            .Setup(r => r.CanAnswer(It.IsAny<Player>(), It.IsAny<int>()))
            .Returns(false);

        // Act
        var result = _sut.Roll(2);

        // Assert
        Assert.False(result);
        AssertLogContains("not getting out of the penalty box");
    }

    [Fact]
    public void Roll_PlayerCanAnswer_MovesPlayerAndLogs()
    {
        // Arrange
        _sut.Add("TestPlayer3");
        _penaltyRuleMock
            .Setup(r => r.CanAnswer(It.IsAny<Player>(), It.IsAny<int>()))
            .Returns(true);

        // Act
        var result = _sut.Roll(3);

        // Assert
        Assert.True(result);
        AssertLogContains("new location is");
        AssertLogContains("The category is");
    }

    [Fact]
    public void WasCorrectlyAnswered_IncreasesCoins_AndLogs()
    {
        // Arrange
        _sut.Add("TestPlayer4");

        // Act
        var result = _sut.WasCorrectlyAnswered();

        // Assert
        Assert.True(result);
        AssertLogContains("Answer was corrent!!!!");
        AssertLogContains("now has 1 Gold Coins");
    }

    [Fact]
    public void WrongAnswer_SendsPlayerToPenaltyBox_AndLogs()
    {
        // Arrange
        _sut.Add("TestPlayer5");

        // Act
        var result = _sut.WrongAnswer();

        // Assert
        Assert.True(result);
        AssertLogContains("Question was incorrectly answered");
        AssertLogContains("was sent to the penalty box");
    }

    private void AssertLogContains(string expectedMessage)
    {
        _outputMock.Verify(o => o.WriteLine(It.Is<string>(s => s.Contains(expectedMessage))), Times.Once);
    }
}