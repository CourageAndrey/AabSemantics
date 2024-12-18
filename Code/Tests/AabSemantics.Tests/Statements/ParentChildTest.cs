using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class ParentChildTest
	{
		[Test]
		public void GivenParentChild_WhenCreate_ThenIntegritySucceeds()
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
			Assert.That(root.Parent, Is.Null);
			Assert.That(root.Children.Single(), Is.SameAs(test));

			Assert.That(test.Parent, Is.SameAs(root));
			Assert.That(test.Children.SequenceEqual(children), Is.True);

			Assert.That(children.All(child => child.Parent == test && child.Children.Count == 0), Is.True);
		}

		[Test]
		public void GivenNoStatements_WhenGetParentsOneOrAllLevels_ThenReturnEmptyList()
		{
			foreach (string node in All)
			{
				Assert.That(Array.Empty<TestParentChild>().GetParentsOneLevel(node).Count, Is.EqualTo(0));
				Assert.That(Array.Empty<TestParentChild>().GetParentsAllLevels(node).Count, Is.EqualTo(0));
			}

			var allRelationships = CreateTestSet();
			Assert.That(allRelationships.GetParentsOneLevel("Absent").Count, Is.EqualTo(0));
			Assert.That(allRelationships.GetParentsAllLevels("Absent").Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenNoStatements_WhenGetChildrenOneOrAllLevels_ThenReturnEmptyList()
		{
			foreach (string node in All)
			{
				Assert.That(Array.Empty<TestParentChild>().GetChildrenOneLevel(node).Count, Is.EqualTo(0));
				Assert.That(Array.Empty<TestParentChild>().GetChildrenAllLevels(node).Count, Is.EqualTo(0));
			}

			var allRelationships = CreateTestSet();
			Assert.That(allRelationships.GetChildrenOneLevel("Absent").Count, Is.EqualTo(0));
			Assert.That(allRelationships.GetChildrenAllLevels("Absent").Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenTestSet_WhenGetParentsOneLevel_ThenReturnAllFound()
		{
			var allRelationships = CreateTestSet();

			var relationships = allRelationships.GetParentsOneLevel(Parent1);
			Assert.That(relationships.Count, Is.EqualTo(0));

			relationships = allRelationships.GetParentsOneLevel(Parent2);
			Assert.That(relationships.Count, Is.EqualTo(0));

			relationships = allRelationships.GetParentsOneLevel(Child1);
			Assert.That(relationships.Count, Is.EqualTo(3));
			Assert.That(relationships.Contains(Medium1), Is.True);
			Assert.That(relationships.Contains(Medium2), Is.True);
			Assert.That(relationships.Contains(Single1), Is.True);

			relationships = allRelationships.GetParentsOneLevel(Child2);
			Assert.That(relationships.Count, Is.EqualTo(3));
			Assert.That(relationships.Contains(Medium1), Is.True);
			Assert.That(relationships.Contains(Medium2), Is.True);
			Assert.That(relationships.Contains(Single2), Is.True);

			relationships = allRelationships.GetParentsOneLevel(Medium1);
			Assert.That(relationships.Count, Is.EqualTo(2));
			Assert.That(relationships.Contains(Parent1), Is.True);
			Assert.That(relationships.Contains(Parent2), Is.True);

			relationships = allRelationships.GetParentsOneLevel(Medium2);
			Assert.That(relationships.Count, Is.EqualTo(2));
			Assert.That(relationships.Contains(Parent1), Is.True);
			Assert.That(relationships.Contains(Parent2), Is.True);

			relationships = allRelationships.GetParentsOneLevel(Single1);
			Assert.That(relationships.Single(), Is.EqualTo(Parent1));

			relationships = allRelationships.GetParentsOneLevel(Single2);
			Assert.That(relationships.Single(), Is.EqualTo(Parent2));
		}

		[Test]
		public void GivenTestSet_WhenGetChildrenOneLevel_ThenReturnAllFound()
		{
			var allRelationships = CreateTestSet();

			var relationships = allRelationships.GetChildrenOneLevel(Parent1);
			Assert.That(relationships.Count, Is.EqualTo(3));
			Assert.That(relationships.Contains(Medium1), Is.True);
			Assert.That(relationships.Contains(Medium2), Is.True);
			Assert.That(relationships.Contains(Single1), Is.True);

			relationships = allRelationships.GetChildrenOneLevel(Parent2);
			Assert.That(relationships.Count, Is.EqualTo(3));
			Assert.That(relationships.Contains(Medium1), Is.True);
			Assert.That(relationships.Contains(Medium2), Is.True);
			Assert.That(relationships.Contains(Single2), Is.True);

			relationships = allRelationships.GetChildrenOneLevel(Child1);
			Assert.That(relationships.Count, Is.EqualTo(0));

			relationships = allRelationships.GetChildrenOneLevel(Child2);
			Assert.That(relationships.Count, Is.EqualTo(0));

			relationships = allRelationships.GetChildrenOneLevel(Medium1);
			Assert.That(relationships.Count, Is.EqualTo(2));
			Assert.That(relationships.Contains(Child1), Is.True);
			Assert.That(relationships.Contains(Child2), Is.True);

			relationships = allRelationships.GetChildrenOneLevel(Medium2);
			Assert.That(relationships.Count, Is.EqualTo(2));
			Assert.That(relationships.Contains(Child1), Is.True);
			Assert.That(relationships.Contains(Child2), Is.True);

			relationships = allRelationships.GetChildrenOneLevel(Single1);
			Assert.That(relationships.Single(), Is.EqualTo(Child1));

			relationships = allRelationships.GetChildrenOneLevel(Single2);
			Assert.That(relationships.Single(), Is.EqualTo(Child2));
		}

		[Test]
		public void GivenTestSet_WhenGetParentsAllLevels_ThenReturnAllFound()
		{
			var allRelationships = CreateTestSet();

			var relationships = allRelationships.GetParentsAllLevels(Parent1);
			Assert.That(relationships.Count, Is.EqualTo(0));

			relationships = allRelationships.GetParentsAllLevels(Parent2);
			Assert.That(relationships.Count, Is.EqualTo(0));

			relationships = allRelationships.GetParentsAllLevels(Child1);
			Assert.That(relationships.Count, Is.EqualTo(5));
			Assert.That(relationships.Contains(Parent1), Is.True);
			Assert.That(relationships.Contains(Parent2), Is.True);
			Assert.That(relationships.Contains(Medium1), Is.True);
			Assert.That(relationships.Contains(Medium2), Is.True);
			Assert.That(relationships.Contains(Single1), Is.True);

			relationships = allRelationships.GetParentsAllLevels(Child2);
			Assert.That(relationships.Count, Is.EqualTo(5));
			Assert.That(relationships.Contains(Parent1), Is.True);
			Assert.That(relationships.Contains(Parent2), Is.True);
			Assert.That(relationships.Contains(Medium1), Is.True);
			Assert.That(relationships.Contains(Medium2), Is.True);
			Assert.That(relationships.Contains(Single2), Is.True);

			relationships = allRelationships.GetParentsAllLevels(Medium1);
			Assert.That(relationships.Count, Is.EqualTo(2));
			Assert.That(relationships.Contains(Parent1), Is.True);
			Assert.That(relationships.Contains(Parent2), Is.True);

			relationships = allRelationships.GetParentsAllLevels(Medium2);
			Assert.That(relationships.Count, Is.EqualTo(2));
			Assert.That(relationships.Contains(Parent1), Is.True);
			Assert.That(relationships.Contains(Parent2), Is.True);

			relationships = allRelationships.GetParentsAllLevels(Single1);
			Assert.That(relationships.Single(), Is.EqualTo(Parent1));

			relationships = allRelationships.GetParentsAllLevels(Single2);
			Assert.That(relationships.Single(), Is.EqualTo(Parent2));
		}

		[Test]
		public void GivenTestSet_WhenGetChildrenAllLevels_ThenReturnAllFound()
		{
			var allRelationships = CreateTestSet();

			var relationships = allRelationships.GetChildrenAllLevels(Parent1);
			Assert.That(relationships.Count, Is.EqualTo(5));
			Assert.That(relationships.Contains(Medium1), Is.True);
			Assert.That(relationships.Contains(Medium2), Is.True);
			Assert.That(relationships.Contains(Single1), Is.True);
			Assert.That(relationships.Contains(Child1), Is.True);
			Assert.That(relationships.Contains(Child2), Is.True);

			relationships = allRelationships.GetChildrenAllLevels(Parent2);
			Assert.That(relationships.Count, Is.EqualTo(5));
			Assert.That(relationships.Contains(Medium1), Is.True);
			Assert.That(relationships.Contains(Medium2), Is.True);
			Assert.That(relationships.Contains(Single2), Is.True);
			Assert.That(relationships.Contains(Child1), Is.True);
			Assert.That(relationships.Contains(Child2), Is.True);

			relationships = allRelationships.GetChildrenAllLevels(Child1);
			Assert.That(relationships.Count, Is.EqualTo(0));

			relationships = allRelationships.GetChildrenAllLevels(Child2);
			Assert.That(relationships.Count, Is.EqualTo(0));

			relationships = allRelationships.GetChildrenAllLevels(Medium1);
			Assert.That(relationships.Count, Is.EqualTo(2));
			Assert.That(relationships.Contains(Child1), Is.True);
			Assert.That(relationships.Contains(Child2), Is.True);

			relationships = allRelationships.GetChildrenAllLevels(Medium2);
			Assert.That(relationships.Count, Is.EqualTo(2));
			Assert.That(relationships.Contains(Child1), Is.True);
			Assert.That(relationships.Contains(Child2), Is.True);

			relationships = allRelationships.GetChildrenAllLevels(Single1);
			Assert.That(relationships.Single(), Is.EqualTo(Child1));

			relationships = allRelationships.GetChildrenAllLevels(Single2);
			Assert.That(relationships.Single(), Is.EqualTo(Child2));
		}

		[Test]
		public void GivenRecursiveLoops_WhenGetChildrenOrParentsAllLevels_ThenNotFail()
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
				Assert.That(list1.Single(), Is.EqualTo(x));
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
				Assert.That(list2.Count, Is.EqualTo(2));
				Assert.That(list2[0], Is.EqualTo(y));
				Assert.That(list2[1], Is.EqualTo(x));
			}
			foreach (var list2 in new[]
			{
				relationshipsLoop2.GetChildrenAllLevels(y),
				relationshipsLoop2.GetParentsAllLevels(y),
			})
			{
				Assert.That(list2.Count, Is.EqualTo(2));
				Assert.That(list2[0], Is.EqualTo(x));
				Assert.That(list2[1], Is.EqualTo(y));
			}

			// 3 variables loop
			const string z = "z";
			var relationshipsLoop3 = new[] { new TestParentChild(x, y), new TestParentChild(y, z), new TestParentChild(z, x) };
			var list3 = relationshipsLoop3.GetChildrenAllLevels(x);
			Assert.That(list3.Count, Is.EqualTo(3));
			Assert.That(list3[0], Is.EqualTo(y));
			Assert.That(list3[1], Is.EqualTo(z));
			Assert.That(list3[2], Is.EqualTo(x));
			list3 = relationshipsLoop3.GetParentsAllLevels(x);
			Assert.That(list3.Count, Is.EqualTo(3));
			Assert.That(list3[0], Is.EqualTo(z));
			Assert.That(list3[1], Is.EqualTo(y));
			Assert.That(list3[2], Is.EqualTo(x));
			list3 = relationshipsLoop3.GetChildrenAllLevels(y);
			Assert.That(list3.Count, Is.EqualTo(3));
			Assert.That(list3[0], Is.EqualTo(z));
			Assert.That(list3[1], Is.EqualTo(x));
			Assert.That(list3[2], Is.EqualTo(y));
			list3 = relationshipsLoop3.GetParentsAllLevels(y);
			Assert.That(list3.Count, Is.EqualTo(3));
			Assert.That(list3[0], Is.EqualTo(x));
			Assert.That(list3[1], Is.EqualTo(z));
			Assert.That(list3[2], Is.EqualTo(y));
			list3 = relationshipsLoop3.GetChildrenAllLevels(z);
			Assert.That(list3.Count, Is.EqualTo(3));
			Assert.That(list3[0], Is.EqualTo(x));
			Assert.That(list3[1], Is.EqualTo(y));
			Assert.That(list3[2], Is.EqualTo(z));
			list3 = relationshipsLoop3.GetParentsAllLevels(z);
			Assert.That(list3.Count, Is.EqualTo(3));
			Assert.That(list3[0], Is.EqualTo(y));
			Assert.That(list3[1], Is.EqualTo(x));
			Assert.That(list3[2], Is.EqualTo(z));
		}

		[Test]
		public void GivenTestSet_WhenGetParentsAllLevels_ThenExplanationContainsFoundRelationships()
		{
			var allRelationships = CreateTestSet();
			var explanation = new List<TestParentChild>();

			allRelationships.GetParentsAllLevels(Child1, explanation);
			Assert.That(explanation.Count, Is.EqualTo(8));
			Assert.That(explanation.Any(relationship => relationship.Parent == Medium1 && relationship.Child == Child1), Is.True);
			Assert.That(explanation.Any(relationship => relationship.Parent == Medium2 && relationship.Child == Child1), Is.True);
			Assert.That(explanation.Any(relationship => relationship.Parent == Single1 && relationship.Child == Child1), Is.True);
			Assert.That(explanation.Any(relationship => relationship.Parent == Parent1 && relationship.Child == Medium1), Is.True);
			Assert.That(explanation.Any(relationship => relationship.Parent == Parent2 && relationship.Child == Medium1), Is.True);
			Assert.That(explanation.Any(relationship => relationship.Parent == Parent1 && relationship.Child == Medium2), Is.True);
			Assert.That(explanation.Any(relationship => relationship.Parent == Parent2 && relationship.Child == Medium2), Is.True);
			Assert.That(explanation.Any(relationship => relationship.Parent == Parent1 && relationship.Child == Single1), Is.True);
		}

		[Test]
		public void GivenValidStructure_WhenFindPath_ThenFindAny()
		{
			var parent1 = ConceptCreationHelper.CreateEmptyConcept();
			var topMedium1 = ConceptCreationHelper.CreateEmptyConcept();
			var bottomMedium1 = ConceptCreationHelper.CreateEmptyConcept();
			var child1 = ConceptCreationHelper.CreateEmptyConcept();
			var parent2 = ConceptCreationHelper.CreateEmptyConcept();
			var topMedium2 = ConceptCreationHelper.CreateEmptyConcept();
			var bottomMedium2 = ConceptCreationHelper.CreateEmptyConcept();
			var child2 = ConceptCreationHelper.CreateEmptyConcept();
			var parentNoConnection = ConceptCreationHelper.CreateEmptyConcept();
			var childNoConnection = ConceptCreationHelper.CreateEmptyConcept();
			var parent2Path = ConceptCreationHelper.CreateEmptyConcept();
			var medium2Path1 = ConceptCreationHelper.CreateEmptyConcept();
			var medium2Path2 = ConceptCreationHelper.CreateEmptyConcept();
			var child2Path = ConceptCreationHelper.CreateEmptyConcept();

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
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), parent1, child1).Any(), Is.True);
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), parent2, child2).Any(), Is.True);
			// invalid direct paths
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), parent1, child2).Any(), Is.False);
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), parent2, child1).Any(), Is.False);
			// invalid reverted paths
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), child1, parent1).Any(), Is.False);
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), child2, parent2).Any(), Is.False);
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), child1, parent2).Any(), Is.False);
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), child2, parent1).Any(), Is.False);
			// other type path
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), parentNoConnection, childNoConnection).Any(), Is.False);
			Assert.That(statements.FindPath<IConcept>(typeof(IsStatement), parentNoConnection, childNoConnection).Any(), Is.True);
			// 2 paths
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), parent2Path, child2Path).Any(), Is.True);
			Assert.That(statements.FindPath<IConcept>(typeof(TestPath), child2Path, parent2Path).Any(), Is.False);
		}

		[Test]
		public void GivenRecursiveLoops_WhenFindPath_ThenNotFail()
		{
			var a = ConceptCreationHelper.CreateEmptyConcept();
			var b = ConceptCreationHelper.CreateEmptyConcept();
			var c = ConceptCreationHelper.CreateEmptyConcept();

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
				Assert.That(statements.FindPath<IConcept>(typeof(TestPath), a, a).Any(), Is.True);
			}

			Assert.That(statementsInvalid.FindPath<IConcept>(typeof(TestPath), a, a).Any(), Is.False);
		}

		[Test]
		public void GivenValidTree_WhenGetChildrenTree_ThenReturnIt()
		{
			// arrange
			var concept1 =    1.CreateConceptByObject();
			var concept11 =   11.CreateConceptByObject();
			var concept12 =   12.CreateConceptByObject();
			var concept13 =   13.CreateConceptByObject();
			var concept111 =  111.CreateConceptByObject();
			var concept112 =  112.CreateConceptByObject();
			var concept113 =  113.CreateConceptByObject();
			var concept121 =  121.CreateConceptByObject();
			var concept122 =  122.CreateConceptByObject();
			var concept123 =  123.CreateConceptByObject();
			var concept131 =  131.CreateConceptByObject();
			var concept132 =  132.CreateConceptByObject();
			var concept133 =  133.CreateConceptByObject();
			var concept1111 = 1111.CreateConceptByObject();
			var concept1112 = 1112.CreateConceptByObject();
			var concept1113 = 1113.CreateConceptByObject();
			var concept1121 = 1121.CreateConceptByObject();
			var concept1122 = 1122.CreateConceptByObject();
			var concept1123 = 1123.CreateConceptByObject();
			var concept1131 = 1131.CreateConceptByObject();
			var concept1132 = 1132.CreateConceptByObject();
			var concept1133 = 1133.CreateConceptByObject();

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
			Assert.That(resultTree, Is.Not.Null);
			Assert.That(resultTree.Value, Is.SameAs(concept11));
			Assert.That(resultTree.Children.Count, Is.EqualTo(3));

			var result111 = resultTree.Children.Single(node => node.Value == concept111);
			Assert.That(result111.Value, Is.SameAs(concept111));
			var result112 = resultTree.Children.Single(node => node.Value == concept112);
			Assert.That(result112.Value, Is.SameAs(concept112));
			var result113 = resultTree.Children.Single(node => node.Value == concept113);
			Assert.That(result113.Value, Is.SameAs(concept113));

			var result1111 = result111.Children.Single(node => node.Value == concept1111);
			Assert.That(result1111.Value, Is.SameAs(concept1111));
			var result1112 = result111.Children.Single(node => node.Value == concept1112);
			Assert.That( result1112.Value, Is.SameAs(concept1112));
			var result1113 = result111.Children.Single(node => node.Value == concept1113);
			Assert.That(result1113.Value, Is.SameAs(concept1113));

			var result1121 = result112.Children.Single(node => node.Value == concept1121);
			Assert.That(result1121.Value, Is.SameAs(concept1121));
			var result1122 = result112.Children.Single(node => node.Value == concept1122);
			Assert.That(result1122.Value, Is.SameAs(concept1122));
			var result1123 = result112.Children.Single(node => node.Value == concept1123);
			Assert.That(result1123.Value, Is.SameAs(concept1123));

			var result1131 = result113.Children.Single(node => node.Value == concept1131);
			Assert.That(result1131.Value, Is.SameAs(concept1131));
			var result1132 = result113.Children.Single(node => node.Value == concept1132);
			Assert.That(result1132.Value, Is.SameAs(concept1132));
			var result1133 = result113.Children.Single(node => node.Value == concept1133);
			Assert.That(result1133.Value, Is.SameAs(concept1133));

			Assert.That(involvedStatements.Count, Is.EqualTo(statements.Count - 3 - 6));
		}

		[Test]
		public void GivenInvolvedStatements_WhenGetChildrenOneLevel_ThenReturnThem()
		{
			// arrange
			var semanticNetwork = CreateBearsSample();
			var animalsConcept = semanticNetwork.Concepts["Kingdom: Animalia"];

			// act
			var animals = semanticNetwork.Statements.GetChildrenOneLevel<IConcept, IsStatement>(animalsConcept).Select(concept => concept.ID).ToList();
			var bears = semanticNetwork.Statements.GetChildrenOneLevel<IConcept, IsStatement>(semanticNetwork.Concepts["Genus: Ursus"]).Select(concept => concept.ID).ToList();

			// assert
			Assert.That(animals.Single(), Is.EqualTo("Phylum: Chordata"));

			Assert.That(bears.Count, Is.EqualTo(4));
			Assert.That(bears.Contains("Ursus americanus"), Is.True);
			Assert.That(bears.Contains("Ursus arctos"), Is.True);
			Assert.That(bears.Contains("Ursus maritimus"), Is.True);
			Assert.That(bears.Contains("Ursus thibetanuss"), Is.True);
		}

		[Test]
		public void GivenInvolvedStatements_WhenGetChildrenAllLevels_ThenReturnThem()
		{
			// arrange
			var semanticNetwork = CreateBearsSample();
			var animalsConcept = semanticNetwork.Concepts["Kingdom: Animalia"];
			var ignoredConcepts = new[] { LogicalValues.True, LogicalValues.False, animalsConcept };

			// act
			var animals = semanticNetwork.Statements.GetChildrenAllLevels<IConcept, IsStatement>(semanticNetwork.Concepts["Kingdom: Animalia"]).Select(concept => concept.ID).ToList();
			var bears = semanticNetwork.Statements.GetChildrenAllLevels<IConcept, IsStatement>(semanticNetwork.Concepts["Genus: Ursus"]).Select(concept => concept.ID).ToList();

			// assert
			Assert.That(animals.Count, Is.EqualTo(semanticNetwork.Concepts.Count - ignoredConcepts.Length));
			foreach (var animal in semanticNetwork.Concepts.Except(ignoredConcepts))
			{
				Assert.That(animals.Contains(animal.ID), Is.True);
			}

			Assert.That(bears.Count, Is.EqualTo(4));
			Assert.That(bears.Contains("Ursus americanus"), Is.True);
			Assert.That(bears.Contains("Ursus arctos"), Is.True);
			Assert.That(bears.Contains("Ursus maritimus"), Is.True);
			Assert.That(bears.Contains("Ursus thibetanuss"), Is.True);
		}

		private static ISemanticNetwork CreateBearsSample()
		{
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			ISemanticNetwork semanticNetwork = new SemanticNetwork(Language.Default).WithModules(modules);

			IConcept animal = "Kingdom: Animalia".CreateConceptById("Animal");
			IConcept chordate = "Phylum: Chordata".CreateConceptById("Chordate");
			IConcept mammal = "Class: Mammalia".CreateConceptById("Mammal");
			IConcept carnivor = "Order: Carnivora".CreateConceptById("Carnivor");
			// Just skip Ursidae, Ursinae and Ursini for short.
			IConcept bear = "Genus: Ursus".CreateConceptById("Bear");
			IConcept americanBlackBear = "Ursus americanus".CreateConceptById("American black bear");
			IConcept brownBear = "Ursus arctos".CreateConceptById("Brown bear");
			IConcept polarBear = "Ursus maritimus".CreateConceptById("Polar bear");
			IConcept asianBlackBear = "Ursus thibetanuss".CreateConceptById("Asian black bear");

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

		private static List<TestParentChild> CreateTestSet()
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
