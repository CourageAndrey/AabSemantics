using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
    public interface ILanguageErrorsInventor
    {
        string UnknownQuestion
        { get; }
    }

    public class LanguageErrorsInventor : ILanguageErrorsInventor
    {
        #region Properties

        [XmlElement]
        public string UnknownQuestion
        { get; set; }

        #endregion

        internal static LanguageErrorsInventor CreateDefault()
        {
            return new LanguageErrorsInventor
            {
                UnknownQuestion = "Неизвестный тип вопроса.",
            };
        }
    }
}
