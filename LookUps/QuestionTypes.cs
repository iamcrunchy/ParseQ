namespace LookUps;

public enum QuestionTypes
{
    MultipleChoice = 1,
    FillInTheBlank = 2,
    MatchingTerms = 3
}

public static class QuestionTypesExtensions
{
    public static string ToText(this QuestionTypes questionType)
    {
        return questionType switch
        {
            QuestionTypes.MultipleChoice => "multiple_choice_question",
            QuestionTypes.FillInTheBlank => "fill_in_the_blank",
            QuestionTypes.MatchingTerms => "matching_terms",
            _ => throw new ArgumentOutOfRangeException(nameof(questionType), questionType, null)
        };
    }
}