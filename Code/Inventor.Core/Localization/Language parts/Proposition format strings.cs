using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
    public interface ILanguagePropositionFormatStrings
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

    public class LanguagePropositionFormatStrings : ILanguagePropositionFormatStrings
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

        internal static LanguagePropositionFormatStrings CreateDefaultTrue()
        {
            return new LanguagePropositionFormatStrings
            {
                SubjectArea = string.Format("Понятие {0} входит в предметную область {1}.", Strings.ParamConcept, Strings.ParamArea),
                Clasification = string.Format("{0} есть {1}.", Strings.ParamChild, Strings.ParamParent),
                HasSign = string.Format("{0} имеет признак {1}.", Strings.ParamConcept, Strings.ParamSign),
                SignValue = string.Format("{0} имеет значение признака {1} равным {2}.", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
                Composition = string.Format("{0} является частью {1}.", Strings.ParamChild, Strings.ParamParent),
            };
        }

        internal static LanguagePropositionFormatStrings CreateDefaultFalse()
        {
            return new LanguagePropositionFormatStrings
            {
                SubjectArea = string.Format("Понятие {0} не входит в предметную область {1}.", Strings.ParamConcept, Strings.ParamArea),
                Clasification = string.Format("{0} не есть {1}.", Strings.ParamChild, Strings.ParamParent),
                HasSign = string.Format("У {0} отсутствует признак {1}.", Strings.ParamConcept, Strings.ParamSign),
                SignValue = string.Format("{0} не имеет значение признака {1} равным {2}.", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
                Composition = string.Format("{0} не является частью {1}.", Strings.ParamChild, Strings.ParamParent),
            };
        }

        internal static LanguagePropositionFormatStrings CreateDefaultQuestion()
        {
            return new LanguagePropositionFormatStrings
            {
                SubjectArea = string.Format("Входит ли понятие {0} в предметную область {1}?", Strings.ParamConcept, Strings.ParamArea),
                Clasification = string.Format("На самом ли деле {0} есть {1}?", Strings.ParamChild, Strings.ParamParent),
                HasSign = string.Format("Есть ли у {0} признак {1}?", Strings.ParamConcept, Strings.ParamSign),
                SignValue = string.Format("Является ли {2} значением признака {1} у {0}?", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
                Composition = string.Format("Является ли {0} частью {1}?", Strings.ParamChild, Strings.ParamParent),
            };
        }
    }
}
