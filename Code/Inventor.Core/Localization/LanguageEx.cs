using System;
using System.Xml.Serialization;

using Sef.Localization;

namespace Inventor.Core.Localization
{
    public interface ILanguageEx : ILanguage
    {
        ILanguagePropositions PropositionNames
        { get; }

        ILanguagePropositions PropositionHints
        { get; }

        ILanguagePropositionFormatStrings TruePropositionFormatStrings
        { get; }

        ILanguagePropositionFormatStrings FalsePropositionFormatStrings
        { get; }

        ILanguagePropositionFormatStrings QuestionPropositionFormatStrings
        { get; }

        ILanguageQuestionNames QuestionNames
        { get; }

        ILanguageAnswers Answers
        { get; }

        ILanguageUi Ui
        { get; }

        ILanguageErrorsInventor ErrorsInventor
        { get; }

        ILanguageConfiguration Configuration
        { get; }

        ILanguageMisc Misc
        { get; }
    }

    [Serializable, XmlRoot(RootName)]
    public class LanguageEx : Language, ILanguageEx
    {
        #region Xml Properties

        private const string ElementPropositionNames = "PropositionNames";
        private const string ElementPropositionHints = "PropositionHints";
        private const string ElementPropositionTrueFormatStrings = "PropositionTrueFormatStrings";
        private const string ElementPropositionFalseFormatStrings = "PropositionFalseFormatStrings";
        private const string ElementPropositionQuestionFormatStrings = "PropositionQuestionFormatStrings";
        private const string ElementQuestionNames = "QuestionNames";
        private const string ElementAnswers = "Answers";
        private const string ElementUi = "Ui";
        private const string ElementErrorsInventor = "ErrorsInventor";
        private const string ElementConfiguration = "Configuration";
        private const string ElementMisc = "Misc";

        [XmlElement(ElementPropositionNames)]
        public LanguagePropositions PropositionNamesXml
        { get; set; }

        [XmlElement(ElementPropositionHints)]
        public LanguagePropositions PropositionHintsXml
        { get; set; }

        [XmlElement(ElementPropositionTrueFormatStrings)]
        public LanguagePropositionFormatStrings TruePropositionFormatStringsXml
        { get; set; }

        [XmlElement(ElementPropositionFalseFormatStrings)]
        public LanguagePropositionFormatStrings FalsePropositionFormatStringsXml
        { get; set; }

        [XmlElement(ElementPropositionQuestionFormatStrings)]
        public LanguagePropositionFormatStrings QuestionPropositionFormatStringsXml
        { get; set; }

        [XmlElement(ElementQuestionNames)]
        public LanguageQuestionNames QuestionNamesXml
        { get; set; }

        [XmlElement(ElementAnswers)]
        public LanguageAnswers AnswersXml
        { get; set; }

        [XmlElement(ElementUi)]
        public LanguageUi UiXml
        { get; set; }

        [XmlElement(ElementErrorsInventor)]
        public LanguageErrorsInventor ErrorsInventorXml
        { get; set; }

        [XmlElement(ElementConfiguration)]
        public LanguageConfiguration ConfigurationXml
        { get; set; }

        [XmlElement(ElementMisc)]
        public LanguageMisc MiscXml
        { get; set; }

        #endregion

        #region Interface Properties

        [XmlIgnore]
        public ILanguagePropositions PropositionNames
        { get { return PropositionNamesXml; } }

        [XmlIgnore]
        public ILanguagePropositions PropositionHints
        { get { return PropositionHintsXml; } }

        [XmlIgnore]
        public ILanguagePropositionFormatStrings TruePropositionFormatStrings
        { get { return TruePropositionFormatStringsXml; } }

        [XmlIgnore]
        public ILanguagePropositionFormatStrings FalsePropositionFormatStrings
        { get { return FalsePropositionFormatStringsXml; } }

