using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Boolean
{
	public interface ILanguageSystemConcepts
	{
		String True
		{ get; }

		String False
		{ get; }
	}

	[XmlType("BooleanSystemConcepts")]
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

		[XmlElement]
		public String True
		{ get; set; }

		[XmlElement]
		public String False
		{ get; set; }

		#endregion

		internal static LanguageSystemConcepts CreateDefaultNames()
		{
			return new LanguageSystemConcepts
			{
				True = "true",
				False = "false",
			};
		}

		internal static LanguageSystemConcepts CreateDefaultHints()
		{
			return new LanguageSystemConcepts
			{
				True = "Boolean: true.",
				False = "Boolean: false.",
			};
		}
	}
}
