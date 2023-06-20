using System.Windows.Media;

using Inventor.Semantics.WPF.Converters;
using Inventor.Semantics.WPF.Properties;

namespace Inventor.Semantics.WPF.TreeNodes
{
	public class ConceptNode : ExtendedTreeNode
	{
		#region Properties

		public override string Text
		{ get { return _concept.Name.GetValue(_application.CurrentLanguage); } }

		public override string Tooltip
		{ get { return _concept.Hint.GetValue(_application.CurrentLanguage); } }

		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Concept.ToSource()); } }

		public IConcept Concept
		{ get { return _concept; } }

		private static ImageSource _icon;
		private readonly IConcept _concept;
		private readonly IInventorApplication _application;

		#endregion

		public ConceptNode(IConcept concept, IInventorApplication application)
		{
			_concept = concept;
			_application = application;
		}
	}
}