using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IAttributeRepository
	{
		IDictionary<Type, AttributeDefinition> Definitions
		{ get; }

		void Define(AttributeDefinition attributeDefinition);
	}
}
