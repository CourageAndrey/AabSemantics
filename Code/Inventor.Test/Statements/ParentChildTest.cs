using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Base;
using Inventor.Core.Statements;

namespace Inventor.Test.Statements
{
	[TestFixture]
	public class ParentChildTest
	{
		[Test]
		public void GetParentsWithoutRelationshipsReturnsEmptyList()
		{
			foreach (string node in All)
			{
				Assert.AreEqual(0, new TestParentChild[0].GetParentsOneLevel(node).Count);
				Assert.AreEqual(0, new TestParentChild[0].GetParentsAllLevels(node).Count);
			}

			var allRelationships = createTestSet();
			Assert.AreEqual(0, allRelationships.GetParentsOneLevel("Absent").Count);
			Assert.AreEqual(0, allRelationships.GetParentsAllLevels("Absent").Count);
		}

		[Test]
		public void GetChildrenWithoutRelationshipsReturnsEmptyList()
		{
			foreach (string node in All)
			{
				Assert.AreEqual(0, new TestParentChild[0].GetChildrenOnLevel(node).Count);
				Assert.AreEqual(0, new TestParentChild[0].GetChildrenAllLevels(node).Count);
			}

			var allRelationships = createTestSet();
			Assert.AreEqual(0, allRelationships.GetChildrenOnLevel("Absent").Count);
			Assert.AreEqual(0, allRelationships.GetChildrenAllLevels("Absent").Count);
		}

		[Test]
		public void GetPlainParentsReturnsOneLevel()
		{
			var allRelationships = createTestSet();

			var relationships = allRelationships.GetParentsOneLevel(Parent1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetParentsOneLevel(Parent2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetParentsOneLevel(Child1);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));

			relationships = allRelationships.GetParentsOneLevel(Child2);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));

