using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Modules.Boolean;
using Inventor.Semantics.Modules.Boolean.Concepts;
using Inventor.Semantics.Modules.Classification;
using Inventor.Semantics.Set;
using Inventor.Semantics.Statements;

namespace Inventor.Semantics.Test.Statements
{
	[TestFixture]
	public class ParentChildTest
	{
		[Test]
		public void CheckConstructorsIntegrity()
		{
			// arrange
			var root = new ParentChild<int>(0);

			var children = new[]
			{
				new ParentChild<int>(1),
				new ParentChild<int>(2),
				new ParentChild<int>(3),
			};

			// act
			var test = new ParentChild<int>(10, root, children);

			// assert
			Assert.IsNull(root.Parent);
			Assert.AreSame(test, root.Children.Single());

			Assert.AreSame(root, test.Parent);
			Assert.IsTrue(test.Children.SequenceEqual(children));

			Assert.IsTrue(children.All(child => child.Parent == test && child.Children.Count == 0));
		}

		[Test]
		public void GetParentsWithoutRelationshipsReturnsEmptyList()
		{
			foreach (string node in All)
			{
				Assert.AreEqual(0, Array.Empty<TestParentChild>().GetParentsOneLevel(node).Count);
				Assert.AreEqual(0, Array.Empty<TestParentChild>().GetParentsAllLevels(node).Count);
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
				Assert.AreEqual(0, Array.Empty<TestParentChild>().GetChildrenOneLevel(node).Count);
				Assert.AreEqual(0, Array.Empty<TestParentChild>().GetChildrenAllLevels(node).Count);
			}

			var allRelationships = createTestSet();
			Assert.AreEqual(0, allRelationships.GetChildrenOneLevel("Absent").Count);
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

			var relationships = allRelationships.GetChildrenOneLevel(Parent1);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single1));

			relationships = allRelationships.GetChildrenOneLevel(Parent2);
			Assert.AreEqual(3, relationships.Count);
			Assert.IsTrue(relationships.Contains(Medium1));
			Assert.IsTrue(relationships.Contains(Medium2));
			Assert.IsTrue(relationships.Contains(Single2));

			relationships = allRelationships.GetChildrenOneLevel(Child1);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenOneLevel(Child2);
			Assert.AreEqual(0, relationships.Count);

			relationships = allRelationships.GetChildrenOneLevel(Medium1);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenOneLevel(Medium2);
			Assert.AreEqual(2, relationships.Count);
			Assert.IsTrue(relationships.Contains(Child1));
			Assert.IsTrue(relationships.Contains(Child2));

			relationships = allRelationships.GetChildrenOneLevel(Single1);
			Assert.AreEqual(Child1, relationships.Single());

			relationships = allRelationships.GetChildrenOneLevel(Single2);
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
			var parent1 = ConceptCreationHelper.CreateConcept();
			var topMedium1 = ConceptCreationHelper.CreateConcept();
			var bottomMedium1 = ConceptCreationHelper.CreateConcept();
			var child1 = ConceptCreationHelper.CreateConcept();
			var parent2 = ConceptCreationHelper.CreateConcept();
			var topMedium2 = ConceptCreationHelper.CreateConcept();
			var bottomMedium2 = ConceptCreationHelper.CreateConcept();
			var child2 = ConceptCreationHelper.CreateConcept();
			var parentNoConnection = ConceptCreationHelper.CreateConcept();
			var childNoConnection = ConceptCreationHelper.CreateConcept();
			var parent2Path = ConceptCreationHelper.CreateConcept();
			var medium2Path1 = ConceptCreationHelper.CreateConcept();
			var medium2Path2 = ConceptCreationHelper.CreateConcept();
			var child2Path = ConceptCreationHelper.CreateConcept();

