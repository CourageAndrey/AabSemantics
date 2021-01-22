using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class QuestionWithCondition : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConditions")]
		public ICollection<IStatement> Conditions
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamQuestion")]
		public IQuestion Question
		{ get; set; }
	}
}
