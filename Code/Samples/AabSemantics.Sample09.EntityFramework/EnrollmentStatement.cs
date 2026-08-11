using System;
using System.Collections.Generic;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Statements;

using AabSemantics.Sample09.EntityFramework.Models;

namespace AabSemantics.Sample09.EntityFramework
{
	internal class EnrollmentStatement : Statement<EnrollmentStatement>
	{
		#region Description parameters

		private const String ParamStudent = "#STUDENT#";
		private const String ParamCourse = "#COURSE#";
		private const String ParamGrade = "#GRADE#";

		private const String NoGrade = "none";

		#endregion

		#region Properties

		public IConcept Course
		{ get; }

		public IConcept Student
		{ get; }

		public Grade? Grade
		{ get; }

		#endregion

		public EnrollmentStatement(Enrollment enrollment, IRepository<IConcept> concepts)
			: base(
				enrollment.EnrollmentID.ToString(),
				new LocalizedStringConstant(language => $"[{enrollment.EnrollmentID}] {enrollment.StudentID}[{enrollment.CourseID}]={enrollment.Grade}"))
		{
			Course = concepts.GetItem(enrollment.CourseID.GetCourseId());
			Student = concepts.GetItem(enrollment.StudentID.GetStudentId());
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

		public override bool Equals(EnrollmentStatement other)
		{
			return ID == other.ID;
		}

		#endregion

		#region Metadata

		public static void RegisterMetadata()
		{
			Repositories.RegisterStatement(
				typeof(EnrollmentStatement),
				language => "Enrollment",
				language => $"{ParamStudent} is enrolled to {ParamCourse} with grade {ParamGrade}.",
				language => $"{ParamStudent} is not enrolled to {ParamCourse} with grade {ParamGrade}.",
				language => $"Is {ParamStudent} enrolled to {ParamCourse} with grade {ParamGrade}?",
				statement => ((EnrollmentStatement) statement).getDescriptionParameters(),
				StatementDefinition.NoConsistencyCheck);
		}

		private IDictionary<String, IKnowledge> getDescriptionParameters()
		{
			return new Dictionary<String, IKnowledge>
			{
				{ ParamStudent, Student },
				{ ParamCourse, Course },
				{ ParamGrade, new Concept($"G{Grade?.ToString() ?? NoGrade}", new LocalizedStringConstant(language => Grade?.ToString() ?? NoGrade)) },
			};
		}

		#endregion
	}
}
