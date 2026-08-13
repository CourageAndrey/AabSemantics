using System.Windows.Media;

using AabSemantics.Extensions.WPF.Converters;
using AabSemantics.Extensions.WPF.Properties;

namespace AabSemantics.Extensions.WPF.TreeNodes
{
	/// <summary>Tree node representing a single concept.</summary>
	public class ConceptNode : ExtendedTreeNode
	{
		#region Properties

		/// <summary>Caption shown in the tree.</summary>
		public override string Text
		{ get { return _concept.Name.GetValue(_application.CurrentLanguage); } }

		/// <summary>Tooltip shown for the node.</summary>
		public override string Tooltip
		{ get { return _concept.Hint.GetValue(_application.CurrentLanguage); } }

		/// <summary>Icon shown next to the caption.</summary>
		public override ImageSource Icon
		{ get { return _icon ?? (_icon = Resources.Concept.ToSource()); } }

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get { return _concept; } }

		private static ImageSource _icon;
		private readonly IConcept _concept;
		private readonly IInventorApplication _application;

		#endregion

		/// <summary>Creates the node.</summary>
		/// <param name="concept">Concept the node represents.</param>
		/// <param name="application">The hosting application.</param>
		public ConceptNode(IConcept concept, IInventorApplication application)
		{
			_concept = concept;
			_application = application;
		}
	}
}