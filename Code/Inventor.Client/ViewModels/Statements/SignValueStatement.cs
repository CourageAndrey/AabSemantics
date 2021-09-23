using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;
using Inventor.Core.Localization.Modules;
using Inventor.Set.Localization;

namespace Inventor.Client.ViewModels.Statements
{
	public class SignValueStatement : StatementViewModel<Set.Statements.SignValueStatement>
	{
		#region Properties

		public ConceptItem Concept
		{ get; set; }

		public ConceptItem Sign
		{ get; set; }

		public ConceptItem Value
		{ get; set; }

		#endregion

		#region Constructors

		public SignValueStatement(ILanguage language)
			: this(string.Empty, null, null, null, language)
		{ }

		public SignValueStatement(Set.Statements.SignValueStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Concept, language), new ConceptItem(statement.Sign, language), new ConceptItem(statement.Value, language), language)
		{
			BoundObject = statement;
		}

		public SignValueStatement(string id, ConceptItem concept, ConceptItem sign, ConceptItem value, ILanguage language)
			: base(id, language)
		{
			Concept = concept;
			Sign = sign;
			Value = value;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new SignValueStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageSetModule>().Statements.Names.SignValue,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Set.Statements.SignValueStatement CreateStatementImplementation()
		{
			return new Set.Statements.SignValueStatement(ID, Concept.Concept, Sign.Concept, Value.Concept);
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Concept.Concept, Sign.Concept, Value.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new SignValueStatement(ID, Concept, Sign, Value, _language);
		}
	}
}
