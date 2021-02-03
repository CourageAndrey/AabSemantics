using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;

namespace Inventor.Core.Base
{
	public class AttributeRepository : IAttributeRepository
	{
		public IDictionary<Type, AttributeDefinition> AttributeDefinitions
		{ get; }

		public AttributeRepository()
		{
			AttributeDefinitions = new Dictionary<Type, AttributeDefinition>
			{
				{ typeof(IsSignAttribute), new AttributeDefinition(typeof(IsSignAttribute), IsSignAttribute.Value, language => language.Attributes.IsSign) },
				{ typeof(IsValueAttribute), new AttributeDefinition(typeof(IsValueAttribute), IsValueAttribute.Value, language => language.Attributes.IsValue) },
				{ typeof(IsProcessAttribute), new AttributeDefinition(typeof(IsProcessAttribute), IsProcessAttribute.Value, language => language.Attributes.IsProcess) },
				{ typeof(IsBooleanAttribute), new AttributeDefinition(typeof(IsBooleanAttribute), IsBooleanAttribute.Value, language => language.Attributes.IsBoolean) },
				{ typeof(IsComparisonSignAttribute), new AttributeDefinition(typeof(IsComparisonSignAttribute), IsComparisonSignAttribute.Value, language => language.Attributes.IsComparisonSign) },
				{ typeof(IsSequenceSignAttribute), new AttributeDefinition(typeof(IsSequenceSignAttribute), IsSequenceSignAttribute.Value, language => language.Attributes.IsSequenceSign) },
			};
		}
	}
}
