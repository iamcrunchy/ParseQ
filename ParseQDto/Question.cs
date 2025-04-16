using ParseQDto;

namespace ParseQ.Dto;

public class Question
{
    public string? Title { get; set; }
    public string? QuestionType { get; set; }
    public string? Text { get; set; }
    public List<Answer>? Answers { get; set; }
    public string? CorrectAnswer { get; set; }
    public int? PointsPossible { get; set; }
}