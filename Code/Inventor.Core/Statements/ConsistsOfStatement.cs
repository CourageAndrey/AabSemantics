using System;
using System.Collections.Generic;

using Inventor.Core.Base;

namespace Inventor.Core.Statements
{
	public sealed class ConsistsOfStatement : Statement<ConsistsOfStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept Parent
		{ get { return _parent; } }

		public IConcept Child
		{ get { return _child; } }

		private IConcept _parent;
		private IConcept _child;

		#endregion

		public ConsistsOfStatement(IConcept parent, IConcept child)
			: base(new Func<ILanguage, String>(language => language.StatementNames.Composition), new Func<ILanguage, String>(language => language.StatementHints.Composition))
		{
			Update(parent, child);
		}

		public void Update(IConcept parent, IConcept child)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			if (child == null) throw new ArgumentNullException("child");

			_parent = parent;
			_child = child;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return _parent;
			yield return _child;
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
				{ "#PARENT#", _parent },
				{ "#CHILD#", _child },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(ConsistsOfStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other._parent == _parent &&
						other._child == _child;
			}
			else return false;
		}

		#endregion
	}
}