			relationships = allRelationships.GetParentsOneLevel(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));

			relationships = allRelationships.GetParentsOneLevel(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));

			relationships = allRelationships.GetParentsOneLevel(Single1);
			Assert.AreEqual(Parent1, relationships.Single());

			relationships = allRelationships.GetParentsOneLevel(Single2);
			Assert.AreEqual(Parent2, relationships.Single());
		}

		[Test]
		public void GetPlainChildrenReturnsOneLevel()
		{
			var allRelationships = createTestSet();

			var relationships = allRelationships.GetChildrenOnLevel(Parent1);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));

			relationships = allRelationships.GetChildrenOnLevel(Parent2);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));

			relationships = allRelationships.GetChildrenOnLevel(Child1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenOnLevel(Child2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenOnLevel(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenOnLevel(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenOnLevel(Single1);
			Assert.AreEqual(Child1, relationships.Single());

			relationships = allRelationships.GetChildrenOnLevel(Single2);
			Assert.AreEqual(Child2, relationships.Single());
		}

		[Test]
		public void GetParentsTreeReturnsAllLevels()
		{
			var allRelationships = createTestSet();

			var relationships = allRelationships.GetParentsAllLevels(Parent1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetParentsAllLevels(Parent2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetParentsAllLevels(Child1);
			Assert.AreEqual(5, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));

			relationships = allRelationships.GetParentsAllLevels(Child2);
			Assert.AreEqual(5, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));

			relationships = allRelationships.GetParentsAllLevels(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));

			relationships = allRelationships.GetParentsAllLevels(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Parent1));
			Assert.IsTrue(relationships.Contains(Parent2));

			relationships = allRelationships.GetParentsAllLevels(Single1);
			Assert.AreEqual(Parent1, relationships.Single());

			relationships = allRelationships.GetParentsAllLevels(Single2);
			Assert.AreEqual(Parent2, relationships.Single());
		}

		[Test]
		public void GetChildrenTreeReturnsAllLevels()
		{
			var allRelationships = createTestSet();

			var relationships = allRelationships.GetChildrenAllLevels(Parent1);
			Assert.AreEqual(5, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenAllLevels(Parent2);
			Assert.AreEqual(5, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenAllLevels(Child1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenAllLevels(Child2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenAllLevels(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenAllLevels(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenAllLevels(Single1);
			Assert.AreEqual(Child1, relationships.Single());

			relationships = allRelationships.GetChildrenAllLevels(Single2);
			Assert.AreEqual(Child2, relationships.Single());
		}

		[Test]
		public void AllMethodsSupportRecursion()
		{
			// 1 variable loop
			const string x = "x";
			var relationshipsLoop1 = new[] { new TestParentChild(x, x) };
			foreach (var list1 in new[]
			{
				relationshipsLoop1.GetChildrenAllLevels(x),
				relationshipsLoop1.GetParentsAllLevels(x),
			})
			{
				Assert.AreEqual(x, list1.Single());
			}

			// 2 variables loop
			const string y = "y";
			var relationshipsLoop2 = new[] { new TestParentChild(x, y), new TestParentChild(y, x) };
			foreach (var list2 in new[]
			{
				relationshipsLoop2.GetChildrenAllLevels(x),
				relationshipsLoop2.GetParentsAllLevels(x),
			})
			{
				Assert.AreEqual(2, list2.Count);
				Assert.AreEqual(y, list2[0]);
				Assert.AreEqual(x, list2[1]);
			}
			foreach (var list2 in new[]
			{
				relationshipsLoop2.GetChildrenAllLevels(y),
				relationshipsLoop2.GetParentsAllLevels(y),
			})
			{
				Assert.AreEqual(2, list2.Count);
				Assert.AreEqual(x, list2[0]);
				Assert.AreEqual(y, list2[1]);
			}

			// 3 variables loop
			const string z = "z";
			var relationshipsLoop3 = new[] { new TestParentChild(x, y), new TestParentChild(y, z), new TestParentChild(z, x) };
			var list3 = relationshipsLoop3.GetChildrenAllLevels(x);
			Assert.AreEqual(3, list3.Count);
			Assert.AreEqual(y, list3[0]);
			Assert.AreEqual(z, list3[1]);
			Assert.AreEqual(x, list3[2]);
			list3 = relationshipsLoop3.GetParentsAllLevels(x);
			Assert.AreEqual(3, list3.Count);
			Assert.AreEqual(z, list3[0]);
			Assert.AreEqual(y, list3[1]);
			Assert.AreEqual(x, list3[2]);
			list3 = relationshipsLoop3.GetChildrenAllLevels(y);
			Assert.AreEqual(3, list3.Count);
			Assert.AreEqual(z, list3[0]);
			Assert.AreEqual(x, list3[1]);
			Assert.AreEqual(y, list3[2]);
			list3 = relationshipsLoop3.GetParentsAllLevels(y);
			Assert.AreEqual(3, list3.Count);
			Assert.AreEqual(x, list3[0]);
			Assert.AreEqual(z, list3[1]);
			Assert.AreEqual(y, list3[2]);
			list3 = relationshipsLoop3.GetChildrenAllLevels(z);
			Assert.AreEqual(3, list3.Count);
			Assert.AreEqual(x, list3[0]);
			Assert.AreEqual(y, list3[1]);
			Assert.AreEqual(z, list3[2]);
			list3 = relationshipsLoop3.GetParentsAllLevels(z);
			Assert.AreEqual(3, list3.Count);
			Assert.AreEqual(y, list3[0]);
			Assert.AreEqual(x, list3[1]);
			Assert.AreEqual(z, list3[2]);
		}

		[Test]
		public void CheckExplanation()
		{
			var allRelationships = createTestSet();
			var explanation = new List<TestParentChild>();

			allRelationships.GetParentsAllLevels(Child1, explanation);
			Assert.AreEqual(8, explanation.Count);
			Assert.IsTrue(explanation.Any(relationship => relationship.Parent == Medium1 && relationship.Child == Child1));
			Assert.IsTrue(explanation.Any(relationship => relationship.Parent == Medium2 && relationship.Child == Child1));
			Assert.IsTrue(explanation.Any(relationship => relationship.Parent == Single1 && relationship.Child == Child1));
			Assert.IsTrue(explanation.Any(relationship => relationship.Parent == Parent1 && relationship.Child == Medium1));
			Assert.IsTrue(explanation.Any(relationship => relationship.Parent == Parent2 && relationship.Child == Medium1));
			Assert.IsTrue(explanation.Any(relationship => relationship.Parent == Parent1 && relationship.Child == Medium2));
			Assert.IsTrue(explanation.Any(relationship => relationship.Parent == Parent2 && relationship.Child == Medium2));
			Assert.IsTrue(explanation.Any(relationship => relationship.Parent == Parent1 && relationship.Child == Single1));
		}

		[Test]
		public void CheckPathFinding()
		{
			var parent1 = TestHelper.CreateConcept();
			var topMedium1 = TestHelper.CreateConcept();
			var bottomMedium1 = TestHelper.CreateConcept();
			var child1 = TestHelper.CreateConcept();
			var parent2 = TestHelper.CreateConcept();
			var topMedium2 = TestHelper.CreateConcept();
			var bottomMedium2 = TestHelper.CreateConcept();
			var child2 = TestHelper.CreateConcept();
			var parentNoConnection = TestHelper.CreateConcept();
			var childNoConnection = TestHelper.CreateConcept();
			var parent2Path = TestHelper.CreateConcept();
			var medium2Path1 = TestHelper.CreateConcept();
			var medium2Path2 = TestHelper.CreateConcept();
			var child2Path = TestHelper.CreateConcept();

			var statements = new IStatement[]
			{
				new TestPath(parent1, topMedium1),
				new TestPath(topMedium1, bottomMedium1),
				new TestPath(bottomMedium1, child1),
				new TestPath(parent2, topMedium2),
				new TestPath(topMedium2, bottomMedium2),
				new TestPath(bottomMedium2, child2),
				new IsStatement(parentNoConnection, childNoConnection),
				new TestPath(parent2Path, medium2Path1),
				new TestPath(parent2Path, medium2Path2),
				new TestPath(medium2Path1, child2Path),
				new TestPath(medium2Path2, child2Path),
			};

			// valid direct paths
			Assert.IsTrue(statements.FindPath<IConcept>(typeof(TestPath), parent1, child1).Any());
			Assert.IsTrue(statements.FindPath<IConcept>(typeof(TestPath), parent2, child2).Any());
			// invalid direct paths
			Assert.IsFalse(statements.FindPath<IConcept>(typeof(TestPath), parent1, child2).Any());
			Assert.IsFalse(statements.FindPath<IConcept>(typeof(TestPath), parent2, child1).Any());
			// invalid reverted paths
			Assert.IsFalse(statements.FindPath<IConcept>(typeof(TestPath), child1, parent1).Any());
			Assert.IsFalse(statements.FindPath<IConcept>(typeof(TestPath), child2, parent2).Any());
			Assert.IsFalse(statements.FindPath<IConcept>(typeof(TestPath), child1, parent2).Any());
			Assert.IsFalse(statements.FindPath<IConcept>(typeof(TestPath), child2, parent1).Any());
			// other type path
			Assert.IsFalse(statements.FindPath<IConcept>(typeof(TestPath), parentNoConnection, childNoConnection).Any());
			Assert.IsTrue(statements.FindPath<IConcept>(typeof(IsStatement), parentNoConnection, childNoConnection).Any());
			// 2 paths
			Assert.IsTrue(statements.FindPath<IConcept>(typeof(TestPath), parent2Path, child2Path).Any());
			Assert.IsFalse(statements.FindPath<IConcept>(typeof(TestPath), child2Path, parent2Path).Any());
		}

		[Test]
		public void CheckRecursivePaths()
		{
			var a = TestHelper.CreateConcept();
			var b = TestHelper.CreateConcept();
			var c = TestHelper.CreateConcept();

			var statementsValid1 = new IStatement[]
			{
				new TestPath(a, a),
			};
			var statementsValid2 = new IStatement[]
			{
				new TestPath(a, b),
				new TestPath(b, a),
			};
			var statementsValid3 = new IStatement[]
			{
				new TestPath(a, b),
				new TestPath(b, c),
				new TestPath(c, a),
			};
			var statementsInvalid = new IStatement[]
			{
				new TestPath(a, b),
				new TestPath(b, b),
			};
			var statementsValid4 = new IStatement[]
			{
				new TestPath(a, b),
				new TestPath(b, c),
				new TestPath(c, a),
			};

			foreach (var statements in new[] { statementsValid1, statementsValid2, statementsValid3, statementsValid4 })
			{
				Assert.IsTrue(statements.FindPath<IConcept>(typeof(TestPath), a, a).Any());
			}

			Assert.IsFalse(statementsInvalid.FindPath<IConcept>(typeof(TestPath), a, a).Any());
		}

		private const string Parent1 = "Parent 1";
		private const string Parent2 = "Parent 2";
		private const string Child1 = "Descendant 1";
		private const string Child2 = "Descendant 2";
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

		private class TestPath : IParentChild<IConcept>, IStatement
		{
			public IConcept Parent { get; }
			public IConcept Child { get; }

			public TestPath(IConcept parent, IConcept child)
			{
				Parent = parent;
				Child = child;
			}

			public ILocalizedString Name
			{ get { throw new NotSupportedException(); } }

			public IContext Context
			{
				get { throw new NotSupportedException(); }
				set { throw new NotSupportedException(); }
			}

			public ILocalizedString Hint
			{ get { throw new NotSupportedException(); } }

			public IEnumerable<IConcept> GetChildConcepts()
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeTrue(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeFalse(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeQuestion(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public bool CheckUnique(IEnumerable<IStatement> statements)
			{
				throw new NotSupportedException();
			}
		}
	}
}
