using System;
using Trivia.Interfaces;

namespace Trivia.Adapters;

public class ConsoleGameOutput : IGameOutput
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}