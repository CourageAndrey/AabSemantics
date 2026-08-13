using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	/// <summary>Captions of the parameters the mathematics module's questions ask for.</summary>
	public interface ILanguageQuestionParameters
	{
		/// <summary>Caption of the left-hand value parameter.</summary>
		String LeftValue
		{ get; }

		/// <summary>Caption of the right-hand value parameter.</summary>
		String RightValue
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestionParameters"/>, loaded from a language file.</summary>
	[XmlType("MathematicsQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		/// <summary>Caption of the left-hand value parameter.</summary>
		[XmlElement]
		public String LeftValue
		{ get; set; }

		/// <summary>Caption of the right-hand value parameter.</summary>
		[XmlElement]
		public String RightValue
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				LeftValue = "Left value",
				RightValue = "Right value",
			};
		}
	}
}
