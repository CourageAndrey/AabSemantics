using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Utils;

namespace Inventor.Core
{
	public interface IKnowledgeBase : IKnowledge, IChangeable
	{
		ICollection<IConcept> Concepts
		{ get; }

		ICollection<IStatement> Statements
		{ get; }

		IQuestionRepository QuestionRepository
		{ get; }

		IQuestionProcessingContext AskQuestion(IQuestion question);

		IEnumerable<IKnowledge> EnumerateKnowledge(Func<IContext, Boolean> contextFilter);
		void Add(IKnowledge knowledge);
		Boolean Remove(IKnowledge knowledge);

		event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;
		event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;
		event EventHandler<ItemEventArgs<IStatement>> StatementAdded;
		event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;

		IConcept True
		{ get; }

		IConcept False
		{ get; }

		void Save(String fileName);
	}

	public static class KnowledgeBaseHelper
	{
		#region Context helpers

		public static IEnumerable<IKnowledge> EnumerateKnowledge(this IKnowledgeBase knowledgeBase)
		{
			return knowledgeBase.EnumerateKnowledge(context => true);
		}

		public static IEnumerable<IKnowledge> EnumerateKnowledge(this IKnowledgeBase knowledgeBase, IContext certainContext)
		{
			return knowledgeBase.EnumerateKnowledge(context => context == certainContext);
		}

		public static IEnumerable<IKnowledge> EnumerateKnowledge(this IKnowledgeBase knowledgeBase, ICollection<IContext> validContexts)
		{
			return knowledgeBase.EnumerateKnowledge(context => validContexts.Contains(context));
		}

		#endregion

		public static FormattedText DescribeRules(this IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var result = new FormattedText();
			foreach (var statement in knowledgeBase.Statements)
			{
				result.Add(statement.DescribeTrue(language));
			}
			return result;
		}

		public static FormattedText CheckConsistensy(this IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var result = new FormattedText();

			// 1. check all duplicates
			foreach (var statement in knowledgeBase.Statements)
			{
				if (!statement.CheckUnique(knowledgeBase.Statements))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorDuplicate,
						new Dictionary<String, INamed> { { "#STATEMENT#", statement } });
				}
			}

			// 2. check cyclic parents
			var clasifications = knowledgeBase.Statements.OfType<IsStatement>().ToList();
			foreach (var clasification in clasifications)
			{
				if (!clasification.CheckCyclic(clasifications))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorCyclic,
						new Dictionary<String, INamed> { { "#STATEMENT#", clasification } });
				}
			}

			// 4. check multi values
			var signValues = knowledgeBase.Statements.OfType<SignValueStatement>().ToList();
			foreach (var concept in knowledgeBase.Concepts)
			{
				var parents = clasifications.GetParentsOneLevel(concept);
				foreach (var sign in HasSignStatement.GetSigns(knowledgeBase.Statements, concept, true))
				{
					if (signValues.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
						parents.Select(p => SignValueStatement.GetSignValue(knowledgeBase.Statements, p, sign.Sign)).Count(r => r != null) > 1)
					{
						result.Add(
							() => language.Misc.ConsistencyErrorMultipleSignValue,
							new Dictionary<String, INamed>
							{
								{ "#CONCEPT#", concept },
								{ "#SIGN#", sign.Sign },
							});
					}
				}
			}

			// 5. check values without sign
			foreach (var signValue in signValues)
			{
				if (!signValue.CheckHasSign(knowledgeBase.Statements))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorSignWithoutValue,
						new Dictionary<String, INamed> { { "#STATEMENT#", signValue } });
				}
			}

			// 6. check sign duplications
			var hasSigns = knowledgeBase.Statements.OfType<HasSignStatement>().ToList();
			foreach (var hasSign in hasSigns)
			{
				if (!hasSign.CheckSignDuplication(hasSigns, clasifications))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorMultipleSign,
						new Dictionary<String, INamed> { { "#STATEMENT#", hasSign } });
				}
			}

			if (result.LinesCount == 0)
			{
				result.Add(() => language.Misc.CheckOk, new Dictionary<String, INamed>());
			}
			return result;
		}
	}
}
