using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;

namespace AabSemantics.SimpleWpfClient
{
	partial class NaturalScience
	{
		#region Properties

		public readonly IConcept ChemicalElement, ChemicalCompound;

		public readonly IConcept Group, Period, Block;

		public readonly IConcept
			Hydrogen,
			Helium,
			Lithium,
			Beryllium,
			Boron,
			Carbon,
			Nitrogen,
			Oxygen,
			Fluorine,
			Neon,
			Sodium,
			Magnesium,
			Aluminium,
			Silicon,
			Phosphorus,
			Sulfur,
			Chlorine,
			Argon,
			Potassium,
			Calcium,
			Scandium,
			Titanium,
			Vanadium,
			Chromium,
			Manganese,
			Iron,
			Cobalt,
			Nickel,
			Copper,
			Zinc,
			Gallium,
			Germanium,
			Arsenic,
			Selenium,
			Bromine,
			Krypton,
			Rubidium,
			Strontium,
			Yttrium,
			Zirconium,
			Niobium,
			Molybdenum,
			Technetium,
			Ruthenium,
			Rhodium,
			Palladium,
			Silver,
			Cadmium,
			Indium,
			Tin,
			Antimony,
			Tellurium,
			Iodine,
			Xenon,
			Caesium,
			Barium,
			Lanthanum,
			Cerium,
			Praseodymium,
			Neodymium,
			Promethium,
			Samarium,
			Europium,
			Gadolinium,
			Terbium,
			Dysprosium,
			Holmium,
			Erbium,
			Thulium,
			Ytterbium,
			Lutetium,
			Hafnium,
			Tantalum,
			Tungsten,
			Rhenium,
			Osmium,
			Iridium,
			Platinum,
			Gold,
			Mercury,
			Thallium,
			Lead,
			Bismuth,
			Polonium,
			Astatine,
			Radon,
			Francium,
			Radium,
			Actinium,
			Thorium,
			Protactinium,
			Uranium,
			Neptunium,
			Plutonium,
			Americium,
			Curium,
			Berkelium,
			Californium,
			Einsteinium,
			Fermium,
			Mendelevium,
			Nobelium,
			Lawrencium,
			Rutherfordium,
			Dubnium,
			Seaborgium,
			Bohrium,
			Hassium,
			Meitnerium,
			Darmstadtium,
			Roentgenium,
			Copernicium,
			Nihonium,
			Flerovium,
			Moscovium,
			Livermorium,
			Tennessine,
			Oganesson;

		public readonly IReadOnlyList<IConcept> ByNumber;
		public readonly IReadOnlyDictionary<String, IConcept> BySymbol;
		public readonly IReadOnlyDictionary<String, IConcept> ByName;
		public readonly IReadOnlyDictionary<Byte, IConcept> Groups;
		public readonly IReadOnlyDictionary<Byte, IConcept> Periods;
		public readonly IReadOnlyDictionary<String, IConcept> Blocks;

		#endregion

		private IConcept DeclareChemicalElement(
			Byte atomicNumber,
			String symbol,
			String name,
			Byte group,
			Byte period,
			String block,
			IList<IConcept> byNumber,
			IDictionary<String, IConcept> bySymbol,
			IDictionary<String, IConcept> byName)
		{
			var element = new Concept(
				$"{atomicNumber}. {symbol}",
				new LocalizedStringConstant(language => symbol),
				new LocalizedStringConstant(language => name));

			SemanticNetwork.Concepts.Add(element);
			SemanticNetwork.DeclareThat(element).IsDescendantOf(ChemicalElement);
			SemanticNetwork.DeclareThat(element).HasSignValue(Group, Groups[group]);
			SemanticNetwork.DeclareThat(element).HasSignValue(Period, Periods[period]);
			SemanticNetwork.DeclareThat(element).HasSignValue(Block, Blocks[block]);

			byNumber.Add(element);
			bySymbol[symbol] = element;
			byName[name] = element;

			return element;
		}

		private ReadOnlyDictionary<KeyT, IConcept> DeclareEnumeration<KeyT>(IConcept sign, String name, IEnumerable<KeyT> keys)
		{
			var concepts = new Dictionary<KeyT, IConcept>();
			foreach (var key in keys)
			{
				concepts[key] = $"{name}: {key}".CreateConceptByName();
			}

			sign.WithAttribute(IsSignAttribute.Value);
			SemanticNetwork.Concepts.Add(sign);

			foreach (var concept in concepts.Values)
			{
				concept.WithAttribute(IsValueAttribute.Value);
				SemanticNetwork.Concepts.Add(concept);
				SemanticNetwork.DeclareThat(concept).IsDescendantOf(sign);
			}

			return new ReadOnlyDictionary<KeyT, IConcept>(concepts);
		}
	}
}