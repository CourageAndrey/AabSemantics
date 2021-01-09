using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core
{
	public sealed class KnowledgeBase : INamed, IChangeable
	{
		#region Properties

		public LocalizedString Name
		{ get { return _name; } }

		public ICollection<Concept> Concepts
		{ get { return _concepts; } }

		public ICollection<Statement> Statements
		{ get { return _statements; } }

		private readonly LocalizedStringVariable _name;
		private readonly EventCollection<Concept> _concepts;
		private readonly EventCollection<Statement> _statements;

		public event EventHandler<ItemEventArgs<Concept>> ConceptAdded;
		public event EventHandler<ItemEventArgs<Concept>> ConceptRemoved;
		public event EventHandler<ItemEventArgs<Statement>> StatementAdded;
		public event EventHandler<ItemEventArgs<Statement>> StatementRemoved;

		#region System

		public Concept True
		{ get; private set; }

		public Concept False
		{ get; private set; }

		#endregion

		#endregion

		public KnowledgeBase(bool initialize = true)
		{
			_name = new LocalizedStringVariable();

			_concepts = new EventCollection<Concept>();
			_concepts.ItemAdded += (sender, args) =>
			{
				var handler = Volatile.Read(ref ConceptAdded);
				if (handler != null)
				{
					handler(sender, args);
				}
			};
			_concepts.ItemRemoved += (sender, args) =>
			{
				var handler = Volatile.Read(ref ConceptRemoved);
				if (handler != null)
				{
					handler(sender, args);
				}
				foreach (var statement in _statements.Where(r => r.GetChildConcepts().Contains(args.Item)).ToList())
				{
					_statements.Remove(statement);
				}
			};

			_statements = new EventCollection<Statement>();
			_statements.ItemAdded += (sender, args) =>
			{
				var handler = Volatile.Read(ref StatementAdded);
				if (handler != null)
				{
					handler(sender, args);
				}
				foreach (var concept in args.Item.GetChildConcepts())
				{
					if (!_concepts.Contains(concept))
					{
						_concepts.Add(concept);
					}
				}
			};
			_statements.ItemRemoved += (sender, args) =>
			{
				var handler = Volatile.Read(ref StatementRemoved);
				if (handler != null)
				{
					handler(sender, args);
				}
			};

			if (initialize)
			{
				Initialize();
			}
			EventHandler<CancelableItemEventArgs<Concept>> systemConceptProtector = (sender, args) =>
			{
				if (args.Item.Type == ConceptType.System)
				{
					args.IsCanceled = true;
				}
			};
			_concepts.ItemAdding += systemConceptProtector;
			_concepts.ItemRemoving += systemConceptProtector;
		}

		public void Initialize()
		{
			_concepts.Add(True = new Concept(
				new LocalizedStringConstant(language => language.Misc.True),
				new LocalizedStringConstant(language => language.Misc.TrueHint)) { Type = ConceptType.System });
			_concepts.Add(False = new Concept(
				new LocalizedStringConstant(language => language.Misc.False),
				new LocalizedStringConstant(language => language.Misc.FalseHint)) { Type = ConceptType.System });
		}

		public override string ToString()
		{
			return string.Format("{0} : {1}", Strings.TostringKnowledgeBase, Name);
		}

		#region Serialization

		public static KnowledgeBase New(ILanguage language)
		{
			var result = new KnowledgeBase(true);
			((LocalizedStringVariable) result.Name).SetLocale(language.Culture, language.Misc.NewKbName);
			return result;
		}

		public static KnowledgeBase Load(string fileName)
		{
			throw new NotImplementedException();
		}

		public void Save(string fileName)
		{
			throw new NotImplementedException();
		}

		public event EventHandler Changed;

		private void raiseChanged()
		{
			var handler = Volatile.Read(ref Changed);
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		#endregion

		public static KnowledgeBase CreateTest()
		{
			// knowledge base
			var knowledgeBase = new KnowledgeBase();
			knowledgeBase._name.SetLocale("ru-RU", "Тестовая база знаний");
			knowledgeBase._name.SetLocale("en-US", "Test knowledgebase");

			// subject areas
			Concept transport;
			knowledgeBase.Concepts.Add(transport = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Транспорт" },
				{ "en-US", "Transport" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Средства передвижения." },
				{ "en-US", "Vehicles." },
			})));

			// concepts
			Concept vehicle;
			knowledgeBase.Concepts.Add(vehicle = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Транспортное средство" },
				{ "en-US", "Vehicle" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Устройство для перевозки людей и/или грузов." },
				{ "en-US", "System which is indended for transportation of humans and cargo." },
			})));

			Concept motorType, mtMucles, mtSteam, mtCombusion, mtJet;
			knowledgeBase.Concepts.Add(motorType = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Движитель" },
				{ "en-US", "Mover" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Система, обеспечивающая движение." },
				{ "en-US", "Initiator of movement." },
			})));
			knowledgeBase.Concepts.Add(mtMucles = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Мускульная сила" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Использования для приведения в движение мускульной силы: собственной, других людей, животных." },
			})));
			knowledgeBase.Concepts.Add(mtSteam = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Паровая тяга" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Использование для движения расширяющей силы нагретого пара." },
			})));
			knowledgeBase.Concepts.Add(mtCombusion = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Внутреннее сгорание" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Использование для движения расширяющей силы топлива, сжигаемого в закрытых цилиндрах." },
			})));
			knowledgeBase.Concepts.Add(mtJet = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Реактивная тяга" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Выталкивание вещества в обратном направлении, обычно сжигаемого топлива." },
			})));

			Concept areaType, atGround, atWater, atAir;
			knowledgeBase.Concepts.Add(areaType = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Среда передвижения" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Среда, для которой предназначено транспортное средство." },
			})));
			knowledgeBase.Concepts.Add(atGround = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Земля" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Наземный транспорт." },
			})));
			knowledgeBase.Concepts.Add(atWater = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Вода" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Плавучий транспорт." },
			})));
			knowledgeBase.Concepts.Add(atAir = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Воздух" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Возможность полёта." },
			})));

			Concept bicycle, curragh, steamLocomotive, steamboat, car, motorcycle, fighter, airbus, jetFighter;
			knowledgeBase.Concepts.Add(bicycle = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Велосипед" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Двухколёсный даритель радости." },
			})));
			knowledgeBase.Concepts.Add(curragh = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Курага" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Первая человеческая потуга создать лодку." },
			})));
			knowledgeBase.Concepts.Add(steamLocomotive = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Паровоз" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Устаревший тип локомотива." },
			})));
			knowledgeBase.Concepts.Add(steamboat = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Пароход" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Устаревший тип корабля." },
			})));
			knowledgeBase.Concepts.Add(car = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Автомобиль" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Четырёхколёсное механическое т/с." },
			})));
			knowledgeBase.Concepts.Add(motorcycle = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Мотоцикл" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Двухколёсное механическое т/с, возможно с коляской." },
			})));
			knowledgeBase.Concepts.Add(fighter = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Поршневой истребитель" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Устаревший самолёт для ведения воздушного боя." },
			})));
			knowledgeBase.Concepts.Add(airbus = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Аэробус" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Гражданский самолёт для перевозки пассажиров." },
			})));
			knowledgeBase.Concepts.Add(jetFighter = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Реактивный истребитель" },
			}), new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "ru-RU", "Современный самолёт для ведения воздушного боя." },
			})));

			// statements
			knowledgeBase.Statements.Add(new GroupStatement(transport, vehicle));
			knowledgeBase.Statements.Add(new GroupStatement(transport, motorType));
			knowledgeBase.Statements.Add(new GroupStatement(transport, mtMucles));
			knowledgeBase.Statements.Add(new GroupStatement(transport, mtSteam));
			knowledgeBase.Statements.Add(new GroupStatement(transport, mtCombusion));
			knowledgeBase.Statements.Add(new GroupStatement(transport, mtJet));
			knowledgeBase.Statements.Add(new GroupStatement(transport, areaType));
			knowledgeBase.Statements.Add(new GroupStatement(transport, atGround));
			knowledgeBase.Statements.Add(new GroupStatement(transport, atWater));
			knowledgeBase.Statements.Add(new GroupStatement(transport, atAir));
			knowledgeBase.Statements.Add(new GroupStatement(transport, bicycle));
			knowledgeBase.Statements.Add(new GroupStatement(transport, curragh));
			knowledgeBase.Statements.Add(new GroupStatement(transport, steamLocomotive));
			knowledgeBase.Statements.Add(new GroupStatement(transport, steamboat));
			knowledgeBase.Statements.Add(new GroupStatement(transport, car));
			knowledgeBase.Statements.Add(new GroupStatement(transport, motorcycle));
			knowledgeBase.Statements.Add(new GroupStatement(transport, fighter));
			knowledgeBase.Statements.Add(new GroupStatement(transport, airbus));
			knowledgeBase.Statements.Add(new GroupStatement(transport, jetFighter));
			knowledgeBase.Statements.Add(new HasSignStatement(vehicle, motorType));
			knowledgeBase.Statements.Add(new HasSignStatement(vehicle, areaType));
			knowledgeBase.Statements.Add(new IsStatement(motorType, mtMucles));
			knowledgeBase.Statements.Add(new IsStatement(motorType, mtSteam));
			knowledgeBase.Statements.Add(new IsStatement(motorType, mtCombusion));
			knowledgeBase.Statements.Add(new IsStatement(motorType, mtJet));
			knowledgeBase.Statements.Add(new IsStatement(areaType, atGround));
			knowledgeBase.Statements.Add(new IsStatement(areaType, atWater));
			knowledgeBase.Statements.Add(new IsStatement(areaType, atAir));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, bicycle));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, curragh));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, steamLocomotive));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, steamboat));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, car));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, motorcycle));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, fighter));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, airbus));
			knowledgeBase.Statements.Add(new IsStatement(vehicle, jetFighter));
			knowledgeBase.Statements.Add(new SignValueStatement(bicycle, motorType, mtMucles));
			knowledgeBase.Statements.Add(new SignValueStatement(curragh, motorType, mtMucles));
			knowledgeBase.Statements.Add(new SignValueStatement(steamLocomotive, motorType, mtSteam));
			knowledgeBase.Statements.Add(new SignValueStatement(steamboat, motorType, mtSteam));
			knowledgeBase.Statements.Add(new SignValueStatement(car, motorType, mtCombusion));
			knowledgeBase.Statements.Add(new SignValueStatement(motorcycle, motorType, mtCombusion));
			knowledgeBase.Statements.Add(new SignValueStatement(fighter, motorType, mtCombusion));
			knowledgeBase.Statements.Add(new SignValueStatement(airbus, motorType, mtJet));
			knowledgeBase.Statements.Add(new SignValueStatement(jetFighter, motorType, mtJet));
			knowledgeBase.Statements.Add(new SignValueStatement(bicycle, areaType, atGround));
			knowledgeBase.Statements.Add(new SignValueStatement(curragh, areaType, atWater));
			knowledgeBase.Statements.Add(new SignValueStatement(steamLocomotive, areaType, atGround));
			knowledgeBase.Statements.Add(new SignValueStatement(steamboat, areaType, atWater));
			knowledgeBase.Statements.Add(new SignValueStatement(car, areaType, atGround));
			knowledgeBase.Statements.Add(new SignValueStatement(motorcycle, areaType, atGround));
			knowledgeBase.Statements.Add(new SignValueStatement(fighter, areaType, atAir));
			knowledgeBase.Statements.Add(new SignValueStatement(airbus, areaType, atAir));
			knowledgeBase.Statements.Add(new SignValueStatement(jetFighter, areaType, atAir));
			knowledgeBase.Statements.Add(new ConsistsOfStatement(vehicle, motorType));

			return knowledgeBase;
		}

		public FormattedText DescribeRules(ILanguage language)
		{
			var result = new FormattedText();
			foreach (var statement in Statements)
			{
				result.Add(statement.DescribeTrue(language));
			}
			return result;
		}

		public FormattedText CheckConsistensy(ILanguage language)
		{
			var result = new FormattedText();

			// 1. check all duplicates
			foreach (var statement in _statements)
			{
				if (!statement.CheckUnique(_statements))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorDuplicate,
						new Dictionary<string, INamed> { { "#STATEMENT#", statement } });
				}
			}

			// 2. check cyclic parents
			var clasifications = _statements.OfType<IsStatement>().ToList();
			foreach (var clasification in clasifications)
			{
				if (!clasification.CheckCyclic(clasifications))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorCyclic,
						new Dictionary<string, INamed> { { "#STATEMENT#", clasification } });
				}
			}

			// 4. check multi values
			var signValues = _statements.OfType<SignValueStatement>().ToList();
			foreach (var concept in _concepts)
			{
				var parents = clasifications.GetParentsPlainList(concept);
				foreach (var sign in HasSignStatement.GetSigns(_statements, concept, true))
				{
					if (signValues.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
					    parents.Select(p => SignValueStatement.GetSignValue(_statements, p, sign.Sign)).Count(r => r != null) > 1)
					{
						result.Add(
							() => language.Misc.ConsistencyErrorMultipleSignValue,
							new Dictionary<string, INamed>
							{
								{ "#CONCEPT#", concept },
								{ "#SIGN#", sign.Sign },
							});
					}
				}
			}

			// 5. check values without sign
			foreach (var signValue in signValues)
			{
				if (!signValue.CheckHasSign(_statements))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorSignWithoutValue,
						new Dictionary<string, INamed> { { "#STATEMENT#", signValue } });
				}
			}

			// 6. check sign duplications
			var hasSigns = _statements.OfType<HasSignStatement>().ToList();
			foreach (var hasSign in hasSigns)
			{
				if (!hasSign.CheckSignDuplication(hasSigns, clasifications))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorMultipleSign,
						new Dictionary<string, INamed> { { "#STATEMENT#", hasSign } });
				}
			}

			if (result.LinesCount == 0)
			{
				result.Add(() => language.Misc.CheckOk, new Dictionary<string, INamed>());
			}
			return result;
		}
	}
}
