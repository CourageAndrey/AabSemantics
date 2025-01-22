using System;
using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Statements
{
	public class HasPartStatement : Statement<HasPartStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept Whole
		{ get; private set; }

		public IConcept Part
		{ get; private set; }

		public IConcept Parent
		{ get { return Whole; } }

		public IConcept Child
		{ get { return Part; } }

		#endregion

		public HasPartStatement(String id, IConcept whole, IConcept part)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageSetModule>().Statements.Names.Composition),
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageSetModule>().Statements.Hints.Composition))
		{
			Update(id, whole, part);
		}

		public void Update(String id, IConcept whole, IConcept part)
		{
			Update(id);
			Whole = whole.EnsureNotNull(nameof(whole));
			Part = part.EnsureNotNull(nameof(part));
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Whole;
			yield return Part;
		}

		#region Consistency checking

		public override System.Boolean Equals(HasPartStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Whole == Whole &&
						other.Part == Part;
			}
			else return false;
		}

		#endregion
	}
}
