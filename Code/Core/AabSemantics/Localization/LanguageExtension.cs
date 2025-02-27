using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageExtension
	{ }

	[XmlType]
	public class LanguageExtension : ILanguageExtension
	{ }
}