using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;

using AabSemantics.Sample09.EntityFramework.Models;

namespace AabSemantics.Sample09.EntityFramework
{
	[DbConfigurationType(typeof(SchoolContextConfiguration))]
	public class SchoolContext : DbContext
	{
		public SchoolContext() : base("SchoolContext")
		{
		}

		public DbSet<Student> Students { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }
		public DbSet<Course> Courses { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

		public static SchoolContext GenerateSample()
		{
			var context = new SchoolContext();

			var students = new List<Student>
			{
				new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
				new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
				new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
				new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
				new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
				new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
				new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
				new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
			};
			var storedStudents = context.Students
				.Select(student => new { student.FirstMidName, student.LastName })
				.ToList()
				.Select(student => $"{student.FirstMidName} {student.LastName}")
				.ToHashSet();
			foreach (var student in students)
			{
				if (storedStudents.Add($"{student.FirstMidName} {student.LastName}"))
				{
					context.Students.Add(student);
				}
			}
			context.SaveChanges();

			var courses = new List<Course>
			{
				new Course{CourseID=1050,Title="Chemistry",Credits=3,},
				new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
				new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
				new Course{CourseID=1045,Title="Calculus",Credits=4,},
				new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
				new Course{CourseID=2021,Title="Composition",Credits=3,},
				new Course{CourseID=2042,Title="Literature",Credits=4,}
			};
			var storedCourses = context.Courses.Select(course => course.CourseID).ToHashSet();
			foreach (var course in courses)
			{
				if (storedCourses.Add(course.CourseID))
				{
					context.Courses.Add(course);
				}
			}
			context.SaveChanges();

			var enrollments = new List<(String LastName, Int32 CourseID, Grade? Grade)>
			{
				("Alexander", 1050, Grade.A),
				("Alexander", 4022, Grade.C),
				("Alexander", 4041, Grade.B),
				("Alonso", 1045, Grade.B),
				("Alonso", 3141, Grade.F),
				("Alonso", 2021, Grade.F),
				("Anand", 1050, null),
				("Barzdukas", 1050, null),
				("Barzdukas", 4022, Grade.F),
				("Li", 4041, Grade.C),
				("Justice", 1045, null),
				("Norman", 3141, Grade.A),
			};
			var studentIdsByLastName = context.Students.ToDictionary(student => student.LastName, student => student.ID);
			var storedEnrollments = context.Enrollments
				.Select(enrollment => new { enrollment.StudentID, enrollment.CourseID })
				.ToList()
				.Select(enrollment => (enrollment.StudentID, enrollment.CourseID))
				.ToHashSet();
			foreach (var enrollment in enrollments)
			{
				var studentId = studentIdsByLastName[enrollment.LastName];
				if (storedEnrollments.Add((studentId, enrollment.CourseID)))
				{
					context.Enrollments.Add(new Enrollment
					{
						StudentID = studentId,
						CourseID = enrollment.CourseID,
						Grade = enrollment.Grade,
					});
				}
			}
			context.SaveChanges();

			return context;
		}
	}
}
