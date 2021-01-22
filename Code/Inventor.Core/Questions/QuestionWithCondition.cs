using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class QuestionWithCondition : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConditions")]
		public ICollection<IStatement> Conditions
		{ get; set; } = new ObservableCollection<IStatement>();
#warning Get rid of concrete type here.

		[PropertyDescriptor(true, "QuestionNames.ParamQuestion")]
		public IQuestion Question
		{ get; set; }
	}
}
