using System;
using System.Collections.Generic;

using Inventor.Core.Questions;

namespace Inventor.Core
{
	public class QuestionBuilder
	{
		public ISemanticNetwork SemanticNetwork
		{ get; }

		public IEnumerable<IStatement> Preconditions
		{ get; }

		public QuestionBuilder(ISemanticNetwork semanticNetwork, IEnumerable<IStatement> preconditions)
		{
			if (semanticNetwork == null) throw new ArgumentNullException(nameof(semanticNetwork));

			SemanticNetwork = semanticNetwork;
			Preconditions = preconditions;
		}

		public QuestionBuilder Ask()
		{
			return this;
		}
	}

	public static class SubjectQuestionExtensions
	{
		public static QuestionBuilder Ask(this ISemanticNetwork semanticNetwork)
		{
			return new QuestionBuilder(semanticNetwork, null);
		}

		public static QuestionBuilder Supposing(this ISemanticNetwork semanticNetwork, IEnumerable<IStatement> preconditions)
		{
			return new QuestionBuilder(semanticNetwork, preconditions);
		}

		public static IAnswer IsTrueThat(this QuestionBuilder builder, IStatement statement)
		{
			var question = new CheckStatementQuestion(statement, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer HowCompared(this QuestionBuilder builder, IConcept leftValue, IConcept rightValue)
		{
			var question = new ComparisonQuestion(leftValue, rightValue, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhichConceptsBelongToSubjectArea(this QuestionBuilder builder, IConcept area)
		{
			var question = new DescribeSubjectAreaQuestion(area, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhichDescendantsHas(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateChildrenQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhichContainersInclude(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateContainersQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhichPartsHas(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumeratePartsQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhichSignsHas(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateSignsQuestion(concept, true, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer ToWhichSubjectAreasBelongs(this QuestionBuilder builder, IConcept concept)
		{
			var question = new FindSubjectAreaQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IfHasSign(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			var question = new HasSignQuestion(concept, sign, true, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IfHasSigns(this QuestionBuilder builder, IConcept concept)
		{
			var question = new HasSignsQuestion(concept, true, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IfIsPartOf(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			var question = new IsPartOfQuestion(child, parent, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IfIs(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			var question = new IsQuestion(child, parent, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IfIsSign(this QuestionBuilder builder, IConcept concept)
		{
			var question = new IsSignQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IfConceptBelongsToSubjectArea(this QuestionBuilder builder, IConcept concept, IConcept area)
		{
			var question = new IsSubjectAreaQuestion(concept, area, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer IfIsValue(this QuestionBuilder builder, IConcept concept)
		{
			var question = new IsValueQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhatIsMutualSequenceOfProcesses(this QuestionBuilder builder, IConcept processA, IConcept processB)
		{
			var question = new ProcessesQuestion(processA, processB, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhatIsSignValue(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			var question = new SignValueQuestion(concept, sign, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhatIs(this QuestionBuilder builder, IConcept concept)
		{
			var question = new WhatQuestion(concept, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhatInCommon(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			var question = new GetCommonQuestion(concept1, concept2, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhatIsTheDifference(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			var question = new GetDifferencesQuestion(concept1, concept2, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}
	}
}
