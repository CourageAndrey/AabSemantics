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
	public class ChemicalElements
	{
		#region List

		public readonly ISemanticNetwork SemanticNetwork;

		public readonly IConcept Science;

		public readonly IConcept Chemistry;

		public readonly IConcept ChemicalElement;

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

		private IConcept Declare(
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

		public ChemicalElements(ISemanticNetwork semanticNetwork)
		{
			SemanticNetwork = semanticNetwork ?? throw new ArgumentNullException(nameof(semanticNetwork));

			semanticNetwork.Concepts.Add(Science = "Science".CreateConceptByName());
			semanticNetwork.Concepts.Add(Chemistry = "Chemistry".CreateConceptByName());
			semanticNetwork.Concepts.Add(ChemicalElement = "ChemicalElement".CreateConceptByName());
			semanticNetwork.DeclareThat(Science).HasPart(Chemistry);
			semanticNetwork.DeclareThat(Science).IsAncestorOf(Chemistry);
			semanticNetwork.DeclareThat(ChemicalElement).BelongsToSubjectArea(Chemistry);

			var groups = new Dictionary<Byte, IConcept>
			{
				{  0, "Group:  0".CreateConceptByName() },
				{  1, "Group:  1".CreateConceptByName() },
				{  2, "Group:  2".CreateConceptByName() },
				{  3, "Group:  3".CreateConceptByName() },
				{  4, "Group:  4".CreateConceptByName() },
				{  5, "Group:  5".CreateConceptByName() },
				{  6, "Group:  6".CreateConceptByName() },
				{  7, "Group:  7".CreateConceptByName() },
				{  8, "Group:  8".CreateConceptByName() },
				{  9, "Group:  9".CreateConceptByName() },
				{ 10, "Group: 10".CreateConceptByName() },
				{ 11, "Group: 11".CreateConceptByName() },
				{ 12, "Group: 12".CreateConceptByName() },
				{ 13, "Group: 13".CreateConceptByName() },
				{ 14, "Group: 14".CreateConceptByName() },
				{ 15, "Group: 15".CreateConceptByName() },
				{ 16, "Group: 16".CreateConceptByName() },
				{ 17, "Group: 17".CreateConceptByName() },
				{ 18, "Group: 18".CreateConceptByName() },
			};
			Group = "Group".CreateConceptByName().WithAttribute(IsSignAttribute.Value);
			semanticNetwork.Concepts.Add(Group);
			foreach (var group in groups.Values)
			{
				group.WithAttribute(IsValueAttribute.Value);
				semanticNetwork.Concepts.Add(group);
				semanticNetwork.DeclareThat(group).IsDescendantOf(Group);
			}
			Groups = new ReadOnlyDictionary<Byte, IConcept>(groups);

			var periods = new Dictionary<Byte, IConcept>
			{
				{ 1, "Period: 1".CreateConceptByName() },
				{ 2, "Period: 2".CreateConceptByName() },
				{ 3, "Period: 3".CreateConceptByName() },
				{ 4, "Period: 4".CreateConceptByName() },
				{ 5, "Period: 5".CreateConceptByName() },
				{ 6, "Period: 6".CreateConceptByName() },
				{ 7, "Period: 7".CreateConceptByName() },
			};
			Period = "Period".CreateConceptByName().WithAttribute(IsSignAttribute.Value);
			semanticNetwork.Concepts.Add(Period);
			foreach (var period in periods.Values)
			{
				period.WithAttribute(IsValueAttribute.Value);
				semanticNetwork.Concepts.Add(period);
				semanticNetwork.DeclareThat(period).IsDescendantOf(Period);
			}
			Periods = new ReadOnlyDictionary<Byte, IConcept>(periods);

			var blocks = new Dictionary<String, IConcept>
			{
				{ "s", "Block: s".CreateConceptByName() },
				{ "p", "Block: p".CreateConceptByName() },
				{ "d", "Block: d".CreateConceptByName() },
				{ "f", "Block: f".CreateConceptByName() },
				{ "f-block", "Block: f-block".CreateConceptByName() },
			};
			Block = "Block".CreateConceptByName().WithAttribute(IsSignAttribute.Value);
			semanticNetwork.Concepts.Add(Block);
			foreach (var block in blocks.Values)
			{
				block.WithAttribute(IsValueAttribute.Value);
				semanticNetwork.Concepts.Add(block);
				semanticNetwork.DeclareThat(block).IsDescendantOf(Block);
			}
			Blocks = new ReadOnlyDictionary<String, IConcept>(blocks);

			var bySymbol = new Dictionary<String, IConcept>();
			var byNumber = new List<IConcept> { null };
			var byName = new Dictionary<String, IConcept>();

			Hydrogen		= Declare(1,	"H",	nameof(Hydrogen),		1,	1, "s", byNumber, bySymbol, byName);
			Helium			= Declare(2,	"He",	nameof(Helium),			18,	1, "s", byNumber, bySymbol, byName);
			Lithium			= Declare(3,	"Li",	nameof(Lithium),		1,	2, "s", byNumber, bySymbol, byName);
			Beryllium		= Declare(4,	"Be",	nameof(Beryllium),		2,	2, "s", byNumber, bySymbol, byName);
			Boron			= Declare(5,	"B",	nameof(Boron),			13,	2, "p", byNumber, bySymbol, byName);
			Carbon			= Declare(6,	"C",	nameof(Carbon),			14,	2, "p", byNumber, bySymbol, byName);
			Nitrogen		= Declare(7,	"N",	nameof(Nitrogen),		15,	2, "p", byNumber, bySymbol, byName);
			Oxygen			= Declare(8,	"O",	nameof(Oxygen),			16,	2, "p", byNumber, bySymbol, byName);
			Fluorine		= Declare(9,	"F",	nameof(Fluorine),		17,	2, "p", byNumber, bySymbol, byName);
			Neon			= Declare(10,	"Ne",	nameof(Neon),			18,	2, "p", byNumber, bySymbol, byName);
			Sodium			= Declare(11,	"Na",	nameof(Sodium),			1,	3, "s", byNumber, bySymbol, byName);
			Magnesium		= Declare(12,	"Mg",	nameof(Magnesium),		2,	3, "s", byNumber, bySymbol, byName);
			Aluminium		= Declare(13,	"Al",	nameof(Aluminium),		13,	3, "p", byNumber, bySymbol, byName);
			Silicon			= Declare(14,	"Si",	nameof(Silicon),		14,	3, "p", byNumber, bySymbol, byName);
			Phosphorus		= Declare(15,	"P",	nameof(Phosphorus),		15,	3, "p", byNumber, bySymbol, byName);
			Sulfur			= Declare(16,	"S",	nameof(Sulfur),			16,	3, "p", byNumber, bySymbol, byName);
			Chlorine		= Declare(17,	"Cl",	nameof(Chlorine),		17,	3, "p", byNumber, bySymbol, byName);
			Argon			= Declare(18,	"Ar",	nameof(Argon),			18,	3, "p", byNumber, bySymbol, byName);
			Potassium		= Declare(19,	"K",	nameof(Potassium),		1,	4, "s", byNumber, bySymbol, byName);
			Calcium			= Declare(20,	"Ca",	nameof(Calcium),		2,	4, "s", byNumber, bySymbol, byName);
			Scandium		= Declare(21,	"Sc",	nameof(Scandium),		3,	4, "d", byNumber, bySymbol, byName);
			Titanium		= Declare(22,	"Ti",	nameof(Titanium),		4,	4, "d", byNumber, bySymbol, byName);
			Vanadium		= Declare(23,	"V",	nameof(Vanadium),		5,	4, "d", byNumber, bySymbol, byName);
			Chromium		= Declare(24,	"Cr",	nameof(Chromium),		6,	4, "d", byNumber, bySymbol, byName);
			Manganese		= Declare(25,	"Mn",	nameof(Manganese),		7,	4, "d", byNumber, bySymbol, byName);
			Iron			= Declare(26,	"Fe",	nameof(Iron),			8,	4, "d", byNumber, bySymbol, byName);
			Cobalt			= Declare(27,	"Co",	nameof(Cobalt),			9,	4, "d", byNumber, bySymbol, byName);
			Nickel			= Declare(28,	"Ni",	nameof(Nickel),			10,	4, "d", byNumber, bySymbol, byName);
			Copper			= Declare(29,	"Cu",	nameof(Copper),			11,	4, "d", byNumber, bySymbol, byName);
			Zinc			= Declare(30,	"Zn",	nameof(Zinc),			12,	4, "d", byNumber, bySymbol, byName);
			Gallium			= Declare(31,	"Ga",	nameof(Gallium),		13,	4, "p", byNumber, bySymbol, byName);
			Germanium		= Declare(32,	"Ge",	nameof(Germanium),		14,	4, "p", byNumber, bySymbol, byName);
			Arsenic			= Declare(33,	"As",	nameof(Arsenic),		15,	4, "p", byNumber, bySymbol, byName);
			Selenium		= Declare(34,	"Se",	nameof(Selenium),		16,	4, "p", byNumber, bySymbol, byName);
			Bromine			= Declare(35,	"Br",	nameof(Bromine),		17,	4, "p", byNumber, bySymbol, byName);
			Krypton			= Declare(36,	"Kr",	nameof(Krypton),		18,	4, "p", byNumber, bySymbol, byName);
			Rubidium		= Declare(37,	"Rb",	nameof(Rubidium),		1,	5, "s", byNumber, bySymbol, byName);
			Strontium		= Declare(38,	"Sr",	nameof(Strontium),		2,	5, "s", byNumber, bySymbol, byName);
			Yttrium			= Declare(39,	"Y",	nameof(Yttrium),		3,	5, "d", byNumber, bySymbol, byName);
			Zirconium		= Declare(40,	"Zr",	nameof(Zirconium),		4,	5, "d", byNumber, bySymbol, byName);
			Niobium			= Declare(41,	"Nb",	nameof(Niobium),		5,	5, "d", byNumber, bySymbol, byName);
			Molybdenum		= Declare(42,	"Mo",	nameof(Molybdenum),		6,	5, "d", byNumber, bySymbol, byName);
			Technetium		= Declare(43,	"Tc",	nameof(Technetium),		7,	5, "d", byNumber, bySymbol, byName);
			Ruthenium		= Declare(44,	"Ru",	nameof(Ruthenium),		8,	5, "d", byNumber, bySymbol, byName);
			Rhodium			= Declare(45,	"Rh",	nameof(Rhodium),		9,	5, "d", byNumber, bySymbol, byName);
			Palladium		= Declare(46,	"Pd",	nameof(Palladium),		10,	5, "d", byNumber, bySymbol, byName);
			Silver			= Declare(47,	"Ag",	nameof(Silver),			11,	5, "d", byNumber, bySymbol, byName);
			Cadmium			= Declare(48,	"Cd",	nameof(Cadmium),		12,	5, "d", byNumber, bySymbol, byName);
			Indium			= Declare(49,	"In",	nameof(Indium),			13,	5, "p", byNumber, bySymbol, byName);
			Tin				= Declare(50,	"Sn",	nameof(Tin),			14,	5, "p", byNumber, bySymbol, byName);
			Antimony		= Declare(51,	"Sb",	nameof(Antimony),		15,	5, "p", byNumber, bySymbol, byName);
			Tellurium		= Declare(52,	"Te",	nameof(Tellurium),		16,	5, "p", byNumber, bySymbol, byName);
			Iodine			= Declare(53,	"I",	nameof(Iodine),			17,	5, "p", byNumber, bySymbol, byName);
			Xenon			= Declare(54,	"Xe",	nameof(Xenon),			18,	5, "p", byNumber, bySymbol, byName);
			Caesium			= Declare(55,	"Cs",	nameof(Caesium),		1,	6, "s", byNumber, bySymbol, byName);
			Barium			= Declare(56,	"Ba",	nameof(Barium),			2,	6, "s", byNumber, bySymbol, byName);
			Lanthanum		= Declare(57,	"La",	nameof(Lanthanum),		0,	6, "f-block", byNumber, bySymbol, byName);
			Cerium			= Declare(58,	"Ce",	nameof(Cerium),			0,	6, "f-block", byNumber, bySymbol, byName);
			Praseodymium	= Declare(59,	"Pr",	nameof(Praseodymium),	0,	6, "f-block", byNumber, bySymbol, byName);
			Neodymium		= Declare(60,	"Nd",	nameof(Neodymium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Promethium		= Declare(61,	"Pm",	nameof(Promethium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Samarium		= Declare(62,	"Sm",	nameof(Samarium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Europium		= Declare(63,	"Eu",	nameof(Europium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Gadolinium		= Declare(64,	"Gd",	nameof(Gadolinium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Terbium			= Declare(65,	"Tb",	nameof(Terbium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Dysprosium		= Declare(66,	"Dy",	nameof(Dysprosium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Holmium			= Declare(67,	"Ho",	nameof(Holmium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Erbium			= Declare(68,	"Er",	nameof(Erbium),			0,	6, "f-block", byNumber, bySymbol, byName);
			Thulium			= Declare(69,	"Tm",	nameof(Thulium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Ytterbium		= Declare(70,	"Yb",	nameof(Ytterbium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Lutetium		= Declare(71,	"Lu",	nameof(Lutetium),		3,	6, "d", byNumber, bySymbol, byName);
			Hafnium			= Declare(72,	"Hf",	nameof(Hafnium),		4,	6, "d", byNumber, bySymbol, byName);
			Tantalum		= Declare(73,	"Ta",	nameof(Tantalum),		5,	6, "d", byNumber, bySymbol, byName);
			Tungsten		= Declare(74,	"W",	nameof(Tungsten),		6,	6, "d", byNumber, bySymbol, byName);
			Rhenium			= Declare(75,	"Re",	nameof(Rhenium),		7,	6, "d", byNumber, bySymbol, byName);
			Osmium			= Declare(76,	"Os",	nameof(Osmium),			8,	6, "d", byNumber, bySymbol, byName);
			Iridium			= Declare(77,	"Ir",	nameof(Iridium),		9,	6, "d", byNumber, bySymbol, byName);
			Platinum		= Declare(78,	"Pt",	nameof(Platinum),		10,	6, "d", byNumber, bySymbol, byName);
			Gold			= Declare(79,	"Au",	nameof(Gold),			11,	6, "d", byNumber, bySymbol, byName);
			Mercury			= Declare(80,	"Hg",	nameof(Mercury),		12,	6, "d", byNumber, bySymbol, byName);
			Thallium		= Declare(81,	"Tl",	nameof(Thallium),		13,	6, "p", byNumber, bySymbol, byName);
			Lead			= Declare(82,	"Pb",	nameof(Lead),			14,	6, "p", byNumber, bySymbol, byName);
			Bismuth			= Declare(83,	"Bi",	nameof(Bismuth),		15,	6, "p", byNumber, bySymbol, byName);
			Polonium		= Declare(84,	"Po",	nameof(Polonium),		16,	6, "p", byNumber, bySymbol, byName);
			Astatine		= Declare(85,	"At",	nameof(Astatine),		17,	6, "p", byNumber, bySymbol, byName);
			Radon			= Declare(86,	"Rn",	nameof(Radon),			18,	6, "p", byNumber, bySymbol, byName);
			Francium		= Declare(87,	"Fr",	nameof(Francium),		1,	7, "s", byNumber, bySymbol, byName);
			Radium			= Declare(88,	"Ra",	nameof(Radium),			2,	7, "s", byNumber, bySymbol, byName);
			Actinium		= Declare(89,	"Ac",	nameof(Actinium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Thorium			= Declare(90,	"Th",	nameof(Thorium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Protactinium	= Declare(91,	"Pa",	nameof(Protactinium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Uranium			= Declare(92,	"U",	nameof(Uranium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Neptunium		= Declare(93,	"Np",	nameof(Neptunium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Plutonium		= Declare(94,	"Pu",	nameof(Plutonium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Americium		= Declare(95,	"Am",	nameof(Americium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Curium			= Declare(96,	"Cm",	nameof(Curium),			0,	7, "f-block", byNumber, bySymbol, byName);
			Berkelium		= Declare(97,	"Bk",	nameof(Berkelium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Californium		= Declare(98,	"Cf",	nameof(Californium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Einsteinium		= Declare(99,	"Es",	nameof(Einsteinium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Fermium			= Declare(100,	"Fm",	nameof(Fermium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Mendelevium		= Declare(101,	"Md",	nameof(Mendelevium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Nobelium		= Declare(102,	"No",	nameof(Nobelium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Lawrencium		= Declare(103,	"Lr",	nameof(Lawrencium),		3,	7, "d", byNumber, bySymbol, byName);
			Rutherfordium	= Declare(104,	"Rf",	nameof(Rutherfordium),	4,	7, "d", byNumber, bySymbol, byName);
			Dubnium			= Declare(105,	"Db",	nameof(Dubnium),		5,	7, "d", byNumber, bySymbol, byName);
			Seaborgium		= Declare(106,	"Sg",	nameof(Seaborgium),		6,	7, "d", byNumber, bySymbol, byName);
			Bohrium			= Declare(107,	"Bh",	nameof(Bohrium),		7,	7, "d", byNumber, bySymbol, byName);
			Hassium			= Declare(108,	"Hs",	nameof(Hassium),		8,	7, "d", byNumber, bySymbol, byName);
			Meitnerium		= Declare(109,	"Mt",	nameof(Meitnerium),		9,	7, "d", byNumber, bySymbol, byName);
			Darmstadtium	= Declare(110,	"Ds",	nameof(Darmstadtium),	10,	7, "d", byNumber, bySymbol, byName);
			Roentgenium		= Declare(111,	"Rg",	nameof(Roentgenium),	11,	7, "d", byNumber, bySymbol, byName);
			Copernicium		= Declare(112,	"Cn",	nameof(Copernicium),	12,	7, "d", byNumber, bySymbol, byName);
			Nihonium		= Declare(113,	"Nh",	nameof(Nihonium),		13,	7, "p", byNumber, bySymbol, byName);
			Flerovium		= Declare(114,	"Fl",	nameof(Flerovium),		14,	7, "p", byNumber, bySymbol, byName);
			Moscovium		= Declare(115,	"Mc",	nameof(Moscovium),		15,	7, "p", byNumber, bySymbol, byName);
			Livermorium		= Declare(116,	"Lv",	nameof(Livermorium),	16,	7, "p", byNumber, bySymbol, byName);
			Tennessine		= Declare(117,	"Ts",	nameof(Tennessine),		17,	7, "p", byNumber, bySymbol, byName);
			Oganesson		= Declare(118,	"Og",	nameof(Oganesson),		18,	7, "p", byNumber, bySymbol, byName);

			ByNumber = byNumber.ToArray();
			BySymbol = new ReadOnlyDictionary<String, IConcept>(bySymbol);
			ByName = new ReadOnlyDictionary<String, IConcept>(byName);
		}
	}
}