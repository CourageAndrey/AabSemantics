using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Boolean.Localization
{
	public interface ILanguageAttributes : ILanguageExtensionAttributes
	{
		String IsValue
		{ get; }

		String IsBoolean
		{ get; }
	}

	[XmlType("BooleanAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		[XmlElement]
		public String IsValue
		{ get; set; }

		[XmlElement]
		public String IsBoolean
		{ get; set; }

		#endregion

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
