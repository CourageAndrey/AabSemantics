using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public static class DefinitionRepositoryHelper
	{
		public static DefinitionT GetSuitable<InstanceT, DefinitionT>(this IDictionary<Type, DefinitionT> repository, InstanceT instance)
		{
			var type = instance.GetType();

			while (type != null)
			{
				DefinitionT definition;
				if (repository.TryGetValue(type, out definition))
				{
					return definition;
				}

				type = type.BaseType;
			}

			throw new NotSupportedException();
		}
	}
}
