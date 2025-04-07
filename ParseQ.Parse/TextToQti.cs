using System.Text.RegularExpressions;

namespace ParseQ.Parse;

public class TextToQti
{
    private string _qtiXml = string.Empty;
    
    public static void ConvertTextToQti(string text)
    {
        // Split the text into lines
        //string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Initialize the QTI XML structure
        // string qtiXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
        // qtiXml += "<assessmentItem xmlns=\"http://www.imsglobal.org/xsd/imsg\">\n";
        // qtiXml += "  <responseDeclaration identifier=\"RESPONSE\" cardinality=\"single\" baseType=\"string\"/>\n";
        // qtiXml += "  <itemBody>\n";

        // Process each line and convert to QTI format
        // foreach (string line in lines)
        // {
        //     qtiXml += $"    <p>{line}</p>\n"; // Wrap each line in a paragraph tag
        // }
        //
        // // qtiXml += "  </itemBody>\n";
        // // qtiXml += "</assessmentItem>";
        //
        // return qtiXml;
    }

    public async Task<string> ConvertTextToQtiAsync(Stream textStream, string filePath)
    {
        _qtiXml = BuildQtiXml(filePath);
        using var reader = new StreamReader(textStream);
        while(!reader.EndOfStream)
        {
            // Read each line from the stream
            var line = (await reader.ReadLineAsync())?.Trim();
            if (string.IsNullOrEmpty(line))
                continue;
            
            // if the line starts with a number followed by a period, it is a question
            var firstCharacter = line[0];
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

            return null;
        }

        return null;
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
    
    private void BuildQtiXml(string fileName)
    {
        // Build the QTI XML structure
        _qtiXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
        _qtiXml += "<questestinterop>\n";
        _qtiXml += "<assessment title=\"fileName\">\n";
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