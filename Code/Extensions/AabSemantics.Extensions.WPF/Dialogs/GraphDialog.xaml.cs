using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using AabSemantics.Metadata;

namespace AabSemantics.Extensions.WPF.Dialogs
{
	public partial class GraphDialog
	{
		public GraphDialog()
		{
			InitializeComponent();

			_statementColors = Repositories.Statements.Definitions.Keys.GetDifferentColors();
			foreach (var colorMapping in _statementColors)
			{
				_listBoxLegend.Items.Add(new ListBoxItem
				{
					Content = colorMapping.Key.Name,
					Background = colorMapping.Value,
				});
			}
		}

		#region Properties

		private IInventorApplication _application;
		private ISemanticNetwork _semanticNetwork;
		private IConcept _selectedConcept;
		private FrameworkElement _selectedConceptView;
		private readonly IDictionary<IConcept, FrameworkElement> _relatedConcepts = new Dictionary<IConcept, FrameworkElement>();
		private readonly IDictionary<IStatement, ICollection<FrameworkElement>> _visibleStatements = new Dictionary<IStatement, ICollection<FrameworkElement>>();
		private readonly IDictionary<Type, Brush> _statementColors;

		public IInventorApplication Application
		{
			get { return _application; }
			set
			{
				_application = value;
				_semanticNetwork = value?.SemanticNetwork;
				_legendButton.ToolTip = _application.CurrentLanguage.GetExtension<IWpfUiModule>().Misc.NameCategoryStatements;
				selectConcept(_selectedConcept);
			}
		}

		public IConcept SelectedConcept
		{
			get { return _selectedConcept; }
			set { selectConcept(value); }
		}

		public ICollection<IConcept> RelatedConcepts
		{ get { return _relatedConcepts.Keys; } }

		public ICollection<IStatement> VisibleStatements
		{ get { return _visibleStatements.Keys; } }

		#endregion

		private void selectConcept(IConcept concept)
		{
			_selectedConcept = getFirstConcept(_semanticNetwork, concept);

			_relatedConcepts.Clear();
			_visibleStatements.Clear();

			_screen.Children.Clear();
			_screen.Children.Add(_legendButton);
			_screen.Children.Add(_popupLegend);

			if (_semanticNetwork != null && _selectedConcept != null)
			{
				_selectedConceptView = displayConcept(_selectedConcept, Brushes.Aqua);

				double centerX = _screen.ActualWidth / 2;
				double centerY = _screen.ActualHeight / 2;
				double radius = Math.Min(centerX, centerY) * 0.8;

				var maxSize = new Size(_screen.ActualWidth, _screen.ActualHeight);
				var sourceSize = _selectedConceptView.GetDesiredSize(maxSize);
				Canvas.SetLeft(_selectedConceptView, centerX);
				Canvas.SetTop(_selectedConceptView, centerY);
				centerX += sourceSize.Width / 2;
				centerY += sourceSize.Height / 2;

				foreach (var statement in _semanticNetwork.Statements.Where(r => r.GetChildConcepts().Contains(_selectedConcept)))
				{
					_visibleStatements[statement] = displayStatement(_selectedConcept, statement);
				}

				double angleBetweenRelatedConcepts = Math.PI * 2 / _relatedConcepts.Count;

				double angle = 0;
				foreach (var relatedConceptView in _relatedConcepts.Values)
				{
					angle = adjustNodePosition(relatedConceptView, centerX, centerY, radius, angle, angleBetweenRelatedConcepts);
				}

				foreach (var statements in _visibleStatements)
				{
					foreach (var statement in statements.Value)
					{
						adjustConnectorPosition((Line) statement, centerX, centerY, maxSize);
					}
				}
			}
		}

		private static IConcept getFirstConcept(ISemanticNetwork semanticNetwork, IConcept concept)
		{
			if (semanticNetwork == null)
			{
				return null;
			}

			if (concept == null)
			{
				foreach (var statement in semanticNetwork.Statements)
				{
					concept = statement.GetChildConcepts().FirstOrDefault();
					if (concept != null)
					{
						break;
					}
				}
			}

			return concept;
		}

		private FrameworkElement displayConcept(IConcept concept, Brush foregroudBrush)
		{
			var textBlock = new TextBlock
			{
				Text = concept.Name.GetValue(_application.CurrentLanguage),
				Foreground = Brushes.Black,
				Background = foregroudBrush,
			};

			textBlock.MouseDown += (s, e) => selectConcept(concept);

			var conceptView = new Border
			{
				Child = textBlock,
				BorderThickness = new Thickness(1),
				BorderBrush = Brushes.DarkGray,
				Tag = concept,
			};

			_screen.Children.Add(conceptView);

			return conceptView;
		}

		private ICollection<FrameworkElement> displayStatement(IConcept sourceConcept, IStatement statement)
		{
			ICollection<FrameworkElement> statementViews;
			if (!_visibleStatements.TryGetValue(statement, out statementViews))
			{
				_visibleStatements[statement] = statementViews = new List<FrameworkElement>();

				foreach (var relatedConcept in statement.GetChildConcepts().Except(new[] { sourceConcept }))
				{
					FrameworkElement conceptView;
					if (!_relatedConcepts.TryGetValue(relatedConcept, out conceptView))
					{
						_relatedConcepts[relatedConcept] = conceptView = displayConcept(relatedConcept, Brushes.AntiqueWhite);
					}

					Brush brush;
					if (!_statementColors.TryGetValue(statement.GetType(), out brush))
					{
						brush = Brushes.Black;
					}

					var statementView = new Line
					{
						Stroke = brush,
						StrokeThickness = 2,
						Tag = relatedConcept,
						ToolTip = TextRenders.PlainString.Render(statement.DescribeTrue(), _application.CurrentLanguage).ToString(),
					};

					_screen.Children.Add(statementView);

					statementViews.Add(statementView);
				}
			}

			return statementViews;
		}

		private static double adjustNodePosition(FrameworkElement nodeView, double centerX, double centerY, double radius, double angle, double angleBetweenConcepts)
		{
			Canvas.SetLeft(nodeView, centerX + radius * Math.Cos(angle));
			Canvas.SetTop(nodeView, centerY + radius * Math.Sin(angle));

			return angle + angleBetweenConcepts;
		}

		private void adjustConnectorPosition(Line connectorView, double centerX, double centerY, Size maxSize)
		{
			var destinationConcept = (IConcept) connectorView.Tag;
			var destinationControl = _relatedConcepts[destinationConcept];
			var destinationSize = destinationControl.GetDesiredSize(maxSize);

			connectorView.X1 = centerX;
			connectorView.Y1 = centerY;
			connectorView.X2 = Canvas.GetLeft(destinationControl) + destinationSize.Width / 2;
			connectorView.Y2 = Canvas.GetTop(destinationControl) + destinationSize.Height / 2;

			Panel.SetZIndex(connectorView, -1);
		}

		private void OnNeedRedraw(object sender, RoutedEventArgs e)
		{
			selectConcept(_selectedConcept);
		}

		private void _legendButtonClick(object sender, RoutedEventArgs e)
		{
			if (_popupLegend.IsOpen)
			{
				_popupLegend.IsOpen = false;
				_legendButton.Content = ">>>";
			}
			else
			{
				_popupLegend.IsOpen = true;
				_legendButton.Content = "<<<";
			}
		}
	}
}
