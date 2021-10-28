using System;
using System.Linq;
using System.Reflection;

namespace Inventor.Semantics.Contexts
{
	public class SemanticNetworkContext : Context, ISemanticNetworkContext
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
		{ get; }

		public override Boolean IsSystem
		{ get { return false; } }

		#endregion

		public SemanticNetworkContext(ILanguage language, IContext parent, ISemanticNetwork semanticNetwork)
			: base(language, parent)
		{
			SemanticNetwork = semanticNetwork;
		}

		public IQuestionProcessingContext CreateQuestionContext(IQuestion question, ILanguage language = null)
		{
			var concreteContextType = typeof(QuestionProcessingContext<>).MakeGenericType(question.GetType());
			var contextConstructor = concreteContextType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
			var resultContext = contextConstructor.Invoke(new Object[] { this, question, language }) as IQuestionProcessingContext;
			foreach (var statement in question.Preconditions)
			{
				statement.Context = resultContext;
				resultContext.SemanticNetwork.Statements.Add(statement);
			}
			return resultContext;
		}
	}
}