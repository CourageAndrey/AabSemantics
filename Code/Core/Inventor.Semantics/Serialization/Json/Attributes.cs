using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Json
{
	public static class Attributes
	{
		public static List<String> ToJson(this IEnumerable<IAttribute> attributes)
		{
			var attributeDefinitions = Repositories.Attributes.Definitions;
			return attributes.Select(a => attributeDefinitions[a.GetType()].GetJsonSerializationSettings<AttributeJsonSerializationSettings>().JsonElementName).ToList();
		}

		public static IEnumerable<IAttribute> ToAttributes(this IEnumerable<String> attributes)
		{
			var attributeDefinitions = Repositories.Attributes.Definitions.Values;
			return attributes.Select(a => attributeDefinitions.First(d => d.GetJsonSerializationSettings<AttributeJsonSerializationSettings>().JsonElementName == a).AttributeValue);
		}
	}
}
