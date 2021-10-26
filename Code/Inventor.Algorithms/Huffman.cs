using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Algorithms
{
	public interface IWithWeight
	{
		/// <summary>
		/// Вес.
		/// </summary>
		ulong Weight { get; }
	}

	/// <summary>
	/// Алгоритм построения дерева Хаффмана.
	/// </summary>
	public static class Huffman
	{
		/// <summary>
		/// Кодирование списка с помощью дерева Хаффмана.
		/// Узлам с наибольшим весом присваиваются наиболее короткие коды.
		/// </summary>
		/// <typeparam name="T">тип узлов</typeparam>
		/// <param name="objects">список узлов</param>
		/// <param name="left">символ для "левого" направления</param>
		/// <param name="right">символ для "правого" направления</param>
		/// <returns>сопоставление узлов списка с их кодами</returns>
		public static Dictionary<T, string> Encode<T>(IEnumerable<T> objects, char left, char right)
			where T : class, IWithWeight
		{
			// составление первоначального списка узлов
			var allNodes = new Dictionary<T, TreeNode<T>>();
			foreach (var instance in objects)
				allNodes[instance] = new TreeNode<T>(instance);

			// построение дерева
			var treeNodes = new List<TreeNode<T>>(allNodes.Values);
			if (treeNodes.Count == 0)
				throw new Exception("Список узлов пуст!");
			do
			{
				var hasNotParent = treeNodes.Where(n => n.Parent == null).OrderBy(n => n.Weight).ToList();
				treeNodes.Add(new TreeNode<T>(hasNotParent[0], hasNotParent[1]));
			} while (treeNodes.Count(n => n.Parent == null) > 1);

			// кодирование
			treeNodes.Single(n => n.Parent == null).Encode(string.Empty, left, right);
			var result = new Dictionary<T, string>();
			foreach (var instance in allNodes.Keys)
				result[instance] = allNodes[instance].Code;
			return result;
		}

		private class TreeNode<T> : IWithWeight
			where T : class, IWithWeight
		{
			private readonly T Value;
			public ulong Weight { get; private set; }
			public TreeNode<T> Parent { get; private set; }
			private readonly TreeNode<T> Left;
			private readonly TreeNode<T> Right;
			public string Code { get; private set; }

			public TreeNode(T value)
			{
				Value = value;
				Weight = Value.Weight;
			}

			public TreeNode(TreeNode<T> left, TreeNode<T> right)
			{
				Left = left;
				Right = right;
				Weight = Left.Weight + Right.Weight;
				Left.Parent = this;
				Right.Parent = this;
			}

			public void Encode(string code, char left, char right)
			{
				Code = code;
				if (Left != null && Right != null)
				{
					Left.Encode(code + left, left, right);
					Right.Encode(code + right, left, right);
				}
			}

			public override string ToString()
			{
				return Weight.ToString("F");
			}
		}
	}
}
