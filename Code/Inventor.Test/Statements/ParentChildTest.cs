using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core.Statements;

namespace Inventor.Test.Statements
{
	public class ParentChildTest
	{
		[Test]
		public void GetParentsWithoutRelationshipsReturnsEmptyList()
		{
			foreach (string node in All)
			{
				Assert.AreEqual(0, new TestParentChild[0].GetParentsPlainList(node).Count);
				Assert.AreEqual(0, new TestParentChild[0].GetParentsTree(node).Count);
			}

			var allRelationships = createTestSet();
			Assert.AreEqual(0, allRelationships.GetParentsPlainList("Absent").Count);
			Assert.AreEqual(0, allRelationships.GetParentsTree("Absent").Count);
		}

		[Test]
		public void GetChildrenWithoutRelationshipsReturnsEmptyList()
		{
			foreach (string node in All)
			{
				Assert.AreEqual(0, new TestParentChild[0].GetChildrenPlainList(node).Count);
				Assert.AreEqual(0, new TestParentChild[0].GetChildrenTree(node).Count);
			}

			var allRelationships = createTestSet();
			Assert.AreEqual(0, allRelationships.GetChildrenPlainList("Absent").Count);
			Assert.AreEqual(0, allRelationships.GetChildrenTree("Absent").Count);
		}

		[Test]
		public void GetPlainParentsReturnsOneLevel()
		{
			var allRelationships = createTestSet();

			var relationships = allRelationships.GetParentsPlainList(Parent1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetParentsPlainList(Parent2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetParentsPlainList(Child1);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));

			relationships = allRelationships.GetParentsPlainList(Child2);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));

			relationships = allRelationships.GetParentsPlainList(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));

			relationships = allRelationships.GetParentsPlainList(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));

			relationships = allRelationships.GetParentsPlainList(Single1);
			Assert.AreEqual(Parent1, relationships.Single());

			relationships = allRelationships.GetParentsPlainList(Single2);
			Assert.AreEqual(Parent2, relationships.Single());
		}

		[Test]
		public void GetPlainChildrenReturnsOneLevel()
		{
			var allRelationships = createTestSet();

			var relationships = allRelationships.GetChildrenPlainList(Parent1);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));

			relationships = allRelationships.GetChildrenPlainList(Parent2);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));

			relationships = allRelationships.GetChildrenPlainList(Child1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenPlainList(Child2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenPlainList(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenPlainList(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenPlainList(Single1);
			Assert.AreEqual(Child1, relationships.Single());

			relationships = allRelationships.GetChildrenPlainList(Single2);
			Assert.AreEqual(Child2, relationships.Single());
		}

		[Test]
		public void GetParentsTreeReturnsAllLevels()
		{
			var allRelationships = createTestSet();

			var relationships = allRelationships.GetParentsTree(Parent1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetParentsTree(Parent2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetParentsTree(Child1);
			Assert.AreEqual(5, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));

			relationships = allRelationships.GetParentsTree(Child2);
			Assert.AreEqual(5, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));

			relationships = allRelationships.GetParentsTree(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));

			relationships = allRelationships.GetParentsTree(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));

			relationships = allRelationships.GetParentsTree(Single1);
			Assert.AreEqual(Parent1, relationships.Single());

			relationships = allRelationships.GetParentsTree(Single2);
			Assert.AreEqual(Parent2, relationships.Single());
		}

		[Test]
		public void GetChildrenTreeReturnsAllLevels()
		{
			var allRelationships = createTestSet();

			var relationships = allRelationships.GetChildrenTree(Parent1);
			Assert.AreEqual(5, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenTree(Parent2);
			Assert.AreEqual(5, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenTree(Child1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenTree(Child2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenTree(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenTree(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenTree(Single1);
			Assert.AreEqual(Child1, relationships.Single());

			relationships = allRelationships.GetChildrenTree(Single2);
			Assert.AreEqual(Child2, relationships.Single());
		}

		private const string Parent1 = "Parent 1";
		private const string Parent2 = "Parent 2";
		private const string Child1 = "Child 1";
		private const string Child2 = "Child 2";
		private const string Medium1 = "Medium 1";
		private const string Medium2 = "Medium 2";
		private const string Single1 = "Single 1";
		private const string Single2 = "Single 2";
		private static readonly ICollection<string> All = new[]
		{
			Parent1,
			Parent2,
			Child1,
			Child2,
			Medium1,
			Medium2,
			Single1,
			Single2,
		};

		private static List<TestParentChild> createTestSet()
		{
			return new List<TestParentChild>
			{
				new TestParentChild(Parent1, Medium1),
				new TestParentChild(Parent1, Medium2),
				new TestParentChild(Parent1, Single1),
				new TestParentChild(Parent2, Medium1),
				new TestParentChild(Parent2, Medium2),
				new TestParentChild(Parent2, Single2),
				new TestParentChild(Medium1, Child1),
				new TestParentChild(Medium1, Child2),
				new TestParentChild(Medium2, Child1),
				new TestParentChild(Medium2, Child2),
				new TestParentChild(Single1, Child1),
				new TestParentChild(Single2, Child2),
			};
		}

		private class TestParentChild : IParentChild<string>
		{
			public string Parent { get; }
			public string Child { get; }

			public TestParentChild(string parent, string child)
			{
				Parent = parent;
				Child = child;
			}
		}
	}
}
