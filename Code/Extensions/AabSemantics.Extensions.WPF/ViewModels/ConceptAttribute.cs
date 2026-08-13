using System.ComponentModel;
using System.Threading;

using AabSemantics.Metadata;

namespace AabSemantics.Extensions.WPF.ViewModels
{
	/// <summary>Checkbox row in the concept editor, standing for one attribute the concept may carry.</summary>
	public class ConceptAttribute : INotifyPropertyChanged
	{
		#region Properties

		/// <summary>Display name.</summary>
		public string Name
		{ get; }

		/// <summary>The value concept.</summary>
		public IAttribute Value
		{ get; }

		/// <summary>Whether the attribute is set on the concept.</summary>
		public bool IsOn
		{ get; set; }

		#endregion

		/// <summary>Creates the row.</summary>
		/// <param name="attributeDefinition">Attribute the row stands for.</param>
		/// <param name="language">Language its name is shown in.</param>
		/// <param name="isOn">Whether the concept currently carries the attribute.</param>
		public ConceptAttribute(AttributeDefinition attributeDefinition, ILanguage language, bool isOn)
		{
			if (attributeDefinition != null)
			{
				Name = attributeDefinition.GetName(language);
				Value = attributeDefinition.Value;
			}
			else
			{
				Name = language.Attributes.None;
				Value = null;
			}

			IsOn = isOn;
		}

		/// <summary>Raised when a bound property changes.</summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Clears the flag without raising a change notification.</summary>
		internal void SwitchOff()
		{
			IsOn = false;
			var handler = Volatile.Read(ref PropertyChanged);
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(nameof(IsOn)));
			}
		}
	}
}
