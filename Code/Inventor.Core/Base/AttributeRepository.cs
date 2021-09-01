using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class AttributeRepository : IAttributeRepository
	{
		public IDictionary<Type, AttributeDefinition> Definitions
		{ get; } = new Dictionary<Type, AttributeDefinition>();

		public void Define(AttributeDefinition attributeDefinition)
		{
			Definitions[attributeDefinition.AttributeType] = attributeDefinition;
		}
	}
}
