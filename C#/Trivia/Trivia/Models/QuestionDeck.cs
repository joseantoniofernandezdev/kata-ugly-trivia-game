using System.Collections.Generic;
using System.Linq;
using Trivia.Enums;

namespace Trivia.Models;

public class QuestionDeck
{
    private readonly Dictionary<Category, LinkedList<string>> _questions;

    public QuestionDeck()
    {
        _questions = new Dictionary<Category, LinkedList<string>>
            {
                { Category.Pop, new LinkedList<string>() },
                { Category.Science, new LinkedList<string>() },
                { Category.Sports, new LinkedList<string>() },
                { Category.Rock, new LinkedList<string>() }
            };

        Enumerable.Range(0, 50).ToList().ForEach(i =>
        {
            foreach (var category in _questions.Keys)
            {
                _questions[category].AddLast($"{category} Question {i}");
            }
        });
    }

    public string NextQuestion(Category category)
    {
        var question = _questions[category].First();
        _questions[category].RemoveFirst();
        return question;
    }
}