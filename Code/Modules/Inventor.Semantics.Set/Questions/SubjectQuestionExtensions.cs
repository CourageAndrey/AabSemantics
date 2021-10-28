using Inventor.Semantics;
using Inventor.Semantics.Questions;

namespace Inventor.Semantics.Set.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static IAnswer WhichConceptsBelongToSubjectArea(this QuestionBuilder builder, IConcept area)
		{
			var question = new DescribeSubjectAreaQuestion(area, builder.Preconditions);
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
