using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Sample05.CustomStatement
{
	public class GetTallerQuestion : Question
	{
		public IConcept Person
		{ get; }

		public GetTallerQuestion(IConcept person)
		{
			Person = person.EnsureNotNull(nameof(person));
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<GetTallerQuestion, IsTallerThanStatement>()
				.WithTransitives(
					statements => Task.FromResult(true),
					c => c.SemanticNetwork.Statements
						.OfType<IsTallerThanStatement>()
						.Where(s => s.ShorterPerson == c.Question.Person)
						.Select(s => new NestedQuestion(
							new GetTallerQuestion(s.TallerPerson),
							new IStatement[] { s })),
					true)
				.Where(s => s.ShorterPerson == Person)
				.SelectAllConceptsAsync(
					statement => statement.TallerPerson,
					question => question.Person,
					"#SHORTER#",
					language => $"#SHORTER# is shorter than",
					concepts => concepts.Distinct());
		}
	}
}
