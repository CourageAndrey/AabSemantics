using System.Xml.Serialization;

namespace AabSemantics.Sample07.CustomModule.Localization
{
	public interface ILanguageConsistency
	{
		string SelfReference
		{ get; }
	}

	[XmlType]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		[XmlElement]
		public string SelfReference
		{ get;
			set;
		}

		#endregion

		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				SelfReference = $"{CustomStatement.ParamConcept1} references to itself."
			};
		}
	}
}
