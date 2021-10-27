using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Statements;
using Samples._07.CustomModule.Localization;

namespace Samples._07.CustomModule
{
	public class CustomStatement : Statement<CustomStatement>
	{
		internal const string ParamConcept1 = "#C1#";
		internal const string ParamConcept2 = "#C2#";

		public IConcept Concept1
		{ get; private set; }

		public IConcept Concept2
		{ get; private set; }

		public CustomStatement(string id, IConcept concept1, IConcept concept2)
			: base(
				id,
				new Func<ILanguage, string>(language => language.GetExtension<ILanguageCustomModule>().Statements.Names.Custom),
				new Func<ILanguage, string>(language => language.GetExtension<ILanguageCustomModule>().Statements.Hints.Custom))
		{
			Update(id, concept1, concept2);
		}

		public void Update(String id, IConcept concept1, IConcept concept2)
		{
			if (concept1 == null) throw new ArgumentNullException(nameof(concept1));
			if (concept2 == null) throw new ArgumentNullException(nameof(concept2));
			if (!concept1.HasAttribute<CustomAttribute>()) throw new ArgumentException("C1 has to be marked as CustomAttribute Attribute.", nameof(concept1));
			if (!concept2.HasAttribute<CustomAttribute>()) throw new ArgumentException("C2 has to be marked as CustomAttribute Attribute.", nameof(concept2));

			Update(id);
			Concept1 = concept1;
			Concept2 = concept2;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Concept1;
			yield return Concept2;
		}

		protected override string GetDescriptionTrueText(ILanguage language)
		{
			return language.GetExtension<ILanguageCustomModule>().Statements.TrueFormatStrings.Custom;
		}

		protected override string GetDescriptionFalseText(ILanguage language)
		{
			return language.GetExtension<ILanguageCustomModule>().Statements.TrueFormatStrings.Custom;
		}

		protected override string GetDescriptionQuestionText(ILanguage language)
		{
			return language.GetExtension<ILanguageCustomModule>().Statements.TrueFormatStrings.Custom;
		}

		protected override IDictionary<string, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<string, IKnowledge>
			{
				{ ParamConcept1, Concept1 },
				{ ParamConcept2, Concept2 },
			};
		}

		public override bool Equals(CustomStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Concept1 == Concept1 &&
						other.Concept2 == Concept2;
			}
			else return false;
		}
	}
}
