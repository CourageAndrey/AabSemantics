using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	/// <summary>Names of the built-in concept attributes in one language.</summary>
	public interface ILanguageAttributes
	{
		/// <summary>Name shown for "no attribute".</summary>
		String None
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageAttributes"/>, loaded from a language file.</summary>
	[XmlType("CommonAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		/// <summary>Name shown for "no attribute".</summary>
		[XmlElement]
		public String None
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				None = "None...",
			};
		}
	}
}
