using System.Xml.Linq;

namespace Interfaces;

public interface IMultipleChoice : IAddQuestion
{
    // what are the nodes that we need to parse for a multiple-choice question?
    XElement AddQuestionItem();
}