using System;
using System.Collections.Generic;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Mathematics.Tests
{
	public static class TestSemanticNetworkExtension
	{
		public static MathematicsTestSemanticNetwork CreateMathematicsTestData(this ISemanticNetwork semanticNetwork)
		{
			return new MathematicsTestSemanticNetwork(semanticNetwork);
		}
	}

	public class MathematicsTestSemanticNetwork
	{
		#region Properties

		public ISemanticNetwork SemanticNetwork
		{ get; }

		#region Comparable Values

		public IConcept Number0
		{ get; }

		public IConcept NumberZero
		{ get; }

		public IConcept NumberNotZero
		{ get; }

		public IConcept Number1
		{ get; }

		public IConcept Number1or2
		{ get; }

		public IConcept Number2
		{ get; }

		public IConcept Number2or3
		{ get; }

		public IConcept Number3
		{ get; }

		public IConcept Number3or4
		{ get; }

		public IConcept Number4
		{ get; }

		#endregion

		#endregion

		public MathematicsTestSemanticNetwork(ISemanticNetwork semanticNetwork)
		{
			#region Semantic network

			SemanticNetwork = semanticNetwork
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>()
				.WithModule<MathematicsModule>();

			#endregion

			#region Comparable Values

			Func<String, LocalizedStringVariable> getString = text => new LocalizedStringVariable(
				new Dictionary<String, String>
				{
					{ "ru-RU", text },
					{ "en-US", text },
				});
			Func<Int32, LocalizedStringVariable> getStringByNumber = number => getString(number.ToString());

			SemanticNetwork.Concepts.Add(Number0 = new Concept("0", getStringByNumber(0), getStringByNumber(0)));

			SemanticNetwork.Concepts.Add(NumberZero = new Concept("_0_", getString("_0_"), getString("_0_")));

			SemanticNetwork.Concepts.Add(NumberNotZero = new Concept("!0", getString("!0"), getString("!0")));

			SemanticNetwork.Concepts.Add(Number1 = new Concept("1", getStringByNumber(1), getStringByNumber(1)));

			SemanticNetwork.Concepts.Add(Number1or2 = new Concept("1 || 2", getString("1 || 2"), getString("1 || 2")));

			SemanticNetwork.Concepts.Add(Number2 = new Concept("2", getStringByNumber(2), getStringByNumber(2)));

			SemanticNetwork.Concepts.Add(Number2or3 = new Concept("2 || 3", getString("2 || 3"), getString("2 || 3")));

			SemanticNetwork.Concepts.Add(Number3 = new Concept("3", getStringByNumber(3), getStringByNumber(3)));

			SemanticNetwork.Concepts.Add(Number3or4 = new Concept("3 || 4", getString("3 || 4"), getString("3 || 4")));

			SemanticNetwork.Concepts.Add(Number4 = new Concept("4", getStringByNumber(4), getStringByNumber(4)));

			#endregion

			#region Concept Attributes

			Number0.WithAttribute(IsValueAttribute.Value);
			NumberZero.WithAttribute(IsValueAttribute.Value);
			NumberNotZero.WithAttribute(IsValueAttribute.Value);
			Number1.WithAttribute(IsValueAttribute.Value);
			Number1or2.WithAttribute(IsValueAttribute.Value);
			Number2.WithAttribute(IsValueAttribute.Value);
			Number2or3.WithAttribute(IsValueAttribute.Value);
			Number3.WithAttribute(IsValueAttribute.Value);
			Number3or4.WithAttribute(IsValueAttribute.Value);
			Number4.WithAttribute(IsValueAttribute.Value);

			#endregion

			#region Statements

			SemanticNetwork.DeclareThat(Number0).IsEqualTo(NumberZero);
			SemanticNetwork.DeclareThat(NumberNotZero).IsNotEqualTo(NumberZero);
			SemanticNetwork.DeclareThat(Number0).IsLessThan(Number1);
			SemanticNetwork.DeclareThat(Number1).IsLessThan(Number2);
			SemanticNetwork.DeclareThat(Number3).IsGreaterThan(Number2);
			SemanticNetwork.DeclareThat(Number4).IsGreaterThan(Number3);
			SemanticNetwork.DeclareThat(Number1).IsLessThanOrEqualTo(Number1or2);
			SemanticNetwork.DeclareThat(Number1or2).IsLessThanOrEqualTo(Number2);
			SemanticNetwork.DeclareThat(Number2).IsLessThanOrEqualTo(Number2or3);
			SemanticNetwork.DeclareThat(Number3).IsGreaterThanOrEqualTo(Number2or3);
			SemanticNetwork.DeclareThat(Number4).IsGreaterThanOrEqualTo(Number3or4);
			SemanticNetwork.DeclareThat(Number3or4).IsGreaterThanOrEqualTo(Number3);

			#endregion
		}
	}
}