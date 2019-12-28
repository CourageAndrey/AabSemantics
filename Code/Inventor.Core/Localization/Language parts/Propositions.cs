using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
    public interface ILanguageStatements
    {
        string SubjectArea
        { get; }

        string Clasification
        { get; }

        string HasSign
        { get; }

        string SignValue
        { get; }

        string Composition
        { get; }
    }

    public class LanguageStatements : ILanguageStatements
    {
        #region Properties

        [XmlElement]
        public string SubjectArea
        { get; set; }

        [XmlElement]
        public string Clasification
        { get; set; }

        [XmlElement]
        public string HasSign
        { get; set; }

        [XmlElement]
        public string SignValue
        { get; set; }

        [XmlElement]
        public string Composition
        { get; set; }

        #endregion

        public static LanguageStatements CreateDefaultNames()
        {
            return new LanguageStatements
            {
                SubjectArea = "Предметная область",
                Clasification = "Классификация",
                HasSign = "Признак",
                SignValue = "Значение признака",
                Composition = "Часть-целое",
            };
        }

        internal static LanguageStatements CreateDefaultHints()
        {
            return new LanguageStatements
            {
                SubjectArea = "Отношение описывает вхождение терминов в некоторую предметную область.",
                Clasification = "Отношение описывает связь между родительским и дочерним (подчинённым) понятиями.",
                HasSign = "Отношение описывает наличие некоторого признака, описывающего свойства понятия.",
                SignValue = "Отношение описывает значение признака, свойственного данному понятию.",
                Composition = "Отношение описывает вхождение одного понятия в другое качестве элемента структуры последнего.",
            };
        }
    }
}
