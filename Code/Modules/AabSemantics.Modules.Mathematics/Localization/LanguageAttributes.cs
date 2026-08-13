using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	/// <summary>Names of the attributes contributed by the mathematics module.</summary>
	public interface ILanguageAttributes : ILanguageExtensionAttributes
	{
		/// <summary>Name of the "is a comparison sign" attribute.</summary>
		String IsComparisonSign
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageAttributes"/>, loaded from a language file.</summary>
	[XmlType("MathematicsAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		/// <summary>Name of the "is a comparison sign" attribute.</summary>
		[XmlElement]
		public String IsComparisonSign
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				IsComparisonSign = "Is Comparison Sign",
			};
		}
	}
}
