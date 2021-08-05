using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

using Inventor.Core.Utils;

namespace Inventor.Client.Localization
{
	[Serializable, XmlRoot(RootName)]
	public class Language : Core.Localization.Language, ILanguage
	{
		#region Constants

		[XmlIgnore]
		private const String ElementErrors = "Errors";
		[XmlIgnore]
		private const String FileFormat = "*.xml";
		[XmlIgnore]
		private const String FolderPath = "Localization";
		[XmlIgnore]
		private const String ElementUi = "Ui";
		[XmlIgnore]
		private const String ElementMisc = "Misc";

		#endregion

		#region Xml Properties

		[XmlElement(ElementErrors)]
		public LanguageErrors ErrorsXml
		{ get; set; }

		[XmlElement(ElementUi)]
		public LanguageUi UiXml
		{ get; set; }

		[XmlElement(ElementMisc)]
		public LanguageMisc MiscXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageErrors Errors
		{ get { return ErrorsXml; } }

		[XmlIgnore]
		public ILanguageUi Ui
		{ get { return UiXml; } }

		[XmlIgnore]
		public ILanguageMisc Misc
		{ get { return MiscXml; } }

		#endregion

		static Language()
		{
			var @default = Default;
			Default = new Language
			{
				FileName = @default.FileName,
				Name = @default.Name,
				Culture = @default.Culture,

				CommonXml = @default.CommonXml,
				ErrorsXml = LanguageErrors.CreateDefault(),

				StatementNamesXml = @default.StatementNamesXml,
				StatementHintsXml = @default.StatementHintsXml,
				TrueStatementFormatStringsXml = @default.TrueStatementFormatStringsXml,
				FalseStatementFormatStringsXml = @default.FalseStatementFormatStringsXml,
				QuestionStatementFormatStringsXml = @default.QuestionStatementFormatStringsXml,
				QuestionNamesXml = @default.QuestionNamesXml,
				AnswersXml = @default.AnswersXml,
				AttributesXml = @default.AttributesXml,
				UiXml = LanguageUi.CreateDefault(),
				SystemConceptNamesXml = @default.SystemConceptNamesXml,
				SystemConceptHintsXml = @default.SystemConceptHintsXml,
				ConsistencyXml = @default.ConsistencyXml,
				MiscXml = LanguageMisc.CreateDefault(),
			};
		}

		public static ICollection<ILanguage> LoadAdditional(string applicationPath)
		{
			var languagesFolder = new DirectoryInfo(Path.Combine(applicationPath, FolderPath));
			if (!languagesFolder.Exists)
			{
				languagesFolder.Create();
			}
			return languagesFolder.GetFiles(FileFormat).Select(f => f.FullName.DeserializeFromFile<Language>() as ILanguage).ToList();
		}
	}

	public static class LanguageExtensions
	{
		public static ILanguage FindAppropriate(this IEnumerable<ILanguage> languages, Language @default)
		{
			return languages.FirstOrDefault(l => l.Culture == Thread.CurrentThread.CurrentUICulture.Name) ?? @default;
		}
	}
}
