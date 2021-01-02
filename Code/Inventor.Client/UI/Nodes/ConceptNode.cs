using System.Windows.Media;

using Inventor.Client.Properties;
using Inventor.Core;
using Inventor.Core.Localization;

using Sef.UI;

namespace Inventor.Client.UI.Nodes
{
	public sealed class ConceptNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _concept.Name.GetValue(LanguageEx.CurrentEx); } }

		public override string Tooltip
		{ get { return _concept.Hint.GetValue(LanguageEx.CurrentEx); } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Concept.ToSource()); } }

		public Concept Concept
		{ get { return _concept; } }

		private static ImageSource _icon;
		private readonly Concept _concept;

		#endregion

		public ConceptNode(Concept concept)
		{
			_concept = concept;
		}
	}
}