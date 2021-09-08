﻿namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetCommonQuestion : QuestionViewModel<Core.Questions.GetCommonQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept1")]
		public Core.IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Names.ParamConcept2")]
		public Core.IConcept Concept2
		{ get; set; }

		public override Core.Questions.GetCommonQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.GetCommonQuestion(Concept1, Concept2);
		}
	}
}
