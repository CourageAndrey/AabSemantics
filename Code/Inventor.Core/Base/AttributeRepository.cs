using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class AttributeRepository : IAttributeRepository
	{
		public IDictionary<Type, AttributeDefinition> AttributeDefinitions
		{ get; } = new Dictionary<Type, AttributeDefinition>();

		public void DefineAttribute(AttributeDefinition attributeDefinition)
		{
			AttributeDefinitions[attributeDefinition.AttributeType] = attributeDefinition;
		}
	}
}
