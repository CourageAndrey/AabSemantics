using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IQuestion
	{
		ICollection<IStatement> Preconditions
		{ get; }

		IAnswer Ask(IKnowledgeBaseContext context);
	}

	public interface IQuestion<StatementT> : IQuestion
		where StatementT : IStatement
	{
	}
}
