using Inventor.Semantics;

namespace Samples.Semantics.Sample09.EntityFramework.Models
{
	internal static class IdHelper
	{
		public static int GetEntityId(this IIdentifiable identifiable)
		{
			return int.Parse(identifiable.ID.Substring(1));
		}

		public static string GetCourseId(this int id)
		{
			return $"C{id}";
		}

		public static string GetStudentId(this int id)
		{
			return $"S{id}";
		}
	}
}
