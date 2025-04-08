using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.TestCore
{
	public static class SerializationTestHelper
	{
		private static readonly ITextRender _textRender = TextRenders.PlainString;
		private static readonly ILanguage _language = Language.Default;

		public static void CheckSerialization<T, SnapshotT>(
			this T item,
			Func<T, SnapshotT> itemToSnapshot,
			Func<SnapshotT, string> snapshotToString,
			Func<string, Type, SnapshotT> stringToSnapshot,
			Func<SnapshotT, T> snapshotToItem)
		{
			// arrange
			var propertiesToCompare = item
				.GetType()
				.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

			// act
			var snapshot = itemToSnapshot(item);
			var snapshotType = snapshot.GetType();
			string serialized = snapshotToString(snapshot);
			var restoredSnapshot = stringToSnapshot(serialized, snapshotType);
			var restored = snapshotToItem(restoredSnapshot);

			// assert
			Assert.That(restored.GetType(), Is.SameAs(item.GetType()));
			foreach (var property in propertiesToCompare)
			{
				property.AssertEqual(item, restored);
			}
		}

		public static void AssertEqual(this PropertyInfo property, object left, object right)
		{
			object leftValue = property.GetValue(left);
			object rightValue = property.GetValue(right);
			Type propertyType = property.PropertyType;

			if (typeof(IExplanation).IsAssignableFrom(propertyType))
			{
				leftValue = ((IExplanation) leftValue).Statements;
				rightValue = ((IExplanation) rightValue).Statements;
				propertyType = typeof(ICollection<IStatement>);
			}

			if (propertyType == typeof(bool))
			{
				Assert.That(leftValue, Is.EqualTo(rightValue));
			}
			else if(propertyType == typeof(string))
			{
				Assert.That(leftValue, Is.EqualTo(rightValue));
			}
			else if (propertyType == typeof(IDictionary<string, IConcept>))
			{
				var leftDictionary = (IDictionary<string, IConcept>) leftValue;
				var rightDictionary = (IDictionary<string, IConcept>) rightValue;
				Assert.That(leftDictionary.SequenceEqual(rightDictionary), Is.True);
			}
			else if (typeof(IConcept).IsAssignableFrom(propertyType) ||
					typeof(IStatement).IsAssignableFrom(propertyType))
			{
				Assert.That(leftValue, Is.SameAs(rightValue));
			}
			else if (typeof(IEnumerable<IConcept>).IsAssignableFrom(propertyType))
			{
				var leftCollection = (IEnumerable<IConcept>) leftValue;
				var rightCollection = (IEnumerable<IConcept>) rightValue;
				Assert.That(leftCollection.SequenceEqual(rightCollection), Is.True);
			}
			else if (typeof(IEnumerable<IStatement>).IsAssignableFrom(propertyType))
			{
				var leftCollection = (IEnumerable<IStatement>) leftValue;
				var rightCollection = (IEnumerable<IStatement>) rightValue;
				Assert.That(leftCollection.SequenceEqual(rightCollection), Is.True);
			}
			else if (typeof(IText).IsAssignableFrom(propertyType))
			{
				var leftText = (IText) leftValue;
				var rightText = (IText) rightValue;
				var leftString = _textRender.RenderText(leftText, _language).ToString();
				var rightString = _textRender.RenderText(rightText, _language).ToString();
				Assert.That(leftString, Is.EqualTo(rightString));
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public static void CheckJsonSerialization(
			this IQuestion question,
			ConceptIdResolver conceptIdResolver,
			StatementIdResolver statementIdResolver)
		{
			question.CheckSerialization(
				item => Serialization.Json.Question.Load(question),
				snapshot => snapshot.SerializeToJsonString(),
				(serialized, snapshotType) =>
				{
					var serializer = snapshotType.AcquireJsonSerializer();
					using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(serialized)))
					{
						return (Serialization.Json.Question) serializer.ReadObject(stream);
					}
				},
				snapshot => snapshot.Save(conceptIdResolver, statementIdResolver));
		}

		public static void CheckXmlSerialization(
			this IQuestion question,
			ConceptIdResolver conceptIdResolver,
			StatementIdResolver statementIdResolver)
		{
			question.CheckSerialization(
				item => Serialization.Xml.Question.Load(question),
				snapshot => snapshot.SerializeToXmlString(),
				(serialized, snapshotType) =>
				{
					var serializer = snapshotType.AcquireXmlSerializer();
					using (var stringReader = new StringReader(serialized))
					{
						using (var xmlStringReader = new XmlTextReader(stringReader))
						{
							return (Serialization.Xml.Question) serializer.Deserialize(xmlStringReader);
						}
					}
				},
				snapshot => snapshot.Save(conceptIdResolver, statementIdResolver));
		}
	}
}
