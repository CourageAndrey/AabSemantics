using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	/// <summary>Wordings for the built-in statements and for consistency reporting.</summary>
	public interface ILanguageStatements : ILanguageExtensionStatements
	{
		/// <summary>Wordings used when reporting consistency problems.</summary>
		ILanguageConsistency Consistency
		{ get; }

		/// <summary>Display name shared by every custom statement type.</summary>
		String CustomStatementName
		{ get; }

		/// <summary>Caption introducing a list of found statements.</summary>
		String FoundStatements
		{ get; }
	}

	/// <summary>
	/// Serializable <see cref="ILanguageStatements"/>. Nested bundles are exposed twice: as a
	/// concrete <c>*Xml</c> property the serializer writes, and as the read-only interface
	/// property the engine reads.
	/// </summary>
	[XmlType("CommonStatements")]
	public class LanguageStatements : ILanguageStatements
	{
		#region Xml Properties

		/// <summary>Consistency wordings, in serializable form.</summary>
		[XmlElement(nameof(Consistency))]
		public LanguageConsistency ConsistencyXml
		{ get; set; }

		/// <summary>Display name shared by every custom statement type.</summary>
		[XmlElement(nameof(CustomStatementName))]
		public String CustomStatementName
		{ get; set; }

		/// <summary>Caption introducing a list of found statements.</summary>
		[XmlElement(nameof(FoundStatements))]
		public String FoundStatements
		{ get; set; }

		#endregion

		#region Interface Properties

		/// <summary>Consistency wordings.</summary>
		[XmlIgnore]
		public ILanguageConsistency Consistency
		{ get { return ConsistencyXml; } }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
