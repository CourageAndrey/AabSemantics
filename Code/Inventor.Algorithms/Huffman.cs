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
		public static Dictionary<ItemT, string> HuffmanEncode<ItemT>(this ICollection<ItemT> items, Func<string, string> appendLeft, Func<string, string> appendRight)
			where ItemT : class, IWithWeight
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			if (items.Count == 0) return new Dictionary<ItemT, string>();

			var allNodes = items.ToDictionary(
				instance => instance,
				instance => new TreeNode<ItemT>(instance));

			var treeNodes = new List<TreeNode<ItemT>>(allNodes.Values);
			do
			{
				var hasNotParent = treeNodes.Where(n => n.Parent == null).OrderBy(n => n.Weight).ToList();
				treeNodes.Add(new TreeNode<ItemT>(hasNotParent[0], hasNotParent[1]));
			} while (treeNodes.Count(n => n.Parent == null) > 1);

			treeNodes.Single(n => n.Parent == null).Encode(string.Empty, appendLeft, appendRight);

			return allNodes.ToDictionary(
				instance => instance.Key,
				instance => instance.Value.Code);
		}

		private class TreeNode<ItemT> : IWithWeight
			where ItemT : class, IWithWeight
		{
			private readonly ItemT Item;
			public ulong Weight { get; private set; }
			public TreeNode<ItemT> Parent { get; private set; }
			private readonly TreeNode<ItemT> Left;
			private readonly TreeNode<ItemT> Right;
			public string Code { get; private set; }

			public TreeNode(ItemT item)
			{
				Item = item;
				Weight = Item.Weight;
			}

			public TreeNode(TreeNode<ItemT> left, TreeNode<ItemT> right)
			{
				Left = left;
				Right = right;
				Weight = Left.Weight + Right.Weight;
				Left.Parent = this;
				Right.Parent = this;
			}

			public void Encode(string code, Func<string, string> appendLeft, Func<string, string> appendRight)
			{
				Code = code;
				if (Left != null && Right != null)
				{
					Left.Encode(appendLeft(code), appendLeft, appendRight);
					Right.Encode(appendRight(code), appendLeft, appendRight);
				}
			}

			public override string ToString()
			{
				return Weight.ToString("F");
			}
		}
	}
}
