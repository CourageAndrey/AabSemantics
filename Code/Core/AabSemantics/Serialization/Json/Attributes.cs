using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Json
{
	/// <summary>
	/// Converts concept attributes to and from JSON. Since attributes are stateless, they are
	/// persisted as bare element names rather than objects.
	/// </summary>
	public static class Attributes
	{
		/// <summary>Converts attributes into their JSON element names.</summary>
		/// <param name="attributes">Attributes to convert.</param>
		/// <returns>One element name per attribute.</returns>
		/// <exception cref="KeyNotFoundException">An attribute's exact type is not registered; unlike elsewhere, base types are not tried.</exception>
		public static List<String> ToJson(this IEnumerable<IAttribute> attributes)
		{
			var attributeDefinitions = Repositories.Attributes.Definitions;
			return attributes.Select(a => attributeDefinitions[a.GetType()].GetSerializationSettings<AttributeJsonSerializationSettings>().JsonElementName).ToList();
		}

		/// <summary>Resolves JSON element names back into attribute instances.</summary>
		/// <param name="attributes">Element names to resolve.</param>
		/// <returns>Lazily evaluated attribute instances.</returns>
		/// <exception cref="InvalidOperationException">No registered attribute uses one of the names.</exception>
		public static IEnumerable<IAttribute> ToAttributes(this IEnumerable<String> attributes)
		{
			var attributeDefinitions = Repositories.Attributes.Definitions.Values;
			return attributes.Select(a => attributeDefinitions.First(d => d.GetSerializationSettings<AttributeJsonSerializationSettings>().JsonElementName == a).Value);
		}
	}
}
