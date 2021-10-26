using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Algorithms
{
	public interface IWithWeight
	{
		ulong Weight { get; }
	}

	public static class Huffman
	{
		public static Dictionary<T, string> HuffmanEncode<T>(this ICollection<T> objects, char left, char right)
			where T : class, IWithWeight
		{
			if (objects == null) throw new ArgumentNullException(nameof(objects));
			if (objects.Count == 0) return new Dictionary<T, string>();

			var allNodes = objects.ToDictionary(
				instance => instance,
				instance => new TreeNode<T>(instance));

			var treeNodes = new List<TreeNode<T>>(allNodes.Values);
			do
			{
				var hasNotParent = treeNodes.Where(n => n.Parent == null).OrderBy(n => n.Weight).ToList();
				treeNodes.Add(new TreeNode<T>(hasNotParent[0], hasNotParent[1]));
			} while (treeNodes.Count(n => n.Parent == null) > 1);

			treeNodes.Single(n => n.Parent == null).Encode(string.Empty, left, right);
			var result = new Dictionary<T, string>();
			foreach (var instance in allNodes.Keys)
			{
				result[instance] = allNodes[instance].Code;
			}
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
