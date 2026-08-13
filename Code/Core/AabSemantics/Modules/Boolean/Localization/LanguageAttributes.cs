using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Boolean.Localization
{
	/// <summary>Names of the attributes contributed by the boolean module.</summary>
	public interface ILanguageAttributes : ILanguageExtensionAttributes
	{
		/// <summary>Name of the "is a value" attribute.</summary>
		String IsValue
		{ get; }

		/// <summary>Name of the "is a logical value" attribute.</summary>
		String IsBoolean
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageAttributes"/>, loaded from a language file.</summary>
	[XmlType("BooleanAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		/// <summary>Name of the "is a value" attribute.</summary>
		[XmlElement]
		public String IsValue
		{ get; set; }

		/// <summary>Name of the "is a logical value" attribute.</summary>
		[XmlElement]
		public String IsBoolean
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				IsValue = "Is Value",
				IsBoolean = "Is Boolean",
			};
		}
	}
}
