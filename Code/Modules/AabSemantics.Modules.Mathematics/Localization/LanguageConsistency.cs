using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	/// <summary>Wordings for the mathematics module's consistency problems.</summary>
	public interface ILanguageConsistency
	{
		/// <summary>Message reporting contradicting comparison statements.</summary>
		String ErrorComparisonContradiction
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageConsistency"/>, loaded from a language file.</summary>
	[XmlType("MathematicsConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		/// <summary>Message reporting contradicting comparison statements.</summary>
		[XmlElement]
		public String ErrorComparisonContradiction
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				ErrorComparisonContradiction = $"Impossible to compare {Strings.ParamLeftValue} and {Strings.ParamRightValue}. Possible cases: ",
			};
		}
	}
}
