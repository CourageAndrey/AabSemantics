using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AabSemantics.Concepts;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;

namespace AabSemantics.SimpleWpfClient
{
	public partial class NaturalScience
	{
		#region Properties

		public readonly ISemanticNetwork SemanticNetwork;

		public readonly IConcept Science;

		public readonly IConcept Chemistry;

		#endregion

		public NaturalScience(ISemanticNetwork semanticNetwork)
		{
			SemanticNetwork = semanticNetwork ?? throw new ArgumentNullException(nameof(semanticNetwork));

			semanticNetwork.Concepts.Add(Science = "Science".CreateConceptByName());
			semanticNetwork.Concepts.Add(Chemistry = "Chemistry".CreateConceptByName());

			#region Chemistry

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

			Hydrogen		= DeclareChemicalElement(1,		"H",	nameof(Hydrogen),		1,	1, "s", byNumber, bySymbol, byName);
			Helium			= DeclareChemicalElement(2,		"He",	nameof(Helium),			18,	1, "s", byNumber, bySymbol, byName);
			Lithium			= DeclareChemicalElement(3,		"Li",	nameof(Lithium),		1,	2, "s", byNumber, bySymbol, byName);
			Beryllium		= DeclareChemicalElement(4,		"Be",	nameof(Beryllium),		2,	2, "s", byNumber, bySymbol, byName);
			Boron			= DeclareChemicalElement(5,		"B",	nameof(Boron),			13,	2, "p", byNumber, bySymbol, byName);
			Carbon			= DeclareChemicalElement(6,		"C",	nameof(Carbon),			14,	2, "p", byNumber, bySymbol, byName);
			Nitrogen		= DeclareChemicalElement(7,		"N",	nameof(Nitrogen),		15,	2, "p", byNumber, bySymbol, byName);
			Oxygen			= DeclareChemicalElement(8,		"O",	nameof(Oxygen),			16,	2, "p", byNumber, bySymbol, byName);
			Fluorine		= DeclareChemicalElement(9,		"F",	nameof(Fluorine),		17,	2, "p", byNumber, bySymbol, byName);
			Neon			= DeclareChemicalElement(10,	"Ne",	nameof(Neon),			18,	2, "p", byNumber, bySymbol, byName);
			Sodium			= DeclareChemicalElement(11,	"Na",	nameof(Sodium),			1,	3, "s", byNumber, bySymbol, byName);
			Magnesium		= DeclareChemicalElement(12,	"Mg",	nameof(Magnesium),		2,	3, "s", byNumber, bySymbol, byName);
			Aluminium		= DeclareChemicalElement(13,	"Al",	nameof(Aluminium),		13,	3, "p", byNumber, bySymbol, byName);
			Silicon			= DeclareChemicalElement(14,	"Si",	nameof(Silicon),		14,	3, "p", byNumber, bySymbol, byName);
			Phosphorus		= DeclareChemicalElement(15,	"P",	nameof(Phosphorus),		15,	3, "p", byNumber, bySymbol, byName);
			Sulfur			= DeclareChemicalElement(16,	"S",	nameof(Sulfur),			16,	3, "p", byNumber, bySymbol, byName);
			Chlorine		= DeclareChemicalElement(17,	"Cl",	nameof(Chlorine),		17,	3, "p", byNumber, bySymbol, byName);
			Argon			= DeclareChemicalElement(18,	"Ar",	nameof(Argon),			18,	3, "p", byNumber, bySymbol, byName);
			Potassium		= DeclareChemicalElement(19,	"K",	nameof(Potassium),		1,	4, "s", byNumber, bySymbol, byName);
			Calcium			= DeclareChemicalElement(20,	"Ca",	nameof(Calcium),		2,	4, "s", byNumber, bySymbol, byName);
			Scandium		= DeclareChemicalElement(21,	"Sc",	nameof(Scandium),		3,	4, "d", byNumber, bySymbol, byName);
			Titanium		= DeclareChemicalElement(22,	"Ti",	nameof(Titanium),		4,	4, "d", byNumber, bySymbol, byName);
			Vanadium		= DeclareChemicalElement(23,	"V",	nameof(Vanadium),		5,	4, "d", byNumber, bySymbol, byName);
			Chromium		= DeclareChemicalElement(24,	"Cr",	nameof(Chromium),		6,	4, "d", byNumber, bySymbol, byName);
			Manganese		= DeclareChemicalElement(25,	"Mn",	nameof(Manganese),		7,	4, "d", byNumber, bySymbol, byName);
			Iron			= DeclareChemicalElement(26,	"Fe",	nameof(Iron),			8,	4, "d", byNumber, bySymbol, byName);
			Cobalt			= DeclareChemicalElement(27,	"Co",	nameof(Cobalt),			9,	4, "d", byNumber, bySymbol, byName);
			Nickel			= DeclareChemicalElement(28,	"Ni",	nameof(Nickel),			10,	4, "d", byNumber, bySymbol, byName);
			Copper			= DeclareChemicalElement(29,	"Cu",	nameof(Copper),			11,	4, "d", byNumber, bySymbol, byName);
			Zinc			= DeclareChemicalElement(30,	"Zn",	nameof(Zinc),			12,	4, "d", byNumber, bySymbol, byName);
			Gallium			= DeclareChemicalElement(31,	"Ga",	nameof(Gallium),		13,	4, "p", byNumber, bySymbol, byName);
			Germanium		= DeclareChemicalElement(32,	"Ge",	nameof(Germanium),		14,	4, "p", byNumber, bySymbol, byName);
			Arsenic			= DeclareChemicalElement(33,	"As",	nameof(Arsenic),		15,	4, "p", byNumber, bySymbol, byName);
			Selenium		= DeclareChemicalElement(34,	"Se",	nameof(Selenium),		16,	4, "p", byNumber, bySymbol, byName);
			Bromine			= DeclareChemicalElement(35,	"Br",	nameof(Bromine),		17,	4, "p", byNumber, bySymbol, byName);
			Krypton			= DeclareChemicalElement(36,	"Kr",	nameof(Krypton),		18,	4, "p", byNumber, bySymbol, byName);
			Rubidium		= DeclareChemicalElement(37,	"Rb",	nameof(Rubidium),		1,	5, "s", byNumber, bySymbol, byName);
			Strontium		= DeclareChemicalElement(38,	"Sr",	nameof(Strontium),		2,	5, "s", byNumber, bySymbol, byName);
			Yttrium			= DeclareChemicalElement(39,	"Y",	nameof(Yttrium),		3,	5, "d", byNumber, bySymbol, byName);
			Zirconium		= DeclareChemicalElement(40,	"Zr",	nameof(Zirconium),		4,	5, "d", byNumber, bySymbol, byName);
			Niobium			= DeclareChemicalElement(41,	"Nb",	nameof(Niobium),		5,	5, "d", byNumber, bySymbol, byName);
			Molybdenum		= DeclareChemicalElement(42,	"Mo",	nameof(Molybdenum),		6,	5, "d", byNumber, bySymbol, byName);
			Technetium		= DeclareChemicalElement(43,	"Tc",	nameof(Technetium),		7,	5, "d", byNumber, bySymbol, byName);
			Ruthenium		= DeclareChemicalElement(44,	"Ru",	nameof(Ruthenium),		8,	5, "d", byNumber, bySymbol, byName);
			Rhodium			= DeclareChemicalElement(45,	"Rh",	nameof(Rhodium),		9,	5, "d", byNumber, bySymbol, byName);
			Palladium		= DeclareChemicalElement(46,	"Pd",	nameof(Palladium),		10,	5, "d", byNumber, bySymbol, byName);
			Silver			= DeclareChemicalElement(47,	"Ag",	nameof(Silver),			11,	5, "d", byNumber, bySymbol, byName);
			Cadmium			= DeclareChemicalElement(48,	"Cd",	nameof(Cadmium),		12,	5, "d", byNumber, bySymbol, byName);
			Indium			= DeclareChemicalElement(49,	"In",	nameof(Indium),			13,	5, "p", byNumber, bySymbol, byName);
			Tin				= DeclareChemicalElement(50,	"Sn",	nameof(Tin),			14,	5, "p", byNumber, bySymbol, byName);
			Antimony		= DeclareChemicalElement(51,	"Sb",	nameof(Antimony),		15,	5, "p", byNumber, bySymbol, byName);
			Tellurium		= DeclareChemicalElement(52,	"Te",	nameof(Tellurium),		16,	5, "p", byNumber, bySymbol, byName);
			Iodine			= DeclareChemicalElement(53,	"I",	nameof(Iodine),			17,	5, "p", byNumber, bySymbol, byName);
			Xenon			= DeclareChemicalElement(54,	"Xe",	nameof(Xenon),			18,	5, "p", byNumber, bySymbol, byName);
			Caesium			= DeclareChemicalElement(55,	"Cs",	nameof(Caesium),		1,	6, "s", byNumber, bySymbol, byName);
			Barium			= DeclareChemicalElement(56,	"Ba",	nameof(Barium),			2,	6, "s", byNumber, bySymbol, byName);
			Lanthanum		= DeclareChemicalElement(57,	"La",	nameof(Lanthanum),		0,	6, "f-block", byNumber, bySymbol, byName);
			Cerium			= DeclareChemicalElement(58,	"Ce",	nameof(Cerium),			0,	6, "f-block", byNumber, bySymbol, byName);
			Praseodymium	= DeclareChemicalElement(59,	"Pr",	nameof(Praseodymium),	0,	6, "f-block", byNumber, bySymbol, byName);
			Neodymium		= DeclareChemicalElement(60,	"Nd",	nameof(Neodymium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Promethium		= DeclareChemicalElement(61,	"Pm",	nameof(Promethium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Samarium		= DeclareChemicalElement(62,	"Sm",	nameof(Samarium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Europium		= DeclareChemicalElement(63,	"Eu",	nameof(Europium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Gadolinium		= DeclareChemicalElement(64,	"Gd",	nameof(Gadolinium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Terbium			= DeclareChemicalElement(65,	"Tb",	nameof(Terbium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Dysprosium		= DeclareChemicalElement(66,	"Dy",	nameof(Dysprosium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Holmium			= DeclareChemicalElement(67,	"Ho",	nameof(Holmium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Erbium			= DeclareChemicalElement(68,	"Er",	nameof(Erbium),			0,	6, "f-block", byNumber, bySymbol, byName);
			Thulium			= DeclareChemicalElement(69,	"Tm",	nameof(Thulium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Ytterbium		= DeclareChemicalElement(70,	"Yb",	nameof(Ytterbium),		0,	6, "f-block", byNumber, bySymbol, byName);
			Lutetium		= DeclareChemicalElement(71,	"Lu",	nameof(Lutetium),		3,	6, "d", byNumber, bySymbol, byName);
			Hafnium			= DeclareChemicalElement(72,	"Hf",	nameof(Hafnium),		4,	6, "d", byNumber, bySymbol, byName);
			Tantalum		= DeclareChemicalElement(73,	"Ta",	nameof(Tantalum),		5,	6, "d", byNumber, bySymbol, byName);
			Tungsten		= DeclareChemicalElement(74,	"W",	nameof(Tungsten),		6,	6, "d", byNumber, bySymbol, byName);
			Rhenium			= DeclareChemicalElement(75,	"Re",	nameof(Rhenium),		7,	6, "d", byNumber, bySymbol, byName);
			Osmium			= DeclareChemicalElement(76,	"Os",	nameof(Osmium),			8,	6, "d", byNumber, bySymbol, byName);
			Iridium			= DeclareChemicalElement(77,	"Ir",	nameof(Iridium),		9,	6, "d", byNumber, bySymbol, byName);
			Platinum		= DeclareChemicalElement(78,	"Pt",	nameof(Platinum),		10,	6, "d", byNumber, bySymbol, byName);
			Gold			= DeclareChemicalElement(79,	"Au",	nameof(Gold),			11,	6, "d", byNumber, bySymbol, byName);
			Mercury			= DeclareChemicalElement(80,	"Hg",	nameof(Mercury),		12,	6, "d", byNumber, bySymbol, byName);
			Thallium		= DeclareChemicalElement(81,	"Tl",	nameof(Thallium),		13,	6, "p", byNumber, bySymbol, byName);
			Lead			= DeclareChemicalElement(82,	"Pb",	nameof(Lead),			14,	6, "p", byNumber, bySymbol, byName);
			Bismuth			= DeclareChemicalElement(83,	"Bi",	nameof(Bismuth),		15,	6, "p", byNumber, bySymbol, byName);
			Polonium		= DeclareChemicalElement(84,	"Po",	nameof(Polonium),		16,	6, "p", byNumber, bySymbol, byName);
			Astatine		= DeclareChemicalElement(85,	"At",	nameof(Astatine),		17,	6, "p", byNumber, bySymbol, byName);
			Radon			= DeclareChemicalElement(86,	"Rn",	nameof(Radon),			18,	6, "p", byNumber, bySymbol, byName);
			Francium		= DeclareChemicalElement(87,	"Fr",	nameof(Francium),		1,	7, "s", byNumber, bySymbol, byName);
			Radium			= DeclareChemicalElement(88,	"Ra",	nameof(Radium),			2,	7, "s", byNumber, bySymbol, byName);
			Actinium		= DeclareChemicalElement(89,	"Ac",	nameof(Actinium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Thorium			= DeclareChemicalElement(90,	"Th",	nameof(Thorium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Protactinium	= DeclareChemicalElement(91,	"Pa",	nameof(Protactinium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Uranium			= DeclareChemicalElement(92,	"U",	nameof(Uranium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Neptunium		= DeclareChemicalElement(93,	"Np",	nameof(Neptunium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Plutonium		= DeclareChemicalElement(94,	"Pu",	nameof(Plutonium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Americium		= DeclareChemicalElement(95,	"Am",	nameof(Americium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Curium			= DeclareChemicalElement(96,	"Cm",	nameof(Curium),			0,	7, "f-block", byNumber, bySymbol, byName);
			Berkelium		= DeclareChemicalElement(97,	"Bk",	nameof(Berkelium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Californium		= DeclareChemicalElement(98,	"Cf",	nameof(Californium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Einsteinium		= DeclareChemicalElement(99,	"Es",	nameof(Einsteinium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Fermium			= DeclareChemicalElement(100,	"Fm",	nameof(Fermium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Mendelevium		= DeclareChemicalElement(101,	"Md",	nameof(Mendelevium),	0,	7, "f-block", byNumber, bySymbol, byName);
			Nobelium		= DeclareChemicalElement(102,	"No",	nameof(Nobelium),		0,	7, "f-block", byNumber, bySymbol, byName);
			Lawrencium		= DeclareChemicalElement(103,	"Lr",	nameof(Lawrencium),		3,	7, "d", byNumber, bySymbol, byName);
			Rutherfordium	= DeclareChemicalElement(104,	"Rf",	nameof(Rutherfordium),	4,	7, "d", byNumber, bySymbol, byName);
			Dubnium			= DeclareChemicalElement(105,	"Db",	nameof(Dubnium),		5,	7, "d", byNumber, bySymbol, byName);
			Seaborgium		= DeclareChemicalElement(106,	"Sg",	nameof(Seaborgium),		6,	7, "d", byNumber, bySymbol, byName);
			Bohrium			= DeclareChemicalElement(107,	"Bh",	nameof(Bohrium),		7,	7, "d", byNumber, bySymbol, byName);
			Hassium			= DeclareChemicalElement(108,	"Hs",	nameof(Hassium),		8,	7, "d", byNumber, bySymbol, byName);
			Meitnerium		= DeclareChemicalElement(109,	"Mt",	nameof(Meitnerium),		9,	7, "d", byNumber, bySymbol, byName);
			Darmstadtium	= DeclareChemicalElement(110,	"Ds",	nameof(Darmstadtium),	10,	7, "d", byNumber, bySymbol, byName);
			Roentgenium		= DeclareChemicalElement(111,	"Rg",	nameof(Roentgenium),	11,	7, "d", byNumber, bySymbol, byName);
			Copernicium		= DeclareChemicalElement(112,	"Cn",	nameof(Copernicium),	12,	7, "d", byNumber, bySymbol, byName);
			Nihonium		= DeclareChemicalElement(113,	"Nh",	nameof(Nihonium),		13,	7, "p", byNumber, bySymbol, byName);
			Flerovium		= DeclareChemicalElement(114,	"Fl",	nameof(Flerovium),		14,	7, "p", byNumber, bySymbol, byName);
			Moscovium		= DeclareChemicalElement(115,	"Mc",	nameof(Moscovium),		15,	7, "p", byNumber, bySymbol, byName);
			Livermorium		= DeclareChemicalElement(116,	"Lv",	nameof(Livermorium),	16,	7, "p", byNumber, bySymbol, byName);
			Tennessine		= DeclareChemicalElement(117,	"Ts",	nameof(Tennessine),		17,	7, "p", byNumber, bySymbol, byName);
			Oganesson		= DeclareChemicalElement(118,	"Og",	nameof(Oganesson),		18,	7, "p", byNumber, bySymbol, byName);

			ByNumber = byNumber.ToArray();
			BySymbol = new ReadOnlyDictionary<String, IConcept>(bySymbol);
			ByName = new ReadOnlyDictionary<String, IConcept>(byName);

			#endregion
		}
	}
}