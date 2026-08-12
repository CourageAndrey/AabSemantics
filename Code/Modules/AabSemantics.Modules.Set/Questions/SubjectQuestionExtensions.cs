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
			return TaskHelper.AwaitDetached(() => builder.WhichConceptsBelongToSubjectAreaAsync(area));
		}

		public static IAnswer WhichContainersInclude(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichContainersIncludeAsync(concept));
		}

		public static IAnswer WhichPartsHas(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichPartsHasAsync(concept));
		}

		public static IAnswer WhichSignsHas(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichSignsHasAsync(concept));
		}

		public static IAnswer ToWhichSubjectAreasBelongs(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.ToWhichSubjectAreasBelongsAsync(concept));
		}

		public static IAnswer IfHasSign(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			return TaskHelper.AwaitDetached(() => builder.IfHasSignAsync(concept, sign));
		}

		public static IAnswer IfHasSigns(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.IfHasSignsAsync(concept));
		}

		public static IAnswer IfIsPartOf(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			return TaskHelper.AwaitDetached(() => builder.IfIsPartOfAsync(child, parent));
		}

		public static IAnswer IfIsSign(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.IfIsSignAsync(concept));
		}

		public static IAnswer IfConceptBelongsToSubjectArea(this QuestionBuilder builder, IConcept concept, IConcept area)
		{
			return TaskHelper.AwaitDetached(() => builder.IfConceptBelongsToSubjectAreaAsync(concept, area));
		}

		public static IAnswer IfIsValue(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.IfIsValueAsync(concept));
		}

		public static IAnswer WhatIsSignValue(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatIsSignValueAsync(concept, sign));
		}

		public static IAnswer WhatIs(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatIsAsync(concept));
		}

		public static IAnswer WhatInCommon(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatInCommonAsync(concept1, concept2));
		}

		public static IAnswer WhatIsTheDifference(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatIsTheDifferenceAsync(concept1, concept2));
		}
	}
}
