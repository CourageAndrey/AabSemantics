using System;

namespace AabSemantics.Localization
{
	/// <summary>
	/// Fixed, non-localized strings: the anchor tokens format strings substitute knowledge items
	/// under, and a few internal captions. The <c>Param*</c> values are part of the localization
	/// contract, because language files reference them verbatim.
	/// </summary>
	public static class Strings
	{
		/// <summary>Caption used when formatting a semantic network for diagnostics.</summary>
		public const String TostringSemanticNetwork = "KNOWLEDGE_BASE";

		/// <summary>Caption used when formatting a localized string for diagnostics.</summary>
		public const String TostringLocalized = "LOCALIZED_STRING";

		/// <summary>Anchor token standing for the concept a sentence is about.</summary>
		public const String ParamConcept = "#CONCEPT#";

		/// <summary>Anchor token standing for the more specific side of a relation.</summary>
		public const String ParamChild = "#CHILD#";

		/// <summary>Anchor token standing for the more general side of a relation.</summary>
		public const String ParamParent = "#PARENT#";

		/// <summary>Anchor token standing for the statement a sentence is about.</summary>
		public const String ParamStatement = "#STATEMENT#";

		/// <summary>Anchor token standing for an answer's value.</summary>
		public const String ParamAnswer = "#ANSWER#";

		/// <summary>Default name given to a newly created semantic network.</summary>
		public const String NewKbName = "New...";
	}
}
