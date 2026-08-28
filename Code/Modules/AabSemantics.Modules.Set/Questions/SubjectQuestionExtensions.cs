using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Fluent verbs for the set module's questions.</summary>
	public static class SubjectQuestionExtensions
	{
		/// <summary>Asks which concepts a subject area contains.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="area">Subject area concept.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> WhichConceptsBelongToSubjectAreaAsync(this QuestionBuilder builder, IConcept area)
		{
			var question = new DescribeSubjectAreaQuestion(area, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks which concepts a concept is part of.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> WhichContainersIncludeAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateContainersQuestion(concept, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks which concepts a concept consists of.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> WhichPartsHasAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumeratePartsQuestion(concept, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks which signs a concept has.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> WhichSignsHasAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new EnumerateSignsQuestion(concept, true, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks which subject areas a concept belongs to.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> ToWhichSubjectAreasBelongsAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new FindSubjectAreaQuestion(concept, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks whether a concept has a given sign.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <param name="sign">Sign concept.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> IfHasSignAsync(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			var question = new HasSignQuestion(concept, sign, true, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks whether a concept has any signs at all.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> IfHasSignsAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new HasSignsQuestion(concept, true, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks whether one concept is part of another.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="child">The child.</param>
		/// <param name="parent">The parent.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> IfIsPartOfAsync(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			var question = new IsPartOfQuestion(child, parent, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks whether a concept is used as a sign.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> IfIsSignAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new IsSignQuestion(concept, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks whether a concept belongs to a subject area.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <param name="area">Subject area concept.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> IfConceptBelongsToSubjectAreaAsync(this QuestionBuilder builder, IConcept concept, IConcept area)
		{
			var question = new IsSubjectAreaQuestion(concept, area, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks whether a concept is used as a sign value.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> IfIsValueAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new IsValueQuestion(concept, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks what value a concept's sign has.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <param name="sign">Sign concept.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> WhatIsSignValueAsync(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			var question = new SignValueQuestion(concept, sign, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks for a full description of a concept.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> WhatIsAsync(this QuestionBuilder builder, IConcept concept)
		{
			var question = new WhatQuestion(concept, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks which properties two concepts share.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept1">First compared concept.</param>
		/// <param name="concept2">Second compared concept.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> WhatInCommonAsync(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			var question = new GetCommonQuestion(concept1, concept2, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Asks which properties two concepts differ in.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept1">First compared concept.</param>
		/// <param name="concept2">Second compared concept.</param>
		/// <returns>The answer.</returns>
		public static Task<IAnswer> WhatIsTheDifferenceAsync(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			var question = new GetDifferencesQuestion(concept1, concept2, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Blocking counterpart of <see cref="WhichConceptsBelongToSubjectAreaAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="area">Subject area concept.</param>
		/// <returns>The answer.</returns>
		public static IAnswer WhichConceptsBelongToSubjectArea(this QuestionBuilder builder, IConcept area)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichConceptsBelongToSubjectAreaAsync(area));
		}

		/// <summary>Blocking counterpart of <see cref="WhichContainersIncludeAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static IAnswer WhichContainersInclude(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichContainersIncludeAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="WhichPartsHasAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static IAnswer WhichPartsHas(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichPartsHasAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="WhichSignsHasAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static IAnswer WhichSignsHas(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhichSignsHasAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="ToWhichSubjectAreasBelongsAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static IAnswer ToWhichSubjectAreasBelongs(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.ToWhichSubjectAreasBelongsAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="IfHasSignAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <param name="sign">Sign concept.</param>
		/// <returns>The answer.</returns>
		public static IAnswer IfHasSign(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			return TaskHelper.AwaitDetached(() => builder.IfHasSignAsync(concept, sign));
		}

		/// <summary>Blocking counterpart of <see cref="IfHasSignsAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static IAnswer IfHasSigns(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.IfHasSignsAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="IfIsPartOfAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="child">The child.</param>
		/// <param name="parent">The parent.</param>
		/// <returns>The answer.</returns>
		public static IAnswer IfIsPartOf(this QuestionBuilder builder, IConcept child, IConcept parent)
		{
			return TaskHelper.AwaitDetached(() => builder.IfIsPartOfAsync(child, parent));
		}

		/// <summary>Blocking counterpart of <see cref="IfIsSignAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static IAnswer IfIsSign(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.IfIsSignAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="IfConceptBelongsToSubjectAreaAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <param name="area">Subject area concept.</param>
		/// <returns>The answer.</returns>
		public static IAnswer IfConceptBelongsToSubjectArea(this QuestionBuilder builder, IConcept concept, IConcept area)
		{
			return TaskHelper.AwaitDetached(() => builder.IfConceptBelongsToSubjectAreaAsync(concept, area));
		}

		/// <summary>Blocking counterpart of <see cref="IfIsValueAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static IAnswer IfIsValue(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.IfIsValueAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="WhatIsSignValueAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <param name="sign">Sign concept.</param>
		/// <returns>The answer.</returns>
		public static IAnswer WhatIsSignValue(this QuestionBuilder builder, IConcept concept, IConcept sign)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatIsSignValueAsync(concept, sign));
		}

		/// <summary>Blocking counterpart of <see cref="WhatIsAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The answer.</returns>
		public static IAnswer WhatIs(this QuestionBuilder builder, IConcept concept)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatIsAsync(concept));
		}

		/// <summary>Blocking counterpart of <see cref="WhatInCommonAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept1">First compared concept.</param>
		/// <param name="concept2">Second compared concept.</param>
		/// <returns>The answer.</returns>
		public static IAnswer WhatInCommon(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatInCommonAsync(concept1, concept2));
		}

		/// <summary>Blocking counterpart of <see cref="WhatIsTheDifferenceAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="concept1">First compared concept.</param>
		/// <param name="concept2">Second compared concept.</param>
		/// <returns>The answer.</returns>
		public static IAnswer WhatIsTheDifference(this QuestionBuilder builder, IConcept concept1, IConcept concept2)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatIsTheDifferenceAsync(concept1, concept2));
		}
	}
}
