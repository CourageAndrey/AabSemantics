using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using AabSemantics.Extensions.WPF.Controls;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Modules.Classification.Localization;

namespace AabSemantics.Extensions.WPF.ViewModels.Statements
{
	public class CustomStatement : StatementViewModel<AabSemantics.Statements.CustomStatement>
	{
		#region Properties

		public string Type
		{ get; set; }

		public ObservableCollection<ConceptWithKey> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatement(ILanguage language)
			: this(string.Empty, string.Empty, new ObservableCollection<ConceptWithKey>(), language)
		{ }

		public CustomStatement(AabSemantics.Statements.CustomStatement statement, ILanguage language)
			: this(statement.ID, statement.Type, new ObservableCollection<ConceptWithKey>(statement.Concepts.Select(c => new ConceptWithKey(c.Key, new ConceptItem(c.Value, language)))), language)
		{
			BoundObject = statement;
		}

		public CustomStatement(string id, string type, ObservableCollection<ConceptWithKey> concepts, ILanguage language)
			: base(id, language)
		{
			Type = type;
			Concepts = new ObservableCollection<ConceptWithKey>(concepts);
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new CustomStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageClassificationModule>().Statements.Names.Classification,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override AabSemantics.Statements.CustomStatement CreateStatementImplementation()
		{
			return new AabSemantics.Statements.CustomStatement(ID, Type, Concepts.ToDictionary(c => c.Key, c => c.Concept.Concept));
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Concepts.ToDictionary(c => c.Key, c => c.Concept.Concept));
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new CustomStatement(ID, Type, Concepts, _language);
		}
	}
}
