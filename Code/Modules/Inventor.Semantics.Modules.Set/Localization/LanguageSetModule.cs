using System.Xml.Serialization;
using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Set.Localization
{
	public interface ILanguageSetModule
	{
		ILanguageAttributes Attributes
		{ get; }

		ILanguageStatements Statements
		{ get; }

		ILanguageQuestions Questions
		{ get; }
	}

	[XmlType]
	public class LanguageSetModule : LanguageExtension, ILanguageSetModule
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		public static LanguageSetModule CreateDefault()
		{
			return new LanguageSetModule()
			{
				AttributesXml = LanguageAttributes.CreateDefault(),
				StatementsXml = LanguageStatements.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}
	}
}
