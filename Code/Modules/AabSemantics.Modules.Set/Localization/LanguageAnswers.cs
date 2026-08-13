using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	/// <summary>Wordings of the set module's answers.</summary>
	public interface ILanguageAnswers
	{
		/// <summary>Affirmative wording of the "is a subject area" answer.</summary>
		String IsSubjectAreaTrue
		{ get; }

		/// <summary>Negative wording of the "is a subject area" answer.</summary>
		String IsSubjectAreaFalse
		{ get; }

		/// <summary>Affirmative wording of the "is a sign" answer.</summary>
		String SignTrue
		{ get; }

		/// <summary>Negative wording of the "is a sign" answer.</summary>
		String SignFalse
		{ get; }

		/// <summary>Affirmative wording of the "is a value" answer.</summary>
		String ValueTrue
		{ get; }

		/// <summary>Negative wording of the "is a value" answer.</summary>
		String ValueFalse
		{ get; }

		/// <summary>Affirmative wording of the "has this sign" answer.</summary>
		String HasSignTrue
		{ get; }

		/// <summary>Negative wording of the "has this sign" answer.</summary>
		String HasSignFalse
		{ get; }

		/// <summary>Affirmative wording of the "has any signs" answer.</summary>
		String HasSignsTrue
		{ get; }

		/// <summary>Negative wording of the "has any signs" answer.</summary>
		String HasSignsFalse
		{ get; }

		/// <summary>Wording describing a concept without signs.</summary>
		String IsDescription
		{ get; }

		/// <summary>Wording describing a concept together with one of its signs.</summary>
		String IsDescriptionWithSign
		{ get; }

		/// <summary>Wording describing a concept together with a sign and its value.</summary>
		String IsDescriptionWithSignValue
		{ get; }

		/// <summary>Caption of a subject area's description.</summary>
		String SubjectArea
		{ get; }

		/// <summary>Caption of the list of concepts in a subject area.</summary>
		String SubjectAreaConcepts
		{ get; }

		/// <summary>Caption of a concept's list of signs.</summary>
		String ConceptSigns
		{ get; }

		/// <summary>Wording naming the value of a concept's sign.</summary>
		String SignValue
		{ get; }

		/// <summary>Affirmative wording of the "is part of" answer.</summary>
		String IsPartOfTrue
		{ get; }

		/// <summary>Negative wording of the "is part of" answer.</summary>
		String IsPartOfFalse
		{ get; }

		/// <summary>Caption of the list of a concept's parts.</summary>
		String EnumerateParts
		{ get; }

		/// <summary>Caption of the list of containers a concept belongs to.</summary>
		String EnumerateContainers
		{ get; }

		/// <summary>Message shown when the two concepts cannot be compared at all.</summary>
		String CanNotCompareConcepts
		{ get; }

		/// <summary>Caption introducing the comparison result.</summary>
		String CompareConceptsResult
		{ get; }

		/// <summary>Caption of the properties the two concepts share.</summary>
		String CompareConceptsCommon
		{ get; }

		/// <summary>Wording for a shared property whose value is undefined.</summary>
		String CompareConceptsCommonNotSet
		{ get; }

		/// <summary>Message shown when the concepts share nothing.</summary>
		String CompareConceptsNoCommon
		{ get; }

		/// <summary>Caption of the properties the two concepts differ in.</summary>
		String CompareConceptsDifference
		{ get; }

		/// <summary>Wording for a property defined only for the second concept.</summary>
		String CompareConceptsFirstNotSet
		{ get; }

		/// <summary>Wording for a property defined only for the first concept.</summary>
		String CompareConceptsSecondNotSet
		{ get; }

		/// <summary>Message shown when the concepts do not differ.</summary>
		String CompareConceptsNoDifference
		{ get; }

		/// <summary>Wording used when both concepts sit in the same hierarchy.</summary>
		String CompareConceptsSameHierarchy
		{ get; }

		/// <summary>Wording naming the first concept's hierarchy.</summary>
		String CompareConceptsDifferentHierarchyFirst
		{ get; }

		/// <summary>Wording naming the second concept's hierarchy.</summary>
		String CompareConceptsDifferentHierarchySecond
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageAnswers"/>, loaded from a language file.</summary>
	[XmlType("SetsAnswers")]
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		/// <summary>Affirmative wording of the "is a subject area" answer.</summary>
		[XmlElement]
		public String IsSubjectAreaTrue
		{ get; set; }

		/// <summary>Negative wording of the "is a subject area" answer.</summary>
		[XmlElement]
		public String IsSubjectAreaFalse
		{ get; set; }

		/// <summary>Affirmative wording of the "is a sign" answer.</summary>
		[XmlElement]
		public String SignTrue
		{ get; set; }

		/// <summary>Negative wording of the "is a sign" answer.</summary>
		[XmlElement]
		public String SignFalse
		{ get; set; }

		/// <summary>Affirmative wording of the "is a value" answer.</summary>
		[XmlElement]
		public String ValueTrue
		{ get; set; }

		/// <summary>Negative wording of the "is a value" answer.</summary>
		[XmlElement]
		public String ValueFalse
		{ get; set; }

		/// <summary>Affirmative wording of the "has this sign" answer.</summary>
		[XmlElement]
		public String HasSignTrue
		{ get; set; }

		/// <summary>Negative wording of the "has this sign" answer.</summary>
		[XmlElement]
		public String HasSignFalse
		{ get; set; }

		/// <summary>Affirmative wording of the "has any signs" answer.</summary>
		[XmlElement]
		public String HasSignsTrue
		{ get; set; }

		/// <summary>Negative wording of the "has any signs" answer.</summary>
		[XmlElement]
		public String HasSignsFalse
		{ get; set; }

		/// <summary>Wording describing a concept without signs.</summary>
		[XmlElement]
		public String IsDescription
		{ get; set; }

		/// <summary>Wording describing a concept together with one of its signs.</summary>
		[XmlElement]
		public String IsDescriptionWithSign
		{ get; set; }

		/// <summary>Wording describing a concept together with a sign and its value.</summary>
		[XmlElement]
		public String IsDescriptionWithSignValue
		{ get; set; }

		/// <summary>Caption of a subject area's description.</summary>
		[XmlElement]
		public String SubjectArea
		{ get; set; }

		/// <summary>Caption of the list of concepts in a subject area.</summary>
		[XmlElement]
		public String SubjectAreaConcepts
		{ get; set; }

		/// <summary>Caption of a concept's list of signs.</summary>
		[XmlElement]
		public String ConceptSigns
		{ get; set; }

		/// <summary>Wording naming the value of a concept's sign.</summary>
		[XmlElement]
		public String SignValue
		{ get; set; }

		/// <summary>Affirmative wording of the "is part of" answer.</summary>
		[XmlElement]
		public String IsPartOfTrue
		{ get; set; }

		/// <summary>Negative wording of the "is part of" answer.</summary>
		[XmlElement]
		public String IsPartOfFalse
		{ get; set; }

		/// <summary>Caption of the list of a concept's parts.</summary>
		[XmlElement]
		public String EnumerateParts
		{ get; set; }

		/// <summary>Caption of the list of containers a concept belongs to.</summary>
		[XmlElement]
		public String EnumerateContainers
		{ get; set; }

		/// <summary>Message shown when the two concepts cannot be compared at all.</summary>
		[XmlElement]
		public String CanNotCompareConcepts
		{ get; set; }

		/// <summary>Caption introducing the comparison result.</summary>
		[XmlElement]
		public String CompareConceptsResult
		{ get; set; }

		/// <summary>Caption of the properties the two concepts share.</summary>
		[XmlElement]
		public String CompareConceptsCommon
		{ get; set; }

		/// <summary>Message shown when the concepts share nothing.</summary>
		[XmlElement]
		public String CompareConceptsNoCommon
		{ get; set; }

		/// <summary>Wording for a shared property whose value is undefined.</summary>
		[XmlElement]
		public String CompareConceptsCommonNotSet
		{ get; set; }

		/// <summary>Caption of the properties the two concepts differ in.</summary>
		[XmlElement]
		public String CompareConceptsDifference
		{ get; set; }

		/// <summary>Wording for a property defined only for the second concept.</summary>
		[XmlElement]
		public String CompareConceptsFirstNotSet
		{ get; set; }

		/// <summary>Wording for a property defined only for the first concept.</summary>
		[XmlElement]
		public String CompareConceptsSecondNotSet
		{ get; set; }

		/// <summary>Message shown when the concepts do not differ.</summary>
		[XmlElement]
		public String CompareConceptsNoDifference
		{ get; set; }

		/// <summary>Wording used when both concepts sit in the same hierarchy.</summary>
		[XmlElement]
		public String CompareConceptsSameHierarchy
		{ get; set; }

		/// <summary>Wording naming the first concept's hierarchy.</summary>
		[XmlElement]
		public String CompareConceptsDifferentHierarchyFirst
		{ get; set; }

		/// <summary>Wording naming the second concept's hierarchy.</summary>
		[XmlElement]
		public String CompareConceptsDifferentHierarchySecond
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				IsSubjectAreaTrue = $"Yes, {AabSemantics.Localization.Strings.ParamConcept} concept belongs to {Strings.ParamArea} subject area.",
				IsSubjectAreaFalse = $"No, {AabSemantics.Localization.Strings.ParamConcept} concept does not belong to {Strings.ParamArea} subject area.",
				IsDescription = $"{AabSemantics.Localization.Strings.ParamChild} is {AabSemantics.Localization.Strings.ParamParent}.",
				IsDescriptionWithSign = $"{AabSemantics.Localization.Strings.ParamChild} is {AabSemantics.Localization.Strings.ParamParent} with following sign values (properties):",
				IsDescriptionWithSignValue = $"... {Strings.ParamSign} sign value is equal to {Strings.ParamValue}",
				SignTrue = $"Yes, {AabSemantics.Localization.Strings.ParamConcept} is sign.",
				SignFalse = $"No, {AabSemantics.Localization.Strings.ParamConcept} is not sign.",
				ValueTrue = $"Yes, {AabSemantics.Localization.Strings.ParamConcept} is sign value.",
				ValueFalse = $"No, {AabSemantics.Localization.Strings.ParamConcept} is not sign value.",
				HasSignTrue = $"Yes, {AabSemantics.Localization.Strings.ParamConcept} has got {Strings.ParamSign} sign ",
				HasSignFalse = $"No, {AabSemantics.Localization.Strings.ParamConcept} has not got {Strings.ParamSign} sign ",
				HasSignsTrue = $"Yes, {AabSemantics.Localization.Strings.ParamConcept} has signs ",
				HasSignsFalse = $"No, {AabSemantics.Localization.Strings.ParamConcept} has not signs ",
				SubjectArea = $"{AabSemantics.Localization.Strings.ParamConcept} belongs to following subject areas:",
				SubjectAreaConcepts = $"{AabSemantics.Localization.Strings.ParamConcept} subject area contains following concepts:",
				ConceptSigns = $"{AabSemantics.Localization.Strings.ParamConcept} concept has following signs",
				SignValue = $"{AabSemantics.Localization.Strings.ParamConcept} concept has {Strings.ParamSign} sign value equal to {Strings.ParamValue} (defined for {Strings.ParamDefined}).",
				IsPartOfTrue = $"Yes, {AabSemantics.Localization.Strings.ParamChild} is part of {AabSemantics.Localization.Strings.ParamParent}.",
				IsPartOfFalse = $"No, {AabSemantics.Localization.Strings.ParamChild} is not part of {AabSemantics.Localization.Strings.ParamParent}.",
				EnumerateParts = $"{AabSemantics.Localization.Strings.ParamParent} consists of:",
				EnumerateContainers = $"{AabSemantics.Localization.Strings.ParamChild} is part of:",
				CanNotCompareConcepts = $"Concepts {Strings.ParamConcept1} and {Strings.ParamConcept2} have no common ancestors and can not be compared.",
				CompareConceptsResult = $"Result of {Strings.ParamConcept1} and {Strings.ParamConcept2} comparison:",
				CompareConceptsCommon = $"Both have {Strings.ParamSign} sign value equal to {Strings.ParamValue}.",
				CompareConceptsNoCommon = "No common found according to existing information.",
				CompareConceptsCommonNotSet = $"{Strings.ParamSign} sign value is not set for both concepts.",
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
