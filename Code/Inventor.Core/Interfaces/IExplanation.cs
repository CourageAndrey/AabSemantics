using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IExplanation
	{
		ICollection<IStatement> Statements
		{ get; }
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
