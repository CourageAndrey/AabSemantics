using System.Xml.Serialization;

namespace AabSemantics.Sample07.CustomModule.Localization
{
	public interface ILanguageConcepts
	{
		string CustomName
		{ get; }

		string CustomHint
		{ get; }
	}

	[XmlType("CustomConcepts")]
	public class LanguageConcepts : ILanguageConcepts
	{
		#region Properties

		[XmlElement]
		public string CustomName
		{ get; set; }

		[XmlElement]
		public string CustomHint
		{ get; set; }

		#endregion

		internal static LanguageConcepts CreateDefault()
		{
			return new LanguageConcepts
			{
				CustomName = "Custom",
				CustomHint = "Custom (long descriptive hint)",
			};
		}
	}
}
