using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageStatements : ILanguageExtensionStatements
	{
		ILanguageConsistency Consistency
		{ get; }
	}

	[XmlType("CommonStatements")]
	public class LanguageStatements : ILanguageStatements
	{
		#region Xml Properties

		[XmlElement(nameof(Consistency))]
		public LanguageConsistency ConsistencyXml
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
			};
		}
	}
}
