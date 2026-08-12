using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Statements
{
	public class IsStatement : Statement<IsStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept Ancestor
		{ get; private set; }

		public IConcept Descendant
		{ get; private set; }

		public IConcept Parent
		{ get { return Ancestor; } }

		public IConcept Child
		{ get { return Descendant; } }

		#endregion

		public IsStatement(String id, IConcept ancestor, IConcept descendant)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageClassificationModule, ILanguageStatements>().Names.Classification),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageClassificationModule, ILanguageStatements>().Hints.Classification))
		{
			Update(id, ancestor, descendant);
		}

		public void Update(String id, IConcept ancestor, IConcept descendant)
		{
			Update(id);
			Ancestor = ancestor.EnsureNotNull(nameof(ancestor));
			Descendant = descendant.EnsureNotNull(nameof(descendant));
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Ancestor;
			yield return Descendant;
		}

		#region Consistency checking

		public override System.Boolean Equals(IsStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Ancestor == Ancestor &&
						other.Descendant == Descendant;
			}
			else return false;
		}

		public async Task<System.Boolean> CheckCyclicAsync(IEnumerable<IsStatement> statements)
		{
			var path = await statements.FindPathAsync(typeof(IsStatement), Child, Parent);
			return !path.Any();
		}

		#endregion
	}

	public static class IsStatementExtensions
	{
		public static System.Boolean CheckCyclic(this IsStatement statement, IEnumerable<IsStatement> statements)
		{
			return TaskHelper.AwaitDetached(() => statement.CheckCyclicAsync(statements));
		}
	}
}
