using System;

using AabSemantics.Concepts;
using AabSemantics.Localization;

using AabSemantics.Sample09.EntityFramework.Models;

namespace AabSemantics.Sample09.EntityFramework
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var language = Language.Default;

			Console.WriteLine("Initialize DB Context.");
			using (var dbContext = SchoolContext.GenerateSample())
			{
				Console.WriteLine("Wrap it with semantic network.");
				var semanticNetwork = new AabSemantics.Extensions.EF.DbSemanticNetwork<SchoolContext>(
					language,
					new LocalizedStringConstant(l => "DB test"),
					dbContext);

				Console.WriteLine("Map concepts to Courses DB set.");
				semanticNetwork.MapConcepts(
					dbContext.Courses,
					dbCourse => new Concept(dbCourse.CourseID.GetCourseId(), new LocalizedStringConstant(l => dbCourse.Title)),
					course => new Course
					{
						CourseID = course.GetEntityId(),
						Title = course.Name.GetValue(language),
					},
					dbCourse => dbCourse.CourseID.GetCourseId());

				Console.WriteLine("Map concepts to Students DB set.");
				semanticNetwork.MapConcepts(
					dbContext.Students,
					dbStudent => new Concept(dbStudent.ID.GetStudentId(), new LocalizedStringConstant(l => $"{dbStudent.FirstMidName} {dbStudent.LastName}")),
					student => new Student
					{
						ID = student.GetEntityId(),
						FirstMidName = student.Name.GetValue(language).Split(' ')[0],
						LastName = student.Name.GetValue(language).Split(' ')[1],
					},
					dbStudent => dbStudent.ID.GetStudentId());

				Console.WriteLine("Map special statements to Enrollment DB set");
				semanticNetwork.MapStatements(
					dbContext.Enrollments,
					dbEnrollment => new EnrollmentStatement(dbEnrollment, semanticNetwork.Concepts),
					enrollment => enrollment.GetEntity(),
					dbEnrollment => dbEnrollment.EnrollmentID.ToString());

				Console.WriteLine();
				Console.WriteLine("=== Enumerate all knowledge: ===");
				var text = semanticNetwork.DescribeRules();
				var render = TextRenders.PlainString;
				Console.WriteLine(render.Render(text, language).ToString());

				Console.WriteLine("Sample is finished. Press any key to exit app...");
				Console.ReadKey();
			}
		}
	}
}
