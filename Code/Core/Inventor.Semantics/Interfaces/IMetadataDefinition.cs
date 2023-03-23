using System;

namespace Inventor.Semantics
{
	public interface IMetadataDefinition
	{
		Type Type
		{ get; }

		Type XmlType
		{ get; }

		Type JsonType
		{ get; }
	}
}
