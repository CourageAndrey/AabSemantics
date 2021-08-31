using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IAttributeRepository
	{
		IDictionary<Type, AttributeDefinition> AttributeDefinitions
		{ get; }

		void DefineAttribute(AttributeDefinition attributeDefinition);
	}
}
