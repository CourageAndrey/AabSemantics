using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static async Task<IAnswer> WhichConceptsBelongToSubjectAreaAsync(this QuestionBuilder builder, IConcept area)
		{
			var question = new DescribeSubjectAreaQuestion(area, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> WhichContainersIncludeAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateContainersQuestion(concept, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> WhichPartsHasAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumeratePartsQuestion(concept, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> WhichSignsHasAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateSignsQuestion(concept, true, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> ToWhichSubjectAreasBelongsAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new FindSubjectAreaQuestion(concept, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> IfHasSignAsync(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			var question = new HasSignQuestion(concept, sign, true, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> IfHasSignsAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new HasSignsQuestion(concept, true, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> IfIsPartOfAsync(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			var question = new IsPartOfQuestion(child, parent, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> IfIsSignAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new IsSignQuestion(concept, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> IfConceptBelongsToSubjectAreaAsync(this QuestionBuilder builder, IConcept concept, IConcept area)
		{
			var question = new IsSubjectAreaQuestion(concept, area, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> IfIsValueAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new IsValueQuestion(concept, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> WhatIsSignValueAsync(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			var question = new SignValueQuestion(concept, sign, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> WhatIsAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new WhatQuestion(concept, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> WhatInCommonAsync(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			var question = new GetCommonQuestion(concept1, concept2, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static async Task<IAnswer> WhatIsTheDifferenceAsync(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			var question = new GetDifferencesQuestion(concept1, concept2, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context);
		}

		public static IAnswer WhichConceptsBelongToSubjectArea(this QuestionBuilder builder, IConcept area)
		{
			return builder.WhichConceptsBelongToSubjectAreaAsync(area).Await();
		}

		public static IAnswer WhichContainersInclude(this QuestionBuilder builder, IConcept concept)
		{
			return builder.WhichContainersIncludeAsync(concept).Await();
		}

		public static IAnswer WhichPartsHas(this QuestionBuilder builder, IConcept concept)
		{
			return builder.WhichPartsHasAsync(concept).Await();
		}

		public static IAnswer WhichSignsHas(this QuestionBuilder builder, IConcept concept)
		{
			return builder.WhichSignsHasAsync(concept).Await();
		}

		public static IAnswer ToWhichSubjectAreasBelongs(this QuestionBuilder builder, IConcept concept)
		{
			return builder.ToWhichSubjectAreasBelongsAsync(concept).Await();
		}

		public static IAnswer IfHasSign(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			return builder.IfHasSignAsync(concept, sign).Await();
		}

		public static IAnswer IfHasSigns(this QuestionBuilder builder, IConcept concept)
		{
			return builder.IfHasSignsAsync(concept).Await();
		}

		public static IAnswer IfIsPartOf(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			return builder.IfIsPartOfAsync(child, parent).Await();
		}

		public static IAnswer IfIsSign(this QuestionBuilder builder, IConcept concept)
		{
			return builder.IfIsSignAsync(concept).Await();
		}

		public static IAnswer IfConceptBelongsToSubjectArea(this QuestionBuilder builder, IConcept concept, IConcept area)
		{
			return builder.IfConceptBelongsToSubjectAreaAsync(concept, area).Await();
		}

		public static IAnswer IfIsValue(this QuestionBuilder builder, IConcept concept)
		{
			return builder.IfIsValueAsync(concept).Await();
		}

		public static IAnswer WhatIsSignValue(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			return builder.WhatIsSignValueAsync(concept, sign).Await();
		}

		public static IAnswer WhatIs(this QuestionBuilder builder, IConcept concept)
		{
			return builder.WhatIsAsync(concept).Await();
		}

		public static IAnswer WhatInCommon(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			return builder.WhatInCommonAsync(concept1, concept2).Await();
		}

		public static IAnswer WhatIsTheDifference(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			return builder.WhatIsTheDifferenceAsync(concept1, concept2).Await();
		}
	}
}
