using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
    public interface ILanguagePropositions
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

    public class LanguagePropositions : ILanguagePropositions
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

        public static LanguagePropositions CreateDefaultNames()
        {
            return new LanguagePropositions
            {
                SubjectArea = "Предметная область",
                Clasification = "Классификация",
                HasSign = "Признак",
                SignValue = "Значение признака",
                Composition = "Часть-целое",
            };
        }

        internal static LanguagePropositions CreateDefaultHints()
        {
            return new LanguagePropositions
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