        [XmlIgnore]
        public ILanguagePropositionFormatStrings QuestionPropositionFormatStrings
        { get { return QuestionPropositionFormatStringsXml; } }

        [XmlIgnore]
        public ILanguageQuestionNames QuestionNames
        { get { return QuestionNamesXml; } }

        [XmlIgnore]
        public ILanguageAnswers Answers
        { get { return AnswersXml; } }

        [XmlIgnore]
        public ILanguageUi Ui
        { get { return UiXml; } }

        [XmlIgnore]
        public ILanguageErrorsInventor ErrorsInventor
        { get { return ErrorsInventorXml; } }

        [XmlIgnore]
        public ILanguageConfiguration Configuration
        { get { return ConfigurationXml; } }

        [XmlIgnore]
        public ILanguageMisc Misc
        { get { return MiscXml; } }

        #endregion

        #region Singleton

        private static LanguageEx defaultEx;
        private static LanguageEx currentEx;

        [XmlIgnore]
        public static LanguageEx CurrentEx
        {
            get { return currentEx; }
            set { Current = currentEx = value; }
        }

        [XmlIgnore]
        public static LanguageEx DefaultEx
        {
            get { return defaultEx; }
            private set { Default = defaultEx = value; }
        }

        static LanguageEx()
        {
            CurrentEx = DefaultEx = new LanguageEx
            {
                Culture = Default.Culture,
                Name = Default.Name,
                FileName = Default.FileName,
                CommonXml = Default.CommonXml,
                ErrorsXml = Default.ErrorsXml,
                EditorXml = Default.EditorXml,
                PropositionNamesXml = LanguagePropositions.CreateDefaultNames(),
                PropositionHintsXml = LanguagePropositions.CreateDefaultHints(),
                TruePropositionFormatStringsXml = LanguagePropositionFormatStrings.CreateDefaultTrue(),
                FalsePropositionFormatStringsXml = LanguagePropositionFormatStrings.CreateDefaultFalse(),
                QuestionPropositionFormatStringsXml = LanguagePropositionFormatStrings.CreateDefaultQuestion(),
                QuestionNamesXml = LanguageQuestionNames.CreateDefault(),
                AnswersXml = LanguageAnswers.CreateDefault(),
                UiXml = LanguageUi.CreateDefault(),
                ErrorsInventorXml = LanguageErrorsInventor.CreateDefault(),
                ConfigurationXml = LanguageConfiguration.CreateDefault(),
                MiscXml = LanguageMisc.CreateDefault(),
            };
        }

        #endregion
    }

    public sealed class LocalizatorEx : Localizator, ILanguageEx
    {
        public ILanguagePropositions PropositionNames
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.PropositionNames
                    : null;
            }
        }

        public ILanguagePropositions PropositionHints
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.PropositionHints
                    : null;
            }
        }

        public ILanguagePropositionFormatStrings TruePropositionFormatStrings
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.TruePropositionFormatStrings
                    : null;
            }
        }

        public ILanguagePropositionFormatStrings FalsePropositionFormatStrings
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.FalsePropositionFormatStrings
                    : null;
            }
        }

        public ILanguagePropositionFormatStrings QuestionPropositionFormatStrings
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.QuestionPropositionFormatStrings
                    : null;
            }
        }

        public ILanguageQuestionNames QuestionNames
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.QuestionNames
                    : null;
            }
        }

        public ILanguageAnswers Answers
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.Answers
                    : null;
            }
        }

        public ILanguageUi Ui
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.Ui
                    : null;
            }
        }

        public ILanguageErrorsInventor ErrorsInventor
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.ErrorsInventor
                    : null;
            }
        }

        public ILanguageConfiguration Configuration
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.Configuration
                    : null;
            }
        }

        public ILanguageMisc Misc
        {
            get
            {
                return LanguageEx.CurrentEx != null
                    ? LanguageEx.CurrentEx.Misc
                    : null;
            }
        }
    }
}
