using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.Modules.Set.Localization
{
	public interface ILanguageAnswers
	{
		String IsTrue
		{ get; }

		String IsFalse
		{ get; }

		String IsSubjectAreaTrue
		{ get; }

		String IsSubjectAreaFalse
		{ get; }

		String SignTrue
		{ get; }

		String SignFalse
		{ get; }

		String ValueTrue
		{ get; }

		String ValueFalse
		{ get; }

		String HasSignTrue
		{ get; }

		String HasSignFalse
		{ get; }

		String HasSignsTrue
		{ get; }

		String HasSignsFalse
		{ get; }

		String IsDescription
		{ get; }

		String IsDescriptionWithSign
		{ get; }

		String IsDescriptionWithSignValue
		{ get; }

		String SubjectArea
		{ get; }

		String SubjectAreaConcepts
		{ get; }

		String ConceptSigns
		{ get; }

		String SignValue
		{ get; }

		String IsPartOfTrue
		{ get; }

		String IsPartOfFalse
		{ get; }

		String EnumerateParts
		{ get; }

		String EnumerateContainers
		{ get; }

		String CanNotCompareConcepts
		{ get; }

		String CompareConceptsResult
		{ get; }

		String CompareConceptsCommon
		{ get; }

		String CompareConceptsCommonNotSet
		{ get; }

		String CompareConceptsNoCommon
		{ get; }

		String CompareConceptsDifference
		{ get; }

		String CompareConceptsFirstNotSet
		{ get; }

		String CompareConceptsSecondNotSet
		{ get; }

		String CompareConceptsNoDifference
		{ get; }

		String CompareConceptsSameHierarchy
		{ get; }

		String CompareConceptsDifferentHierarchyFirst
		{ get; }

		String CompareConceptsDifferentHierarchySecond
		{ get; }
	}

	[XmlType("SetsAnswers")]
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

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
		public String CanNotCompareConcepts
		{ get; set; }

		[XmlElement]
		public String CompareConceptsResult
		{ get; set; }

		[XmlElement]
		public String CompareConceptsCommon
		{ get; set; }

		[XmlElement]
		public String CompareConceptsNoCommon
		{ get; set; }

		[XmlElement]
		public String CompareConceptsCommonNotSet
		{ get; set; }

		[XmlElement]
		public String CompareConceptsDifference
		{ get; set; }

		[XmlElement]
		public String CompareConceptsFirstNotSet
		{ get; set; }

		[XmlElement]
		public String CompareConceptsSecondNotSet
		{ get; set; }

		[XmlElement]
		public String CompareConceptsNoDifference
		{ get; set; }

		[XmlElement]
		public String CompareConceptsSameHierarchy
		{ get; set; }

		[XmlElement]
		public String CompareConceptsDifferentHierarchyFirst
		{ get; set; }

		[XmlElement]
		public String CompareConceptsDifferentHierarchySecond
		{ get; set; }

		#endregion

		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				IsTrue = $"Yes, {Semantics.Localization.Strings.ParamChild} is {Semantics.Localization.Strings.ParamParent}.",
				IsFalse = $"No, {Semantics.Localization.Strings.ParamChild} is not {Semantics.Localization.Strings.ParamParent}.",
				IsSubjectAreaTrue = $"Yes, {Semantics.Localization.Strings.ParamConcept} concept belongs to {Strings.ParamArea} subject area.",
				IsSubjectAreaFalse = $"No, {Semantics.Localization.Strings.ParamConcept} concept does not belong to {Strings.ParamArea} subject area.",
				IsDescription = $"{Semantics.Localization.Strings.ParamChild} is {Semantics.Localization.Strings.ParamParent}.",
				IsDescriptionWithSign = $"{Semantics.Localization.Strings.ParamChild} is {Semantics.Localization.Strings.ParamParent} with following sign values (properties):",
				IsDescriptionWithSignValue = $"... {Strings.ParamSign} sign value is equal to {Strings.ParamValue}",
				SignTrue = $"Yes, {Semantics.Localization.Strings.ParamConcept} is sign.",
				SignFalse = $"No, {Semantics.Localization.Strings.ParamConcept} is not sign.",
				ValueTrue = $"Yes, {Semantics.Localization.Strings.ParamConcept} is sign value.",
				ValueFalse = $"No, {Semantics.Localization.Strings.ParamConcept} is not sign value.",
				HasSignTrue = $"Yes, {Semantics.Localization.Strings.ParamConcept} has got {Strings.ParamSign} sign ",
				HasSignFalse = $"No, {Semantics.Localization.Strings.ParamConcept} has not got {Strings.ParamSign} sign ",
				HasSignsTrue = $"Yes, {Semantics.Localization.Strings.ParamConcept} has signs ",
				HasSignsFalse = $"No, {Semantics.Localization.Strings.ParamConcept} has not signs ",
				SubjectArea = $"{Semantics.Localization.Strings.ParamConcept} belongs to following subject areas:",
				SubjectAreaConcepts = $"{Semantics.Localization.Strings.ParamConcept} subject area contains following concepts:",
				ConceptSigns = $"{Semantics.Localization.Strings.ParamConcept} concept has following signs",
				SignValue = $"{Semantics.Localization.Strings.ParamConcept} concept has {Strings.ParamSign} sign value equal to {Strings.ParamValue} (defined for {Strings.ParamDefined}).",
				IsPartOfTrue = $"Yes, {Semantics.Localization.Strings.ParamChild} is part of {Semantics.Localization.Strings.ParamParent}.",
				IsPartOfFalse = $"No, {Semantics.Localization.Strings.ParamChild} is not part of {Semantics.Localization.Strings.ParamParent}.",
				EnumerateParts = $"{Semantics.Localization.Strings.ParamParent} consists of:",
				EnumerateContainers = $"{Semantics.Localization.Strings.ParamChild} is part of:",
				CanNotCompareConcepts = $"Concepts {Strings.ParamConcept1} and {Strings.ParamConcept2} have no common ancestors and can not be compared.",
				CompareConceptsResult = $"Result of {Strings.ParamConcept1} and {Strings.ParamConcept2} comparison:",
				CompareConceptsCommon = $"Both have {Strings.ParamSign} sign value equal to {Strings.ParamValue}.",
				CompareConceptsNoCommon = "No common found according to existing information.",
				CompareConceptsCommonNotSet = "{Strings.ParamSign} sign value is not set for both concepts.",
				CompareConceptsDifference = $"First have {Strings.ParamSign} sign value equal to {Strings.ParamConcept1}, and second one equal to {Strings.ParamConcept2}.",
				CompareConceptsFirstNotSet = $"First have {Strings.ParamSign} sign value not set, and second one equal to {Strings.ParamConcept2}.",
				CompareConceptsSecondNotSet = $"First have {Strings.ParamSign} sign value equal to {Strings.ParamConcept1}, and second one not set.",
				CompareConceptsNoDifference = "No differences found according to existing information.",
				CompareConceptsSameHierarchy = "Both have the same ancestor's hierarchy.",
				CompareConceptsDifferentHierarchyFirst = "First is also:",
				CompareConceptsDifferentHierarchySecond = "Second is also:",
			};
		}
	}
}
