using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	public interface ILanguageAttributes
	{
		String IsSign
		{ get; }
	}

	[XmlType("SetsAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		[XmlElement]
		public String IsSign
		{ get; set; }

		#endregion

		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				IsSign = "Is Sign",
			};
		}
	}
}
