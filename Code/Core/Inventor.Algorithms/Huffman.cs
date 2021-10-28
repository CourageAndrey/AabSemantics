using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Algorithms
{
	public static class Huffman
	{
		public static Dictionary<ItemT, TCode> HuffmanEncode<ItemT, TCode>(
			this ICollection<ItemT> items,
			Func<ItemT, UInt64> getWeight,
			TCode emptyCode,
			Func<TCode, TCode> appendLeft,
			Func<TCode, TCode> appendRight)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			if (getWeight == null) throw new ArgumentNullException(nameof(getWeight));
			if (appendLeft == null) throw new ArgumentNullException(nameof(appendLeft));
			if (appendRight == null) throw new ArgumentNullException(nameof(appendRight));

			if (items.Count == 0)
			{
				return new Dictionary<ItemT, TCode>();
			}
			else if (items.Count == 1)
			{
				return new Dictionary<ItemT, TCode>
				{
					{ items.First(), appendLeft(emptyCode) },
				};
			}
			else if (items.Count == 2)
			{
				return new Dictionary<ItemT, TCode>
				{
					{ items.First(), appendLeft(emptyCode) },
					{ items.Last(), appendRight(emptyCode) },
				};
			}
			else
			{
				var allNodes = items.ToDictionary(
					instance => instance,
					instance => new TreeNode<ItemT, TCode>(instance, getWeight));

				var treeNodes = new List<TreeNode<ItemT, TCode>>(allNodes.Values);
				do
				{
					var hasNotParent = treeNodes.Where(n => n.Parent == null).OrderBy(n => n.Weight).Take(2).ToList();
					treeNodes.Add(new TreeNode<ItemT, TCode>(hasNotParent[0], hasNotParent[1]));
				} while (treeNodes.Count(n => n.Parent == null) > 1);

				treeNodes.Single(n => n.Parent == null).Encode(emptyCode, appendLeft, appendRight);

				return allNodes.ToDictionary(
					instance => instance.Key,
					instance => instance.Value.Code);
			}
		}

		private class TreeNode<ItemT, TCode>
		{
			private readonly ItemT Item;
			public UInt64 Weight { get; }
			public TreeNode<ItemT, TCode> Parent { get; private set; }
			private readonly TreeNode<ItemT, TCode> Left;
			private readonly TreeNode<ItemT, TCode> Right;
			public TCode Code { get; private set; }

			public TreeNode(ItemT item, Func<ItemT, UInt64> getWeight)
			{
				Item = item;
				Weight = getWeight(item);
			}

			public TreeNode(TreeNode<ItemT, TCode> left, TreeNode<ItemT, TCode> right)
			{
				Left = left;
				Right = right;
				Weight = Left.Weight + Right.Weight;
				Left.Parent = this;
				Right.Parent = this;
			}

			public void Encode(TCode code, Func<TCode, TCode> appendLeft, Func<TCode, TCode> appendRight)
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
