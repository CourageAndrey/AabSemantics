using System;
using System.Linq;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Questions;
using Inventor.Core.Text.Primitives;

namespace Samples._04.CustomQuestion
{
	public class SelectRandomConceptQuestion : Question
	{
		public override IAnswer Process(IQuestionProcessingContext context)
		{
			var random = new Random(DateTime.Now.Millisecond);
			var allConcepts = context.SemanticNetwork.Concepts.ToList();
			var resultConcept = allConcepts[random.Next(allConcepts.Count)];

			return new ConceptAnswer(
				resultConcept,
				new FormattedText(language => resultConcept.Name.GetValue(language)),
				new Explanation(Array.Empty<IStatement>()));
		}
	}

	public static class SubjectQuestionExtensions
	{
		public static IAnswer ForRandomConcept(this QuestionBuilder builder)
		{
			var question = new SelectRandomConceptQuestion();
			return question.Ask(builder.SemanticNetwork.Context);
		}
	}
}
