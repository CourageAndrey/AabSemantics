﻿using System.Linq; using System.Windows.Controls;  using AabSemantics.Extensions.WPF.ViewModels; using AabSemantics.Metadata;  namespace AabSemantics.Extensions.WPF.Controls { 	public partial class CustomStatementControl : IStatementEditor 	{ 		public CustomStatementControl() 		{ 			InitializeComponent();  			_comboBoxType.MakeAutoComplete(); 		}  		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language) 		{ 			_comboBoxType.ItemsSource = Repositories.CustomStatements.Values;  			var wrappedConcepts = semanticNetwork.Concepts.Select(c => new ConceptItem(c, language)).ToList(); 			_columnConcept.ItemsSource = wrappedConcepts;  			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing; 			_groupID.Header = languageEditing.PropertyID; 			_groupType.Header = languageEditing.PropertyType; 			_groupConcepts.Header = languageEditing.PropertyConcepts; 			_columnKey.Header = languageEditing.PropertyKey; 			_columnConcept.Header = languageEditing.PropertyConcept; 		}  		public StatementViewModel Statement 		{ 			get { return _contextControl.DataContext as ViewModels.Statements.CustomStatement; } 			set 			{ 				_contextControl.DataContext = value; 				_idControl.IsReadOnly = value.BoundStatement?.Context is Contexts.SystemContext; 			} 		}  		private void _selectedTypeChanged(object sender, SelectionChangedEventArgs e) 		{ 			var statement = (ViewModels.Statements.CustomStatement) _contextControl.DataContext; 			statement.Concepts.Clear();  			var newType = _comboBoxType.SelectedItem as CustomStatementDefinition; 			if (newType != null) 			{ 				foreach (var concept in newType.Concepts) 				{ 					statement.Concepts.Add(new ConceptWithKey(concept, null)); 				} 			} 		} 	} } 