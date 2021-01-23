using System;
using System.Collections.Generic;

using Inventor.Core.Base;

namespace Inventor.Core.Statements
{
	public sealed class ConsistsOfStatement : Statement<ConsistsOfStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept Parent
		{ get; private set; }

		public IConcept Child
		{ get; private set; }

		#endregion

		public ConsistsOfStatement(IConcept parent, IConcept child)
			: base(new Func<ILanguage, String>(language => language.StatementNames.Composition), new Func<ILanguage, String>(language => language.StatementHints.Composition))
		{
			Update(parent, child);
		}

		public void Update(IConcept parent, IConcept child)
		{
			if (parent == null) throw new ArgumentNullException(nameof(parent));
			if (child == null) throw new ArgumentNullException(nameof(child));

			Parent = parent;
			Child = child;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Parent;
			yield return Child;
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
				{ "#PARENT#", Parent },
				{ "#CHILD#", Child },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(ConsistsOfStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Parent == Parent &&
						other.Child == Child;
			}
			else return false;
		}

		#endregion
	}
}