			var statements = new IStatement[]
			{
				new TestPath(parent1, topMedium1),
				new TestPath(topMedium1, bottomMedium1),
				new TestPath(bottomMedium1, child1),
				new TestPath(parent2, topMedium2),
				new TestPath(topMedium2, bottomMedium2),
				new TestPath(bottomMedium2, child2),
				new IsStatement(null, parentNoConnection, childNoConnection),
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
			var a = ConceptCreationHelper.CreateConcept();
			var b = ConceptCreationHelper.CreateConcept();
			var c = ConceptCreationHelper.CreateConcept();

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

		[Test]
		public void CheckGetChildrenTree()
		{
			// arrange
			var concept1 =    1.CreateConcept();
			var concept2 =    2.CreateConcept();
			var concept3 =    3.CreateConcept();
			var concept11 =   11.CreateConcept();
			var concept12 =   12.CreateConcept();
			var concept13 =   13.CreateConcept();
			var concept111 =  111.CreateConcept();
			var concept112 =  112.CreateConcept();
			var concept113 =  113.CreateConcept();
			var concept121 =  121.CreateConcept();
			var concept122 =  122.CreateConcept();
			var concept123 =  123.CreateConcept();
			var concept131 =  131.CreateConcept();
			var concept132 =  132.CreateConcept();
			var concept133 =  133.CreateConcept();
			var concept1111 = 1111.CreateConcept();
			var concept1112 = 1112.CreateConcept();
			var concept1113 = 1113.CreateConcept();
			var concept1121 = 1121.CreateConcept();
			var concept1122 = 1122.CreateConcept();
			var concept1123 = 1123.CreateConcept();
			var concept1131 = 1131.CreateConcept();
			var concept1132 = 1132.CreateConcept();
			var concept1133 = 1133.CreateConcept();

			var statements = new List<IStatement>
			{
				new IsStatement(null, concept1, concept11),
				new IsStatement(null, concept1, concept12),
				new IsStatement(null, concept1, concept13),

				new IsStatement(null, concept11, concept111),
				new IsStatement(null, concept11, concept112),
				new IsStatement(null, concept11, concept113),

				new IsStatement(null, concept12, concept121),
				new IsStatement(null, concept12, concept122),
				new IsStatement(null, concept12, concept123),

				new IsStatement(null, concept13, concept131),
				new IsStatement(null, concept13, concept132),
				new IsStatement(null, concept13, concept133),

				new IsStatement(null, concept111, concept1111),
				new IsStatement(null, concept111, concept1112),
				new IsStatement(null, concept111, concept1113),

				new IsStatement(null, concept112, concept1121),
				new IsStatement(null, concept112, concept1122),
				new IsStatement(null, concept112, concept1123),

				new IsStatement(null, concept113, concept1131),
				new IsStatement(null, concept113, concept1132),
				new IsStatement(null, concept113, concept1133),
			};

			// act
			var involvedStatements = new List<IsStatement>();
			var resultTree = statements.GetChildrenTree(concept11, involvedStatements);

			// assert
			Assert.IsNotNull(resultTree);
			Assert.AreSame(concept11, resultTree.Value);
			Assert.AreEqual(3, resultTree.Children.Count);

			var result111 = resultTree.Children.Single(node => node.Value == concept111);
			Assert.AreSame(concept111, result111.Value);
			var result112 = resultTree.Children.Single(node => node.Value == concept112);
			Assert.AreSame(concept112, result112.Value);
			var result113 = resultTree.Children.Single(node => node.Value == concept113);
			Assert.AreSame(concept113, result113.Value);

			var result1111 = result111.Children.Single(node => node.Value == concept1111);
			Assert.AreSame(concept1111, result1111.Value);
			var result1112 = result111.Children.Single(node => node.Value == concept1112);
			Assert.AreSame(concept1112, result1112.Value);
			var result1113 = result111.Children.Single(node => node.Value == concept1113);
			Assert.AreSame(concept1113, result1113.Value);

			var result1121 = result112.Children.Single(node => node.Value == concept1121);
			Assert.AreSame(concept1121, result1121.Value);
			var result1122 = result112.Children.Single(node => node.Value == concept1122);
			Assert.AreSame(concept1122, result1122.Value);
			var result1123 = result112.Children.Single(node => node.Value == concept1123);
			Assert.AreSame(concept1123, result1123.Value);

			var result1131 = result113.Children.Single(node => node.Value == concept1131);
			Assert.AreSame(concept1131, result1131.Value);
			var result1132 = result113.Children.Single(node => node.Value == concept1132);
			Assert.AreSame(concept1132, result1132.Value);
			var result1133 = result113.Children.Single(node => node.Value == concept1133);
			Assert.AreSame(concept1133, result1133.Value);

			Assert.AreEqual(statements.Count - 3 - 6, involvedStatements.Count);
		}

		[Test]
		public void CheckGetChildrenOneLevelStatements()
		{
			// arrange
			var semanticNetwork = createBearsSample();
			var animalsConcept = semanticNetwork.Concepts["Kingdom: Animalia"];

			// act
			var animals = semanticNetwork.Statements.GetChildrenOneLevel<IConcept, IsStatement>(animalsConcept).Select(concept => concept.ID).ToList();
			var bears = semanticNetwork.Statements.GetChildrenOneLevel<IConcept, IsStatement>(semanticNetwork.Concepts["Genus: Ursus"]).Select(concept => concept.ID).ToList();

			// assert
			Assert.AreEqual("Phylum: Chordata", animals.Single());

			Assert.AreEqual(4, bears.Count);
			Assert.IsTrue(bears.Contains("Ursus americanus"));
			Assert.IsTrue(bears.Contains("Ursus arctos"));
			Assert.IsTrue(bears.Contains("Ursus maritimus"));
			Assert.IsTrue(bears.Contains("Ursus thibetanuss"));
		}

		[Test]
		public void CheckGetChildrenAllLevelsStatements()
		{
			// arrange
			var semanticNetwork = createBearsSample();
			var animalsConcept = semanticNetwork.Concepts["Kingdom: Animalia"];
			var ignoredConcepts = new[] { LogicalValues.True, LogicalValues.False, animalsConcept };

			// act
			var animals = semanticNetwork.Statements.GetChildrenAllLevels<IConcept, IsStatement>(semanticNetwork.Concepts["Kingdom: Animalia"]).Select(concept => concept.ID).ToList();
			var bears = semanticNetwork.Statements.GetChildrenAllLevels<IConcept, IsStatement>(semanticNetwork.Concepts["Genus: Ursus"]).Select(concept => concept.ID).ToList();

			// assert
			Assert.AreEqual(semanticNetwork.Concepts.Count - ignoredConcepts.Length, animals.Count);
			foreach (var animal in semanticNetwork.Concepts.Except(ignoredConcepts))
			{
				Assert.IsTrue(animals.Contains(animal.ID));
			}

			Assert.AreEqual(4, bears.Count);
			Assert.IsTrue(bears.Contains("Ursus americanus"));
			Assert.IsTrue(bears.Contains("Ursus arctos"));
			Assert.IsTrue(bears.Contains("Ursus maritimus"));
			Assert.IsTrue(bears.Contains("Ursus thibetanuss"));
		}

		private static ISemanticNetwork createBearsSample()
		{
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
				new SetModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			ISemanticNetwork semanticNetwork = new SemanticNetwork(Language.Default).WithModules(modules);

			IConcept animal = "Kingdom: Animalia".CreateConcept("Animal");
			IConcept chordate = "Phylum: Chordata".CreateConcept("Chordate");
			IConcept mammal = "Class: Mammalia".CreateConcept("Mammal");
			IConcept carnivor = "Order: Carnivora".CreateConcept("Carnivor");
			// Just skip Ursidae, Ursinae and Ursini for short.
			IConcept bear = "Genus: Ursus".CreateConcept("Bear");
			IConcept americanBlackBear = "Ursus americanus".CreateConcept("American black bear");
			IConcept brownBear = "Ursus arctos".CreateConcept("Brown bear");
			IConcept polarBear = "Ursus maritimus".CreateConcept("Polar bear");
			IConcept asianBlackBear = "Ursus thibetanuss".CreateConcept("Asian black bear");

			semanticNetwork.Concepts.Add(animal);
			semanticNetwork.Concepts.Add(chordate);
			semanticNetwork.Concepts.Add(mammal);
			semanticNetwork.Concepts.Add(carnivor);
			semanticNetwork.Concepts.Add(bear);
			semanticNetwork.Concepts.Add(americanBlackBear);
			semanticNetwork.Concepts.Add(brownBear);
			semanticNetwork.Concepts.Add(polarBear);
			semanticNetwork.Concepts.Add(asianBlackBear);

			semanticNetwork.DeclareThat(chordate).IsDescendantOf(animal); // Or we can use DeclareThat(animal).IsAncestorOf(chordate) instead.
			semanticNetwork.DeclareThat(mammal).IsDescendantOf(chordate);
			semanticNetwork.DeclareThat(carnivor).IsDescendantOf(mammal);
			semanticNetwork.DeclareThat(bear).IsDescendantOf(carnivor);
			semanticNetwork.DeclareThat(americanBlackBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(brownBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(polarBear).IsDescendantOf(bear);
			semanticNetwork.DeclareThat(asianBlackBear).IsDescendantOf(bear);

			return semanticNetwork;
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

			public string ID
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

			public IText DescribeTrue()
			{
				throw new NotSupportedException();
			}

			public IText DescribeFalse()
			{
				throw new NotSupportedException();
			}

			public IText DescribeQuestion()
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
