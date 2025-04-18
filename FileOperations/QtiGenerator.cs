﻿using System.Xml.Linq;
using ParseQ.Dto;
using ParseQDto;

namespace FileOperations;

public class QtiGenerator
{
    public XDocument GenerateQtiXml(List<Question> questions, string title)
    {
        var qtiXml = new XDocument(
            new XElement("assessmentItem",
                new XAttribute("xmlns", "http://www.imsglobal.org/xsd/imsg"),
                new XAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute("xsi:schemaLocation", "http://www.imsglobal.org/xsd/imsg assessmentItem_v1p2.xsd"),
                new XElement("itemBody",
                    new XElement("choiceInteraction",
                        new XAttribute("responseIdentifier", "RESPONSE"),
                        new XAttribute("maxChoices", "1"),
                        questions.Select(q => 
                            new XElement("simpleChoice",
                                new XAttribute("identifier", q.CorrectAnswer),
                                q.Text
                            )
                        )
                    )
                )
            )
        );
        
        return new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement("questestinterop", qtiXml)
            );
    }

    public void SetMetaData(XDocument qtiXml, string title,
        int maxAttempts = 2)
    {
        var assessment = qtiXml.Element("assessment");
        if (assessment != null)
        {
            assessment.Add(
                new XElement("qtimetadata",
                    new XElement("qtimetadatafield",
                        new XElement("fieldLabel", "maxAttempts"),
                        new XElement("fieldentry", "2")
                    )
                )
            );
            
            AddRootSection(assessment);
        }
    }

    private void AddRootSection(XElement assessment)
    {
        var rootSection = new XElement("section",
            new XAttribute("ident", "root_section"),
            new XAttribute("title", "Root Section")
        );
        
        assessment.Add(rootSection);
    }
    
    public void AddQuestionMetaData(XDocument qtiXml, Question question)
    {
        var section = qtiXml.Descendants("section").FirstOrDefault();

        if (section != null)
        {
            section.Add(new XElement("item",
                new XAttribute("title", question.Title),
                new XElement("itemmetadata",
                    new XElement("qtimetadata",
                        new XElement("qtimetadatafield",
                            new XElement("fieldlabel", "question_type"),
                            new XElement("fieldentry", question.QuestionType)
                        ),
                        new XElement("qtimetadatafield",
                            new XElement("fieldlabel", "points_possible"),
                            new XElement("fieldentry", question.PointsPossible))
                        // new XElement("qtimetadatafield",
                        //     new XElement("fieldlabel", "original_answer_ids"),
                        //     new XElement("fieldentry", "update when you get the ids, if you get them")),
                        // new XElement("qtimetadatafield",
                        //     new XElement("fieldlabel", "assessment_question_identifierref"),
                        //     new XElement("fieldentry", "update when you get the identifier, if you get it"))
                    )
                )
            ));
        }
    }
}