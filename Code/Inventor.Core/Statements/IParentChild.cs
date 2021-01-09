using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Statements
{
	public interface IParentChild<out T>
	{
		T Parent { get; }

		T Child { get; }
	}

	public static class ParentChildHelper
	{
		public static List<T> GetParentsTree<T, RelationshipT>(this IEnumerable<Statement> statements, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetParentsTree(statements.OfType<RelationshipT>(), concept);
		}

		public static List<T> GetParentsPlainList<T, RelationshipT>(this IEnumerable<Statement> statements, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetParentsPlainList(statements.OfType<RelationshipT>(), concept);
		}

		public static List<T> GetParentsTree<T, RelationshipT>(this IEnumerable<RelationshipT> clasifications, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			var result = new List<T>();
			foreach (var parent in GetParentsPlainList(clasifications, concept))
			{
				var list = new List<T> { parent };
				list.AddRange(GetParentsTree(clasifications, parent));
				list.RemoveAll(result.Contains);
				result.AddRange(list);
			}
			return result;
		}

		public static List<T> GetParentsPlainList<T, RelationshipT>(this IEnumerable<RelationshipT> clasifications, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return clasifications.Where(c => c.Child == concept).Select(c => c.Parent).ToList();
		}
	}
}
