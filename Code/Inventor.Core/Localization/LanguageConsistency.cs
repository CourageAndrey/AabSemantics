using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		[XmlElement]
		public String CheckResult
		{ get; set; }

		[XmlElement]
		public String CheckOk
		{ get; set; }

		[XmlElement]
		public String ErrorDuplicate
		{ get; set; }

		[XmlElement]
		public String ErrorCyclic
		{ get; set; }

		[XmlElement]
		public String ErrorMultipleSign
		{ get; set; }

		[XmlElement]
		public String ErrorMultipleSignValue
		{ get; set; }

		[XmlElement]
		public String ErrorSignWithoutValue
		{ get; set; }

		[XmlElement]
		public String ErrorComparisonContradiction
		{ get; set; }

		[XmlElement]
		public String ErrorProcessesContradiction
		{ get; set; }

	#endregion

	internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				CheckResult = "Check result",
				CheckOk = "There is no errors.",
				ErrorDuplicate = $"Statement {Strings.ParamStatement} is duplicated.",
				ErrorCyclic = $"Statement {Strings.ParamStatement} causes cyclic references.",
				ErrorMultipleSign = $"Statement {Strings.ParamStatement} cause sign value overload.",
				ErrorMultipleSignValue = $"Value of {Strings.ParamSign} sign of {Strings.ParamConcept} concept is uncertain, because many ancestors define  their own values.",
				ErrorSignWithoutValue = $"{Strings.ParamStatement} defines value of sign, which does not belong to concept.",
				ErrorComparisonContradiction = $"Impossible to compare {Strings.ParamLeftValue} and {Strings.ParamRightValue}. Possible cases: ",
				ErrorProcessesContradiction = $"Impossible to detect sequence between {Strings.ParamProcessA} and {Strings.ParamProcessB}. Possible cases: ",
			};
		}
	}
}
