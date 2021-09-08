using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	[Serializable, XmlType(RootName), XmlRoot(RootName)]
	public class Language : ILanguage
	{
		#region Constants

		[XmlIgnore]
		internal const String RootName = "Language";
		[XmlIgnore]
		private const String DefaultCulture = "en-US";
		[XmlIgnore]
		private const String DefaultName = "English";

		#endregion

		#region Xml Properties

		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		[XmlElement(nameof(Concepts))]
		public LanguageConcepts ConceptsXml
		{ get; set; }

		[XmlElement(nameof(Consistency))]
		public LanguageConsistency ConsistencyXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public String FileName
		{ get; protected set; }

		[XmlAttribute]
		public String Name
		{ get; set; }

		[XmlAttribute]
		public String Culture
		{ get; set; }

		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		[XmlIgnore]
		public ILanguageConcepts Concepts
		{ get { return ConceptsXml; } }

		[XmlIgnore]
		public ILanguageConsistency Consistency
		{ get { return ConsistencyXml; } }

		#endregion

		[XmlIgnore]
		public static Language Default
		{ get; protected set; }

		static Language()
		{
			Default = new Language
			{
				FileName = String.Empty,
				Name = DefaultName,
				Culture = DefaultCulture,

				StatementsXml = LanguageStatements.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
				ConceptsXml = LanguageConcepts.CreateDefault(),
				ConsistencyXml = LanguageConsistency.CreateDefault(),
			};
		}

		public override String ToString()
		{
			return Name;
		}
	}
}
