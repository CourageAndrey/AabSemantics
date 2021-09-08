using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
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
			: base(id, new Func<ILanguage, String>(language => language.Statements.Names.Composition), new Func<ILanguage, String>(language => language.Statements.Hints.Composition))
		{
			Update(id, whole, part);
		}

		public void Update(String id, IConcept whole, IConcept part)
		{
			if (whole == null) throw new ArgumentNullException(nameof(whole));
			if (part == null) throw new ArgumentNullException(nameof(part));

			Update(id);
			Whole = whole;
			Part = part;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Whole;
			yield return Part;
		}

		#region Description

		protected override String GetDescriptionText(ILanguageStatementsPart language)
		{
			return language.Composition;
		}

		protected override IDictionary<String, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamParent, Whole },
				{ Strings.ParamChild, Part },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(HasPartStatement other)
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
