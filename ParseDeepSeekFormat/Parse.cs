using System.Text.RegularExpressions;

namespace ParseDeepSeekFormat;

public class Parse
{
    private string _qtiXml; //= "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
    
    public Parse()
    {
        _qtiXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
        _qtiXml =
            @"<questestinterop
xmlns=""http://www.imsglobal.org/xsd/ims_qtiasiv1p2""
xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
xsi:schemaLocation=""http://www.imsglobal.org/xsd/ims_qtiasiv1p2 http://www.imsglobal.org/xsd/ims_qtiasiv1p2p1.xsd"">";
    }

    public async Task<bool> ParseTextFile(string text, string quizName)
    {
        // Split the text into lines
        string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        // name the file to be used in the QTI XML
        SetQtiQuizName(quizName);

        return true;
    }

    public async Task<string> ConvertTextToQtiAsync(string questionText, string fileName)
    {
        
        SetQtiQuizName(fileName);
        SetMetaData();
        AddSection();
        var questions = new List<string>();
        
        try
        {
            // using var reader = new StreamReader(textStream);
            // string line;
            foreach(var line in questionText.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
            {
                // Read each line from the stream
                // var line = (await reader.ReadLineAsync())?.Trim();
                if (string.IsNullOrEmpty(line))
                    continue;
            
                // if the line starts with a number followed by a period, it is a question
                var firstCharacter = line.TrimStart()[0];
                if (char.IsDigit(firstCharacter) && line[1] == '.')
                {
                    AddQuestion(line);
                }
            
                // if the line starts with a letter, or an asterisk followed by a closed parens, it is an answer
                var matches = Regex.Matches(line, @"[a-zA-Z]\)");
    
                foreach (Match match in matches)
                {
                    // this is a possible answer, so add to the answer section
                    AddAnswer(match.Value);
                }
            
                // if the line starts with an asterisk, it is a correct answer
    
            }
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync(ex.Message);
        }
    
        return _qtiXml;
    }
    
    public async Task ParseQuestionBank(string questionText, string quizName)
    {
        // read the question text file and find the multiple choice questions
        // between the line that starts with ### and contains Multiple and Choice
        // and --- before the next // line containing ### 
        
        // find the first line that starts with ### and contains "Multiple" and "Choice"
        var startIndex = questionText.IndexOf("###", StringComparison.Ordinal);
        var endIndex = questionText.IndexOf("---", startIndex, StringComparison.Ordinal);
        
    }

    private void AddQuestion(string line)
    {
        // Extract the question number and text
        var questionNumber = line[0];
        _qtiXml += $"<item indent=\"Q{questionNumber}\" title=\"Question {questionNumber}\">\n";
        _qtiXml += "<material>\n";
        _qtiXml += $"<mattext>{line}</mattext>\n";
        _qtiXml += "</material>\n";
    }

    private void AddAnswerSection(int questionNumber)
    {
        // this is where we add the answers to the question. Each answer is a <responseLabel> element, inside a
        // render_choice element, which is inside a <responseLid> element
        _qtiXml += $"<responseLid ident=\"response{questionNumber}\">\n";
        _qtiXml += $"<render_choice>\n";
    }
    
    private void AddAnswer(string line)
    {
        // Extract the answer text
        
    }
    
    private void SetQtiQuizName(string quizName)
    {
        // Build the QTI XML structure
        var fileName = string.Concat(quizName, ".xml");
        _qtiXml += $"/r/n<assessment title=\"{fileName}\">\n";
    }

    private void SetMetaData()
    {
        _qtiXml += $"<qtimetadata>\n";
        _qtiXml += $"  <qtimetadatafield>\n";
        _qtiXml += $"    <fieldlabel>cc_maxattempts</fieldlabel>\n";
        _qtiXml += $"    <fieldentry>2</fieldentry>\n";
        _qtiXml += $"  </qtimetadatafield>\n";
        _qtiXml += $"<qtimetadata>\n";
    }
    
    private void AddSection()
    {
        _qtiXml += "<section>\n";
    }
    
    private void CompleteQtiXml()
    {
        // Complete the QTI XML structure
        _qtiXml += "  </section>\n";
        _qtiXml += "</assessmentItem>";
        _qtiXml += "</questestinterop>";
    }
}