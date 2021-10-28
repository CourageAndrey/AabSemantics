using System.Xml.Serialization;

namespace Samples._07.CustomModule.Localization
{
	public interface ILanguageStatementsPart
	{
		string Custom
		{ get; }
	}

	[XmlType]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{
		#region Properties

		[XmlElement]
		public string Custom
		{ get; set; }

		#endregion

		public static LanguageStatementsPart CreateDefaultNames()
		{
			return new LanguageStatementsPart
			{
				Custom = "Custom",
			};
		}

		internal static LanguageStatementsPart CreateDefaultHints()
		{
			return new LanguageStatementsPart
			{
				Custom = "Just a statement to demonstate Modules customization."
			};
		}

		internal static LanguageStatementsPart CreateDefaultTrue()
		{
			return new LanguageStatementsPart
			{
				Custom = $"{CustomStatement.ParamConcept1} has got custom relationship with {CustomStatement.ParamConcept2}.",
			};
		}

		internal static LanguageStatementsPart CreateDefaultFalse()
		{
			return new LanguageStatementsPart
			{
				Custom = $"{CustomStatement.ParamConcept1} has not got custom relationship with {CustomStatement.ParamConcept2}.",
			};
		}

		internal static LanguageStatementsPart CreateDefaultQuestion()
		{
			return new LanguageStatementsPart
			{
				Custom = $"Has {CustomStatement.ParamConcept1} got custom relationship with {CustomStatement.ParamConcept2}?",
			};
		}
	}
}
