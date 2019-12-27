using System;

using System.Xml.Serialization;

namespace Sef.Localization
{
    public interface ILanguageEditor
    {
        String ModeCreate
        { get; }

        String ModeEdit
        { get; }

        String ModeView
        { get; }

        String DeletePromt
        { get; }

        String ClearPromt
        { get; }
    }

    [Serializable]
    public sealed class LanguageEditor : ILanguageEditor
    {
        #region Properties

        [XmlElement]
        public String ModeCreate
        { get; set; }

        [XmlElement]
        public String ModeEdit
        { get; set; }

        [XmlElement]
        public String ModeView
        { get; set; }

        [XmlElement]
        public String DeletePromt
        { get; set; }

        [XmlElement]
        public String ClearPromt
        { get; set; }

        #endregion

        internal static LanguageEditor CreateDefault()
        {
            return new LanguageEditor
            {
                ModeCreate = "Режим создания",
                ModeEdit = "Режим редактирования",
                ModeView = "Режим просмотра",
                DeletePromt = "Удалить выбранные элементы?",
                ClearPromt = "Очистить список?",
            };
        }
    }
}
