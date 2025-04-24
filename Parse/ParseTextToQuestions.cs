using LookUps;
using ParseQ.Dto;
using ParseQDto;

namespace Parse;

public class ParseTextToQuestions
{
    private QuestionTypes _questionType;
    private Question? _question;
    
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
            if(line.StartsWith("**") && line.EndsWith("**"))
            {
                // this is a question type
                SetQuestionType(line);
                continue;
            }
            
            if (line.StartsWith(">"))
            {
                // this is a new Question, add the last question and answers to the list 
                if (_question != null)
                {
                    questions.Add(_question);
                }
                
                // intialize the next Question
                _question = new Question();
                
                _question.QuestionType = _questionType.ToText();
                // todo: change this to set the points dynamically if required
                _question.PointsPossible = 1;
                
                SetQuestionText(line);
            }
            else if (line.StartsWith('='))
            {
                // this is a possible answer, so add to the answer section
                SetAnswerText(line);
            }
            else if (line.StartsWith(QuestionTextConstants.CORRECT_ANSWER))
            {
                SetCorrectAnswer(line);
            }
        }

        return questions;
    }

    private void SetCorrectAnswer(string line)
    {
        // Correct answers are prepended with the string "Answer"
        // Extract the correct answer text
        int index;
        if (line.Contains(")"))
        {
            // there is whitespace between the parenthesis and the start of the answer
            index = line.IndexOf(')') + 2;
        }
        else
        {
            // there is whitespace between the parenthesis and the start of the answer
            index = line.IndexOf(':') + 2;
        }
        var correctAnswerText = line.Substring(index).Trim();

        // Set the CorrectAnswer property of the question
        _question.CorrectAnswer = correctAnswerText;

        // Find the matching answer and set its IsCorrect property to true
        var correctAnswer = _question.Answers?.FirstOrDefault(x => x.Text.Contains(correctAnswerText));
        if (correctAnswer != null)
        {
            correctAnswer.IsCorrect = true;
        }
        
    }

    private void SetAnswerText(string line)
    {
        // the answer text starts with a letter and a left parenthesis. We can strip these characters
        // as they are not used in the XML document
        var startIndex = line.IndexOf(')');
        var answerText = line.Substring(startIndex + 1).TrimStart();

        _question.Answers ??= new List<Answer>();
        
        _question.Answers.Add(new Answer
        {
            Text = answerText
        });
    }

    private void SetQuestionText(string line)
    {
        // remove the leading > and trailing whitespace
        var text = line.TrimStart('>').TrimStart();
        
        _question.Text = text;
    }

    private void SetQuestionType(string text)
    {
        // if the text starts and ends with two asterisks, it will contain the question type
        var questionTypeText = text.TrimStart('*').TrimEnd('*');

        _questionType = questionTypeText switch
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

    private string RemoveMetaText(string line)
    {
        var trimmedText = RemoveMetaText(line);

        string text = string.Empty;
        if (line.StartsWith("**") && line.EndsWith("**"))
        {
            text = line.TrimStart('*').TrimEnd('*');
        }
        
        return text;
    }
    
}