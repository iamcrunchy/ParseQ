using System.Xml.Linq;

namespace Interfaces;

public interface IAddQuestion
{
    /// <summary>
    ///     This method is used to add a question item to the XML document.
    ///     Standard attributes are added to the question item: ident and title.
    /// </summary>
    /// <returns>XElement - contains the question identifier and title</returns>
    XElement AddQuestionItem();

    /// <summary>
    ///     When calling the ToString() method, the XML declaration is omitted by default.
    ///     Saving the file to a StringWriter will ensure the XML declaration is included.
    /// </summary>
    /// <param name="qtiXml"></param>
    /// <returns>XDocument - qtiXml document with the XML declaration</returns>
    string AddXmlDeclaration(XDocument qtiXml);
}