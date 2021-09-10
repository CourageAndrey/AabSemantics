using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules
{
	public interface ILanguageSetModule
	{
		Set.ILanguageAttributes Attributes
		{ get; }

		Set.ILanguageStatements Statements
		{ get; }

		Set.ILanguageQuestions Questions
		{ get; }
	}

	[XmlType]
	public class LanguageSetModule : LanguageExtension, ILanguageSetModule
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public Set.LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Statements))]
		public Set.LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public Set.LanguageQuestions QuestionsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public Set.ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public Set.ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public Set.ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		public static LanguageSetModule CreateDefault()
		{
			return new LanguageSetModule()
			{
				AttributesXml = Set.LanguageAttributes.CreateDefault(),
				StatementsXml = Set.LanguageStatements.CreateDefault(),
				QuestionsXml = Set.LanguageQuestions.CreateDefault(),
			};
		}
	}
}
