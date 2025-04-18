using LookUps;
using ParseQ.Dto;
using ParseQDto;

namespace Parse;

public class ParseTextToQuestions
{
    QuestionTypes questionType;
    
    /// <summary>
    ///  Parse the text into a list of questions. 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<List<Question>> BuildQuestionList(string text)
    {
        var questions = new List<Question>();
        var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        
        // Remove non-quiz text
        lines = RemoveNonQuizText(lines).ToArray();
        
        // Loop through each line and parse the questions
        // todo: this first pass is for multiple choice only, we will update this to handle the other question types later
        foreach (var line in lines)
        {
            
        }

        return questions;
    }

    private void SetQuestionType(string text)
    {
        // if the text starts and ends with two asterisks, it will contain the question type
        var questionTypeText = text.TrimStart('*').TrimEnd('*');

        questionType = questionTypeText switch
        {
            QuestionTextConstants.MULTIPLE_CHOICE => QuestionTypes.MultipleChoice,
            QuestionTextConstants.FILL_IN_THE_BLANK => QuestionTypes.FillInTheBlank,
            QuestionTextConstants.MATCH_TERMS => QuestionTypes.MatchingTerms,
        };
    }

    private List<string> RemoveNonQuizText(string[] lines)
    {
        var result = new List<string>();
        var count = 0;

        foreach (var line in lines)
        {
            if (line.Contains(MarkdownConstants.CODE_BLOCK))
            {
                count++;
                if (count == 1) continue;
            }

            if (count == 2) break;

            if (count > 0)
            {
                result.Add(line);
            }
        }

        return result;
    }
}