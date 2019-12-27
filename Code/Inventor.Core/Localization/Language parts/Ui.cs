using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
    public interface ILanguageUi
    {
        string Ok
        { get; }

        string Cancel
        { get; }

        string Next
        { get; }

        string Back
        { get; }

        LanguageMainForm MainForm
        { get; }

        LanguageQuestionDialog QuestionDialog
        { get; }
    }

    public interface ILanguageMainForm
    {
        string Title
        { get; }

        string CreateNew
        { get; }

        string Load
        { get; }

        string Save
        { get; }

        string SaveAs
        { get; }

        string CreateTest
        { get; }

        string DescribeKnowledge
        { get; }

        string CheckKnowledge
        { get; }

        string AskQuestion
        { get; }

        string SelectLanguage
        { get; }

        string Configuration
        { get; }
    }

    public interface ILanguageQuestionDialog
    {
        string Title
        { get; }

        string SelectQuestion
        { get; }
    }

    public class LanguageUi : ILanguageUi
    {
        #region Properties

        [XmlElement]
        public string Ok
        { get; set; }

        [XmlElement]
        public string Cancel
        { get; set; }

        [XmlElement]
        public string Next
        { get; set; }

        [XmlElement]
        public string Back
        { get; set; }

        [XmlElement]
        public LanguageMainForm MainForm
        { get; set; }

        [XmlElement]
        public LanguageQuestionDialog QuestionDialog
        { get; set; }

        #endregion

        internal static LanguageUi CreateDefault()
        {
            return new LanguageUi
            {
                Ok = "ОК",
                Cancel = "Отмена",
                Next = "Далее >",
                Back = "< Назад",
                QuestionDialog = LanguageQuestionDialog.CreateDefault(),
                MainForm = LanguageMainForm.CreateDefault(),
            };
        }
    }

    public class LanguageMainForm : ILanguageMainForm
    {
        #region Properties

        [XmlElement]
        public string Title
        { get; set; }

        [XmlElement]
        public string CreateNew
        { get; set; }

        [XmlElement]
        public string Load
        { get; set; }

        [XmlElement]
        public string Save
        { get; set; }

        [XmlElement]
        public string SaveAs
        { get; set; }

        [XmlElement]
        public string CreateTest
        { get; set; }

        [XmlElement]
        public string DescribeKnowledge
        { get; set; }

        [XmlElement]
        public string CheckKnowledge
        { get; set; }

        [XmlElement]
        public string AskQuestion
        { get; set; }

        [XmlElement]
        public string SelectLanguage
        { get; set; }

        [XmlElement]
        public string Configuration
        { get; set; }

        #endregion

        internal static LanguageMainForm CreateDefault()
        {
            return new LanguageMainForm
            {
                Title = "Вспомогательная утилита \"Изобретатель\" (демо-версия)",
                CreateNew = "Создать новую базу знаний",
                Load = "Открыть...",
                Save = "Сохранить",
                SaveAs = "Сохранить как...",
                CreateTest = "Создать тестовую базу знаний",
                DescribeKnowledge = "Описать все знания...",
                CheckKnowledge = "Выполнить проверку знаний на непротиворечивость...",
                AskQuestion = "Задать вопрос...",
                SelectLanguage = "Язык:",
                Configuration = "Настройки...",
            };
        }
    }

    public class LanguageQuestionDialog : ILanguageQuestionDialog
    {
        #region Properties

        [XmlElement]
        public string Title
        { get; set; }

        [XmlElement]
        public string SelectQuestion
        { get; set; }

        #endregion

        internal static LanguageQuestionDialog CreateDefault()
        {
            return new LanguageQuestionDialog
            {
                Title = "Формирование вопроса",
                SelectQuestion = "Выберите вопрос: ",
            };
        }
    }
}
