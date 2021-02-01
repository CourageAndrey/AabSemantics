using System;
using System.Collections.Generic;

using Inventor.Core.Base;
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

		public HasPartStatement(IConcept whole, IConcept part)
			: base(new Func<ILanguage, String>(language => language.StatementNames.Composition), new Func<ILanguage, String>(language => language.StatementHints.Composition))
		{
			Update(whole, part);
		}

		public void Update(IConcept whole, IConcept part)
		{
			if (whole == null) throw new ArgumentNullException(nameof(whole));
			if (part == null) throw new ArgumentNullException(nameof(part));

			Whole = whole;
			Part = part;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Whole;
			yield return Part;
		}

		#region Description

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.Composition;
		}

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
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
