using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

		private IConcept DeclareElement(
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

		public ChemicalElements(ISemanticNetwork semanticNetwork)
		{
			SemanticNetwork = semanticNetwork ?? throw new ArgumentNullException(nameof(semanticNetwork));

			semanticNetwork.Concepts.Add(Science = "Science".CreateConceptByName());
			semanticNetwork.Concepts.Add(Chemistry = "Chemistry".CreateConceptByName());
			semanticNetwork.Concepts.Add(ChemicalElement = "ChemicalElement".CreateConceptByName());
			semanticNetwork.DeclareThat(Science).HasPart(Chemistry);
			semanticNetwork.DeclareThat(Science).IsAncestorOf(Chemistry);
			semanticNetwork.DeclareThat(ChemicalElement).BelongsToSubjectArea(Chemistry);

			Group = nameof(Group).CreateConceptByName();
			Groups = DeclareEnumeration(Group, nameof(Group), Enumerable.Range(0, 19).Select(i => (Byte) i));

			Period = nameof(Period).CreateConceptByName();
			Periods = DeclareEnumeration(Period, nameof(Period), Enumerable.Range(1, 7).Select(i => (Byte) i));

			Block = nameof(Block).CreateConceptByName();
			Blocks = DeclareEnumeration(Block, nameof(Block), new[] { "s", "p", "d", "f", "f-block" });

			var bySymbol = new Dictionary<String, IConcept>();
			var byNumber = new List<IConcept> { null };
			var byName = new Dictionary<String, IConcept>();

			Hydrogen		= DeclareElement(1,	"H",	nameof(Hydrogen),		1,	1, "s", byNumber, bySymbol, byName);
			Helium			= DeclareElement(2,	"He",	nameof(Helium),			18,	1, "s", byNumber, bySymbol, byName);
			Lithium			= DeclareElement(3,	"Li",	nameof(Lithium),		1,	2, "s", byNumber, bySymbol, byName);
			Beryllium		= DeclareElement(4,	"Be",	nameof(Beryllium),		2,	2, "s", byNumber, bySymbol, byName);
			Boron			= DeclareElement(5,	"B",	nameof(Boron),			13,	2, "p", byNumber, bySymbol, byName);
			Carbon			= DeclareElement(6,	"C",	nameof(Carbon),			14,	2, "p", byNumber, bySymbol, byName);
			Nitrogen		= DeclareElement(7,	"N",	nameof(Nitrogen),		15,	2, "p", byNumber, bySymbol, byName);
			Oxygen			= DeclareElement(8,	"O",	nameof(Oxygen),			16,	2, "p", byNumber, bySymbol, byName);
			Fluorine		= DeclareElement(9,	"F",	nameof(Fluorine),		17,	2, "p", byNumber, bySymbol, byName);
			Neon			= DeclareElement(10,	"Ne",	nameof(Neon),			18,	2, "p", byNumber, bySymbol, byName);
			Sodium			= DeclareElement(11,	"Na",	nameof(Sodium),			1,	3, "s", byNumber, bySymbol, byName);
			Magnesium		= DeclareElement(12,	"Mg",	nameof(Magnesium),		2,	3, "s", byNumber, bySymbol, byName);
			Aluminium		= DeclareElement(13,	"Al",	nameof(Aluminium),		13,	3, "p", byNumber, bySymbol, byName);
			Silicon			= DeclareElement(14,	"Si",	nameof(Silicon),		14,	3, "p", byNumber, bySymbol, byName);
			Phosphorus		= DeclareElement(15,	"P",	nameof(Phosphorus),		15,	3, "p", byNumber, bySymbol, byName);
			Sulfur			= DeclareElement(16,	"S",	nameof(Sulfur),			16,	3, "p", byNumber, bySymbol, byName);
			Chlorine		= DeclareElement(17,	"Cl",	nameof(Chlorine),		17,	3, "p", byNumber, bySymbol, byName);
			Argon			= DeclareElement(18,	"Ar",	nameof(Argon),			18,	3, "p", byNumber, bySymbol, byName);
			Potassium		= DeclareElement(19,	"K",	nameof(Potassium),		1,	4, "s", byNumber, bySymbol, byName);
			Calcium			= DeclareElement(20,	"Ca",	nameof(Calcium),		2,	4, "s", byNumber, bySymbol, byName);
			Scandium		= DeclareElement(21,	"Sc",	nameof(Scandium),		3,	4, "d", byNumber, bySymbol, byName);
			Titanium		= DeclareElement(22,	"Ti",	nameof(Titanium),		4,	4, "d", byNumber, bySymbol, byName);
			Vanadium		= DeclareElement(23,	"V",	nameof(Vanadium),		5,	4, "d", byNumber, bySymbol, byName);
			Chromium		= DeclareElement(24,	"Cr",	nameof(Chromium),		6,	4, "d", byNumber, bySymbol, byName);
			Manganese		= DeclareElement(25,	"Mn",	nameof(Manganese),		7,	4, "d", byNumber, bySymbol, byName);
			Iron			= DeclareElement(26,	"Fe",	nameof(Iron),			8,	4, "d", byNumber, bySymbol, byName);
			Cobalt			= DeclareElement(27,	"Co",	nameof(Cobalt),			9,	4, "d", byNumber, bySymbol, byName);
			Nickel			= DeclareElement(28,	"Ni",	nameof(Nickel),			10,	4, "d", byNumber, bySymbol, byName);
			Copper			= DeclareElement(29,	"Cu",	nameof(Copper),			11,	4, "d", byNumber, bySymbol, byName);
			Zinc			= DeclareElement(30,	"Zn",	nameof(Zinc),			12,	4, "d", byNumber, bySymbol, byName);
			Gallium			= DeclareElement(31,	"Ga",	nameof(Gallium),		13,	4, "p", byNumber, bySymbol, byName);
			Germanium		= DeclareElement(32,	"Ge",	nameof(Germanium),		14,	4, "p", byNumber, bySymbol, byName);
			Arsenic			= DeclareElement(33,	"As",	nameof(Arsenic),		15,	4, "p", byNumber, bySymbol, byName);
			Selenium		= DeclareElement(34,	"Se",	nameof(Selenium),		16,	4, "p", byNumber, bySymbol, byName);
			Bromine			= DeclareElement(35,	"Br",	nameof(Bromine),		17,	4, "p", byNumber, bySymbol, byName);
			Krypton			= DeclareElement(36,	"Kr",	nameof(Krypton),		18,	4, "p", byNumber, bySymbol, byName);
			Rubidium		= DeclareElement(37,	"Rb",	nameof(Rubidium),		1,	5, "s", byNumber, bySymbol, byName);
			Strontium		= DeclareElement(38,	"Sr",	nameof(Strontium),		2,	5, "s", byNumber, bySymbol, byName);
			Yttrium			= DeclareElement(39,	"Y",	nameof(Yttrium),		3,	5, "d", byNumber, bySymbol, byName);
			Zirconium		= DeclareElement(40,	"Zr",	nameof(Zirconium),		4,	5, "d", byNumber, bySymbol, byName);
			Niobium			= DeclareElement(41,	"Nb",	nameof(Niobium),		5,	5, "d", byNumber, bySymbol, byName);
			Molybdenum		= DeclareElement(42,	"Mo",	nameof(Molybdenum),		6,	5, "d", byNumber, bySymbol, byName);
			Technetium		= DeclareElement(43,	"Tc",	nameof(Technetium),		7,	5, "d", byNumber, bySymbol, byName);
			Ruthenium		= DeclareElement(44,	"Ru",	nameof(Ruthenium),		8,	5, "d", byNumber, bySymbol, byName);
			Rhodium			= DeclareElement(45,	"Rh",	nameof(Rhodium),		9,	5, "d", byNumber, bySymbol, byName);
			Palladium		= DeclareElement(46,	"Pd",	nameof(Palladium),		10,	5, "d", byNumber, bySymbol, byName);
			Silver			= DeclareElement(47,	"Ag",	nameof(Silver),			11,	5, "d", byNumber, bySymbol, byName);
			Cadmium			= DeclareElement(48,	"Cd",	nameof(Cadmium),		12,	5, "d", byNumber, bySymbol, byName);
			Indium			= DeclareElement(49,	"In",	nameof(Indium),			13,	5, "p", byNumber, bySymbol, byName);
			Tin				= DeclareElement(50,	"Sn",	nameof(Tin),			14,	5, "p", byNumber, bySymbol, byName);
			Antimony		= DeclareElement(51,	"Sb",	nameof(Antimony),		15,	5, "p", byNumber, bySymbol, byName);
			Tellurium		= DeclareElement(52,	"Te",	nameof(Tellurium),		16,	5, "p", byNumber, bySymbol, byName);
			Iodine			= DeclareElement(53,	"I",	nameof(Iodine),			17,	5, "p", byNumber, bySymbol, byName);
			Xenon			= DeclareElement(54,	"Xe",	nameof(Xenon),			18,	5, "p", byNumber, bySymbol, byName);
			Caesium			= DeclareElement(55,	"Cs",	nameof(Caesium),		1,	6, "s", byNumber, bySymbol, byName);
			Barium			= DeclareElement(56,	"Ba",	nameof(Barium),			2,	6, "s", byNumber, bySymbol, byName);
			Lanthanum		= DeclareElement(57,	"La",	nameof(Lanthanum),		0,	6, "f-block", byNumber, bySymbol, byName);
			Cerium			= DeclareElement(58,	"Ce",	nameof(Cerium),			0,	6, "f-block", byNumber, bySymbol, byName);
			Praseodymium	= DeclareElement(59,	"Pr",	nameof(Praseodymium),	0,	6, "f-block", byNumber, bySymbol, byName);
			Neodymium		= DeclareElement(60,	"Nd",	nameof(Neodymium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Promethium		= DeclareElement(61,	"Pm",	nameof(Promethium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Samarium		= DeclareElement(62,	"Sm",	nameof(Samarium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Europium		= DeclareElement(63,	"Eu",	nameof(Europium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Gadolinium		= DeclareElement(64,	"Gd",	nameof(Gadolinium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Terbium			= DeclareElement(65,	"Tb",	nameof(Terbium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Dysprosium		= DeclareElement(66,	"Dy",	nameof(Dysprosium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Holmium			= DeclareElement(67,	"Ho",	nameof(Holmium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Erbium			= DeclareElement(68,	"Er",	nameof(Erbium),			0,	6, "f-block", byNumber, bySymbol, byName);
			Thulium			= DeclareElement(69,	"Tm",	nameof(Thulium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Ytterbium		= DeclareElement(70,	"Yb",	nameof(Ytterbium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Lutetium		= DeclareElement(71,	"Lu",	nameof(Lutetium),		3,	6, "d", byNumber, bySymbol, byName);
			Hafnium			= DeclareElement(72,	"Hf",	nameof(Hafnium),		4,	6, "d", byNumber, bySymbol, byName);
			Tantalum		= DeclareElement(73,	"Ta",	nameof(Tantalum),		5,	6, "d", byNumber, bySymbol, byName);
			Tungsten		= DeclareElement(74,	"W",	nameof(Tungsten),		6,	6, "d", byNumber, bySymbol, byName);
			Rhenium			= DeclareElement(75,	"Re",	nameof(Rhenium),		7,	6, "d", byNumber, bySymbol, byName);
			Osmium			= DeclareElement(76,	"Os",	nameof(Osmium),			8,	6, "d", byNumber, bySymbol, byName);
			Iridium			= DeclareElement(77,	"Ir",	nameof(Iridium),		9,	6, "d", byNumber, bySymbol, byName);
			Platinum		= DeclareElement(78,	"Pt",	nameof(Platinum),		10,	6, "d", byNumber, bySymbol, byName);
			Gold			= DeclareElement(79,	"Au",	nameof(Gold),			11,	6, "d", byNumber, bySymbol, byName);
			Mercury			= DeclareElement(80,	"Hg",	nameof(Mercury),		12,	6, "d", byNumber, bySymbol, byName);
			Thallium		= DeclareElement(81,	"Tl",	nameof(Thallium),		13,	6, "p", byNumber, bySymbol, byName);
			Lead			= DeclareElement(82,	"Pb",	nameof(Lead),			14,	6, "p", byNumber, bySymbol, byName);
			Bismuth			= DeclareElement(83,	"Bi",	nameof(Bismuth),		15,	6, "p", byNumber, bySymbol, byName);
			Polonium		= DeclareElement(84,	"Po",	nameof(Polonium),		16,	6, "p", byNumber, bySymbol, byName);
			Astatine		= DeclareElement(85,	"At",	nameof(Astatine),		17,	6, "p", byNumber, bySymbol, byName);
			Radon			= DeclareElement(86,	"Rn",	nameof(Radon),			18,	6, "p", byNumber, bySymbol, byName);
			Francium		= DeclareElement(87,	"Fr",	nameof(Francium),		1,	7, "s", byNumber, bySymbol, byName);
			Radium			= DeclareElement(88,	"Ra",	nameof(Radium),			2,	7, "s", byNumber, bySymbol, byName);
			Actinium		= DeclareElement(89,	"Ac",	nameof(Actinium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Thorium			= DeclareElement(90,	"Th",	nameof(Thorium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Protactinium	= DeclareElement(91,	"Pa",	nameof(Protactinium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Uranium			= DeclareElement(92,	"U",	nameof(Uranium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Neptunium		= DeclareElement(93,	"Np",	nameof(Neptunium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Plutonium		= DeclareElement(94,	"Pu",	nameof(Plutonium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Americium		= DeclareElement(95,	"Am",	nameof(Americium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Curium			= DeclareElement(96,	"Cm",	nameof(Curium),			0,	7, "f-block", byNumber, bySymbol, byName);
			Berkelium		= DeclareElement(97,	"Bk",	nameof(Berkelium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Californium		= DeclareElement(98,	"Cf",	nameof(Californium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Einsteinium		= DeclareElement(99,	"Es",	nameof(Einsteinium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Fermium			= DeclareElement(100,	"Fm",	nameof(Fermium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Mendelevium		= DeclareElement(101,	"Md",	nameof(Mendelevium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Nobelium		= DeclareElement(102,	"No",	nameof(Nobelium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Lawrencium		= DeclareElement(103,	"Lr",	nameof(Lawrencium),		3,	7, "d", byNumber, bySymbol, byName);
			Rutherfordium	= DeclareElement(104,	"Rf",	nameof(Rutherfordium),	4,	7, "d", byNumber, bySymbol, byName);
			Dubnium			= DeclareElement(105,	"Db",	nameof(Dubnium),		5,	7, "d", byNumber, bySymbol, byName);
			Seaborgium		= DeclareElement(106,	"Sg",	nameof(Seaborgium),		6,	7, "d", byNumber, bySymbol, byName);
			Bohrium			= DeclareElement(107,	"Bh",	nameof(Bohrium),		7,	7, "d", byNumber, bySymbol, byName);
			Hassium			= DeclareElement(108,	"Hs",	nameof(Hassium),		8,	7, "d", byNumber, bySymbol, byName);
			Meitnerium		= DeclareElement(109,	"Mt",	nameof(Meitnerium),		9,	7, "d", byNumber, bySymbol, byName);
			Darmstadtium	= DeclareElement(110,	"Ds",	nameof(Darmstadtium),	10,	7, "d", byNumber, bySymbol, byName);
			Roentgenium		= DeclareElement(111,	"Rg",	nameof(Roentgenium),	11,	7, "d", byNumber, bySymbol, byName);
			Copernicium		= DeclareElement(112,	"Cn",	nameof(Copernicium),	12,	7, "d", byNumber, bySymbol, byName);
			Nihonium		= DeclareElement(113,	"Nh",	nameof(Nihonium),		13,	7, "p", byNumber, bySymbol, byName);
			Flerovium		= DeclareElement(114,	"Fl",	nameof(Flerovium),		14,	7, "p", byNumber, bySymbol, byName);
			Moscovium		= DeclareElement(115,	"Mc",	nameof(Moscovium),		15,	7, "p", byNumber, bySymbol, byName);
			Livermorium		= DeclareElement(116,	"Lv",	nameof(Livermorium),	16,	7, "p", byNumber, bySymbol, byName);
			Tennessine		= DeclareElement(117,	"Ts",	nameof(Tennessine),		17,	7, "p", byNumber, bySymbol, byName);
			Oganesson		= DeclareElement(118,	"Og",	nameof(Oganesson),		18,	7, "p", byNumber, bySymbol, byName);

			ByNumber = byNumber.ToArray();
			BySymbol = new ReadOnlyDictionary<String, IConcept>(bySymbol);
			ByName = new ReadOnlyDictionary<String, IConcept>(byName);
		}
	}
}