using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageAttributes
	{
		String None
		{ get; }
	}

	[XmlType("CommonAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		[XmlElement]
		public String None
		{ get; set; }

		#endregion

		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				None = "None...",
			};
		}
	}
}
