using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageStatements : ILanguageExtensionStatements
	{
		ILanguageConsistency Consistency
		{ get; }

		String CustomStatementName
		{ get; }

		String FoundStatements
		{ get; }
	}

	[XmlType("CommonStatements")]
	public class LanguageStatements : ILanguageStatements
	{
		#region Xml Properties

		[XmlElement(nameof(Consistency))]
		public LanguageConsistency ConsistencyXml
		{ get; set; }

		[XmlElement(nameof(CustomStatementName))]
		public String CustomStatementName
		{ get; set; }

		[XmlElement(nameof(FoundStatements))]
		public String FoundStatements
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageConsistency Consistency
		{ get { return ConsistencyXml; } }

		#endregion

		internal static LanguageStatements CreateDefault()
		{
			return new LanguageStatements
			{
				ConsistencyXml = LanguageConsistency.CreateDefault(),
				CustomStatementName = "Custom Statement",
				FoundStatements = "Found statements:",
			};
		}
	}
}
