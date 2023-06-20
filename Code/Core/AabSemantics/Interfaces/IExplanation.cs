using System.Collections.Generic;

namespace AabSemantics
{
	public interface IExplanation
	{
		ICollection<IStatement> Statements
		{ get; }
	}

	public class Explanation : IExplanation
	{
		#region Properties

		public ICollection<IStatement> Statements
		{ get; }

		#endregion

		public Explanation(IEnumerable<IStatement> statements)
		{
			Statements = new List<IStatement>(statements);
		}

		public Explanation(IStatement statement)
			: this(new List<IStatement> { statement })
		{ }
	}

	public static class ExplanationExtensions
	{
		public static void Expand(this IExplanation explanation, IEnumerable<IStatement> statements)
		{
			foreach (var statement in statements)
			{
				explanation.Statements.Add(statement);
			}
		}
	}
}
