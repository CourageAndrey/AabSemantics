using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
    public interface ILanguageMisc
    {
        string NameKnowledgeBase
        { get; }

        string NameConcept
        { get; }

        string NameStatement
        { get; }

        string NameCategoryConcepts
        { get; }

        string NameCategoryStatements
        { get; }

        string StrictEnumeration
        { get; }

        string ClasificationSign
        { get; }

        string NewKbName
        { get; }

        string Rules
        { get; }

        string Answer
        { get; }

        string Required
        { get; }

        string CheckResult
        { get; }

        string CheckOk
        { get; }

        string DialogKbOpenTitle
        { get; }

        string DialogKbSaveTitle
        { get; }

        string DialogKbFileFilter
        { get; }

        string ConsistencyErrorDuplicate
        { get; }

        string ConsistencyErrorCyclic
        { get; }

        string ConsistencyErrorMultipleSubjectArea
        { get; }

        string ConsistencyErrorMultipleSign
        { get; }

        string ConsistencyErrorMultipleSignValue
        { get; }

        string ConsistencyErrorSignWithoutValue
        { get; }

        string True
        { get; }

        string False
        { get; }

        string TrueHint
        { get; }

        string FalseHint
        { get; }
    }

    public class LanguageMisc : ILanguageMisc
    {
        #region Properties

        [XmlElement]
        public string NameKnowledgeBase
        { get; set; }

        [XmlElement]
        public string NameConcept
        { get; set; }

        [XmlElement]
        public string NameStatement
        { get; set; }

        [XmlElement]
        public string NameCategoryConcepts
        { get; set; }

        [XmlElement]
        public string NameCategoryStatements
        { get; set; }

        [XmlElement]
        public string StrictEnumeration
        { get; set; }

        [XmlElement]
        public string ClasificationSign
        { get; set; }

        [XmlElement]
        public string NewKbName
        { get; set; }

        [XmlElement]
        public string Rules
        { get; set; }

        [XmlElement]
        public string Answer
        { get; set; }

        [XmlElement]
        public string Required
        { get; set; }

        [XmlElement]
        public string CheckResult
        { get; set; }

        [XmlElement]
        public string CheckOk
        { get; set; }

        [XmlElement]
        public string DialogKbOpenTitle
        { get; set; }

        [XmlElement]
        public string DialogKbSaveTitle
        { get; set; }

        [XmlElement]
        public string DialogKbFileFilter
        { get; set; }

        [XmlElement]
        public string ConsistencyErrorDuplicate
        { get; set; }

        [XmlElement]
        public string ConsistencyErrorCyclic
        { get; set; }

        [XmlElement]
        public string ConsistencyErrorMultipleSubjectArea
        { get; set; }

        [XmlElement]
        public string ConsistencyErrorMultipleSign
        { get; set; }

        [XmlElement]
        public string ConsistencyErrorMultipleSignValue
        { get; set; }

        [XmlElement]
        public string ConsistencyErrorSignWithoutValue
        { get; set; }

        [XmlElement]
        public string True
        { get; set; }

        [XmlElement]
        public string False
        { get; set; }

        [XmlElement]
        public string TrueHint
        { get; set; }

        [XmlElement]
        public string FalseHint
        { get; set; }

        #endregion

        internal static LanguageMisc CreateDefault()
        {
            return new LanguageMisc
            {
                NameKnowledgeBase = "База знаний",
                NameConcept = "Понятие",
                NameStatement = "Утверждение",
                NameCategoryConcepts = "Понятия",
                NameCategoryStatements = "Утверждения",
                StrictEnumeration = " и не может принимать другое значение",
                ClasificationSign = " по признаку \"{0}\"",
                NewKbName = "Новая база знаний",
                Rules = "Все правила базы знаний:",
                Answer = "Ответ:",
                Required = "обязательный",
                CheckResult = "Результат проверки",
                CheckOk = "В результате проверки ошибок не выявлено.",
                DialogKbOpenTitle = "Загрузка базы знаний",
                DialogKbSaveTitle = "Сохранение базы знаний",
                DialogKbFileFilter = "XML с базой знаний|*.xml",
                ConsistencyErrorDuplicate = "Дублирование отношения #STATEMENT#.",
                ConsistencyErrorCyclic = "Отношение #STATEMENT# приводит к циклической ссылке понятий друг на друга.",
                ConsistencyErrorMultipleSubjectArea = "Предметная область #AREA# задана более одного раза.",
                ConsistencyErrorMultipleSign = "#STATEMENT# приводит к повторному определению признака у понятия.",
                ConsistencyErrorMultipleSignValue = "Значение признака #SIGN# понятия #CONCEPT# не может быть корректно определено, так как задано в нескольких его предках.",
                ConsistencyErrorSignWithoutValue = "#STATEMENT# задаёт значение признака, который отсутствует у понятия.",
                True = "Да",
                False = "Нет",
                TrueHint = "Логическое значение: истина.",
                FalseHint = "Логическое значение: ложь.",
            };
        }
    }
}
