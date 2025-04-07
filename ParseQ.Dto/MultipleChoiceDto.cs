namespace ParseQ.Dto;

public class MultipleChoiceDto
{
    public int Id { get; set; }
    public string Question { get; set; }
    public IList<string> Answers { get; set; }
    public bool CorrectAnswer { get; set; }
}