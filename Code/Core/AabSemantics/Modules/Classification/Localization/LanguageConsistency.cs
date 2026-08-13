using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	/// <summary>Wordings for the classification module's consistency problems.</summary>
	public interface ILanguageConsistency
	{
		/// <summary>Message reporting a cycle in the "is a" hierarchy.</summary>
		String ErrorCyclic
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageConsistency"/>, loaded from a language file.</summary>
	[XmlType("ClassificationConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		/// <summary>Message reporting a cycle in the "is a" hierarchy.</summary>
		[XmlElement]
		public String ErrorCyclic
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				ErrorCyclic = $"Statement {Strings.ParamStatement} causes cyclic references.",
			};
		}
	}
}
