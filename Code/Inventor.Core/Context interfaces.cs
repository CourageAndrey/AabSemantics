using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IContext
	{
		ILanguage Language
		{ get; }

		ICollection<IStatement> Scope
		{ get; }

		IContext Parent
		{ get; }

		ICollection<IContext> ActiveContexts
		{ get; }

		ICollection<IContext> Children
		{ get; }

		Boolean IsSystem
		{ get; }
	}

	public interface ISystemContext : IContext
	{
		ISemanticNetworkContext Instantiate(ISemanticNetwork semanticNetwork, IStatementRepository statementRepository, IQuestionRepository questionRepository, IAttributeRepository attributeRepository);
	}

	public interface ISemanticNetworkContext : IContext
	{
		ISemanticNetwork SemanticNetwork
		{ get; }

		IStatementRepository StatementRepository
		{ get; }

		IQuestionRepository QuestionRepository
		{ get; }

		IAttributeRepository AttributeRepository
		{ get; }

		IQuestionProcessingContext CreateQuestionContext(IQuestion question, ILanguage language = null);
	}

	public interface IQuestionProcessingContext : ISemanticNetworkContext, IDisposable
	{
		IQuestion Question
		{ get; }
	}

	public interface IQuestionProcessingContext<out QuestionT> : IQuestionProcessingContext
		where QuestionT : IQuestion
	{
		new QuestionT Question
		{ get; }
	}
}
