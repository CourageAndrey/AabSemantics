using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Statements;
using Inventor.Set.Localization;

namespace Inventor.Set.Statements
{
	public class GroupStatement : Statement<GroupStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept Area
		{ get; private set; }

		public IConcept Concept
		{ get; private set; }

		public IConcept Parent
		{ get { return Area; } }

		public IConcept Child
		{ get { return Concept; } }

		#endregion

		public GroupStatement(String id, IConcept area, IConcept concept)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageSetModule>().Statements.Names.SubjectArea),
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageSetModule>().Statements.Hints.SubjectArea))
		{
			Update(id, area, concept);
		}

		public void Update(String id, IConcept area, IConcept concept)
		{
			if (area == null) throw new ArgumentNullException(nameof(area));
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Update(id);
			Area = area;
			Concept = concept;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Area;
			yield return Concept;
		}

		#region Description

		protected override String GetDescriptionTrueText(ILanguage language)
		{
			return language.GetExtension<ILanguageSetModule>().Statements.TrueFormatStrings.SubjectArea;
		}

		protected override String GetDescriptionFalseText(ILanguage language)
		{
			return language.GetExtension<ILanguageSetModule>().Statements.TrueFormatStrings.SubjectArea;
		}

		protected override String GetDescriptionQuestionText(ILanguage language)
		{
			return language.GetExtension<ILanguageSetModule>().Statements.TrueFormatStrings.SubjectArea;
		}

		protected override IDictionary<String, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamArea, Area },
				{ Semantics.Localization.Strings.ParamConcept, Concept },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(GroupStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Area == Area &&
						other.Concept == Concept;
			}
			else return false;
		}

		#endregion
	}
}
