using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		[XmlElement]
		public String Unknown
		{ get; set; }

		[XmlElement]
		public String IsTrue
		{ get; set; }

		[XmlElement]
		public String IsFalse
		{ get; set; }

		[XmlElement]
		public String IsSubjectAreaTrue
		{ get; set; }

		[XmlElement]
		public String IsSubjectAreaFalse
		{ get; set; }

		[XmlElement]
		public String SignTrue
		{ get; set; }

		[XmlElement]
		public String SignFalse
		{ get; set; }

		[XmlElement]
		public String ValueTrue
		{ get; set; }

		[XmlElement]
		public String ValueFalse
		{ get; set; }

		[XmlElement]
		public String HasSignTrue
		{ get; set; }

		[XmlElement]
		public String HasSignFalse
		{ get; set; }

		[XmlElement]
		public String HasSignsTrue
		{ get; set; }

		[XmlElement]
		public String HasSignsFalse
		{ get; set; }

		[XmlElement]
		public String IsDescription
		{ get; set; }

		[XmlElement]
		public String IsDescriptionWithSign
		{ get; set; }

		[XmlElement]
		public String IsDescriptionWithSignValue
		{ get; set; }

		[XmlElement]
		public String Enumerate
		{ get; set; }

		[XmlElement]
		public String SubjectArea
		{ get; set; }

		[XmlElement]
		public String SubjectAreaConcepts
		{ get; set; }

		[XmlElement]
		public String ConceptSigns
		{ get; set; }

		[XmlElement]
		public String SignValue
		{ get; set; }

		[XmlElement]
		public String RecursiveTrue
		{ get; set; }

		[XmlElement]
		public String RecursiveFalse
		{ get; set; }

		[XmlElement]
		public String IsPartOfTrue
		{ get; set; }

		[XmlElement]
		public String IsPartOfFalse
		{ get; set; }

		[XmlElement]
		public String EnumerateParts
		{ get; set; }

		[XmlElement]
		public String EnumerateContainers
		{ get; set; }

		[XmlElement]
		public String Explanation
		{ get; set; }

		#endregion

		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				Unknown = "Impossible to answer (there is no corresponding information).",
				IsTrue = $"Yes, {Strings.ParamChild} is {Strings.ParamParent}.",
				IsFalse = $"No, {Strings.ParamChild} is not {Strings.ParamParent}.",
				IsSubjectAreaTrue = $"Yes, {Strings.ParamConcept} concept belongs to {Strings.ParamArea} subject area.",
				IsSubjectAreaFalse = $"No, {Strings.ParamConcept} concept does not belong to {Strings.ParamArea} subject area.",
				IsDescription = $"{Strings.ParamChild} is {Strings.ParamParent}.",
				IsDescriptionWithSign = $"{Strings.ParamChild} is {Strings.ParamParent} with following sign values (properties):",
				IsDescriptionWithSignValue = $"... {Strings.ParamSign} sign value is equal to {Strings.ParamValue}",
				SignTrue = $"Yes, {Strings.ParamConcept} is sign.",
				SignFalse = $"No, {Strings.ParamConcept} is not sign.",
				ValueTrue = $"Yes, {Strings.ParamConcept} is sign value.",
				ValueFalse = $"No, {Strings.ParamConcept} is not sign value.",
				HasSignTrue = $"Yes, {Strings.ParamConcept} has got {Strings.ParamSign} sign ",
				HasSignFalse = $"No, {Strings.ParamConcept} has not got {Strings.ParamSign} sign ",
				HasSignsTrue = $"Yes, {Strings.ParamConcept} has signs ",
				HasSignsFalse = $"No, {Strings.ParamConcept} has not signs ",
				Enumerate = $"{Strings.ParamParent} can be following: ",
				SubjectArea = $"{Strings.ParamConcept} belongs to following subject areas: ",
				SubjectAreaConcepts = $"{Strings.ParamConcept} subject area contains following concepts: ",
				ConceptSigns = $"{Strings.ParamConcept} concept has following signs",
				SignValue = $"{Strings.ParamConcept} concept has {Strings.ParamSign} sign value equal to {Strings.ParamValue} (defined for {Strings.ParamDefined}).",
				RecursiveTrue = " (including parents)",
				RecursiveFalse = " (without parents)",
				IsPartOfTrue = $"Yes, {Strings.ParamChild} is part of {Strings.ParamParent}.",
				IsPartOfFalse = $"No, {Strings.ParamChild} is not part of {Strings.ParamParent}.",
				EnumerateParts = $"{Strings.ParamParent} consists of: ",
				EnumerateContainers = $"{Strings.ParamChild} is part of: ",
				Explanation = "Explanation:",
			};
		}
	}
}
