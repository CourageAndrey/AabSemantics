using System.Collections.Generic;

using AabSemantics.Localization;
using AabSemantics.Statements;

using AabSemantics.Sample09.EntityFramework.Models;

namespace AabSemantics.Sample09.EntityFramework
{
	internal class EnrollmentStatement : Statement<EnrollmentStatement>
	{
		#region Properties

		public IConcept Course
		{ get; }

		public IConcept Student
		{ get; }

		public Grade? Grade
		{ get; }

		#endregion

		public EnrollmentStatement(Enrollment enrollment, IKeyedCollection<IConcept> concepts)
			: base(
				enrollment.EnrollmentID.ToString(),
				new LocalizedStringConstant(language => $"[{enrollment.EnrollmentID}] {enrollment.StudentID}[{enrollment.CourseID}]={enrollment.Grade}"))
		{
			Course = concepts[enrollment.CourseID.GetCourseId()];
			Student = concepts[enrollment.StudentID.GetStudentId()];
			Grade = enrollment.Grade;
		}

		public Enrollment GetEntity()
		{
			return new Enrollment
			{
				EnrollmentID = int.Parse(ID),
				StudentID = Student.GetEntityId(),
				CourseID = Course.GetEntityId(),
				Grade = Grade,
			};
		}

		#region Overrides

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Course;
			yield return Student;
		}

		protected override string GetDescriptionTrueText(ILanguage language)
		{
			return Grade.HasValue
				? $"{Student.Name.GetValue(language)} has {Course.Name.GetValue(language)} enrollment grade {Grade}."
				: $"{Student.Name.GetValue(language)} is enrolled to {Course.Name.GetValue(language)}.";
		}

		protected override string GetDescriptionFalseText(ILanguage language)
		{
			return Grade.HasValue
				? $"{Student.Name.GetValue(language)} has not {Course.Name.GetValue(language)} enrollment grade {Grade}."
				: $"{Student.Name.GetValue(language)} isn't enrolled to {Course.Name.GetValue(language)}.";
		}

		protected override string GetDescriptionQuestionText(ILanguage language)
		{
			return Grade.HasValue
				? $"Has {Student.Name.GetValue(language)} {Course.Name.GetValue(language)} enrollment grade {Grade}?"
				: $"Is {Student.Name.GetValue(language)} enrolled to {Course.Name.GetValue(language)}?";
		}

		protected override IDictionary<string, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<string, IKnowledge>();
		}

		public override bool Equals(EnrollmentStatement other)
		{
			// This simplicity is enough here.
			return ID == other.ID;
		}

		#endregion
	}
}
