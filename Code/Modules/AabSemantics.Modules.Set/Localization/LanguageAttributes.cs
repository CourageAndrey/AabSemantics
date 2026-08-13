using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Set.Localization
{
	/// <summary>Names of the attributes contributed by the set module.</summary>
	public interface ILanguageAttributes : ILanguageExtensionAttributes
	{
		/// <summary>Name of the "is a sign" attribute.</summary>
		String IsSign
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageAttributes"/>, loaded from a language file.</summary>
	[XmlType("SetsAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		/// <summary>Name of the "is a sign" attribute.</summary>
		[XmlElement]
		public String IsSign
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				IsSign = "Is Sign",
			};
		}
	}
}
