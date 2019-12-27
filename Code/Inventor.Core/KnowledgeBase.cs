using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Win32;

using Inventor.Core.Localization;
using Inventor.Core.Propositions;

using Sef.Common;
using Sef.Program;

namespace Inventor.Core
{
    public sealed class KnowledgeBase : INamed, IChangeable
    {
        #region Properties

        public LocalizedString Name
        { get { return name; } }

        public IList<Concept> Concepts
        { get { return concepts; } }

        public IList<Proposition> Propositions
        { get { return propositions; } }

        private readonly LocalizedStringVariable name;
        private readonly EventList<Concept> concepts;
        private readonly EventList<Proposition> propositions;

        public event EventList<Concept>.ItemDelegate ConceptAdded;
        public event EventList<Concept>.ItemDelegate ConceptRemoved;
        public event EventList<Proposition>.ItemDelegate PropositionAdded;
        public event EventList<Proposition>.ItemDelegate PropositionRemoved;

        #region System

        public Concept True
        { get; private set; }

        public Concept False
        { get; private set; }

        #endregion

        #endregion

        public KnowledgeBase(bool initialize = true)
        {
            name = new LocalizedStringVariable();

            concepts = new EventList<Concept>();
            concepts.Added += (list, concept) =>
            {
                var handler = Volatile.Read(ref ConceptAdded);
                if (handler != null)
                {
                    handler(list, concept);
                }
            };
            concepts.Removed += (list, concept) =>
            {
                var handler = Volatile.Read(ref ConceptRemoved);
                if (handler != null)
                {
                    handler(list, concept);
                }
                foreach (var proposition in propositions.Where(r => r.ChildConcepts.Contains(concept)).ToList())
                {
                    propositions.Remove(proposition);
                }
            };

            propositions = new EventList<Proposition>();
            propositions.Added += (list, proposition) =>
            {
                var handler = Volatile.Read(ref PropositionAdded);
                if (handler != null)
                {
                    handler(list, proposition);
                }
                foreach (var concept in proposition.ChildConcepts)
                {
                    if (!concepts.Contains(concept))
                    {
                        concepts.Add(concept);
                    }
                }
            };
            propositions.Removed += (list, proposition) =>
            {
                var handler = Volatile.Read(ref PropositionRemoved);
                if (handler != null)
                {
                    handler(list, proposition);
                }
            };

            if (initialize)
            {
                Initialize();
            }
            EventList<Concept>.ItemQueryDelegate systemConceptProtector = (IList<Concept> list, Concept item, ref bool allowed) =>
            {
                if (item.Type == ConceptType.System)
                {
                    allowed = false;
                }
            };
            concepts.Adding += systemConceptProtector;
            concepts.Removing += systemConceptProtector;
        }

        public void Initialize()
        {
            concepts.Add(True = new Concept(
                new LocalizedStringConstant(() => LanguageEx.CurrentEx.Misc.True),
                new LocalizedStringConstant(() => LanguageEx.CurrentEx.Misc.TrueHint)) { Type = ConceptType.System });
            concepts.Add(False = new Concept(
                new LocalizedStringConstant(() => LanguageEx.CurrentEx.Misc.False),
                new LocalizedStringConstant(() => LanguageEx.CurrentEx.Misc.FalseHint)) { Type = ConceptType.System });
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Strings.TostringKnowledgeBase, Name.Value);
        }

        #region Serialization

        public static KnowledgeBase New()
        {
            var result = new KnowledgeBase(true);
            ((LocalizedStringVariable) result.Name).SetLocale(Sef.Localization.Language.Current.Culture, LanguageEx.CurrentEx.Misc.NewKbName);
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

        internal static OpenFileDialog CreateOpenFileDialog()
        {
            var language = LanguageEx.CurrentEx.Misc;
            return new OpenFileDialog
            {
                DefaultExt = ".xml",
                Filter = language.DialogKbFileFilter,
                RestoreDirectory = true,
                Title = language.DialogKbOpenTitle,
            };
        }

        internal static SaveFileDialog CreateSaveFileDialog()
        {
            var language = LanguageEx.CurrentEx.Misc;
            return new SaveFileDialog
            {
                DefaultExt = ".xml",
                Filter = language.DialogKbFileFilter,
                RestoreDirectory = true,
                Title = language.DialogKbSaveTitle,
            };
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
            knowledgeBase.name.SetLocale("ru-RU", "Тестовая база знаний");
            knowledgeBase.name.SetLocale("en-US", "Test knowledgebase");

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

            Concept bicycle, courag, steamLocomotive, steamboat, car, motorcycle, fighter, airbus, jetFighter;
            knowledgeBase.Concepts.Add(bicycle = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
            {
                { "ru-RU", "Велосипед" },
            }), new LocalizedStringVariable(new Dictionary<string, string>
            {
                { "ru-RU", "Двухколёсный даритель радости." },
            })));
            knowledgeBase.Concepts.Add(courag = new Concept(new LocalizedStringVariable(new Dictionary<string, string>
            {
                { "ru-RU", "Курагг" },
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

            // propositions
            knowledgeBase.Propositions.Add(new SubjectArea(transport, vehicle));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, motorType));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, mtMucles));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, mtSteam));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, mtCombusion));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, mtJet));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, areaType));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, atGround));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, atWater));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, atAir));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, bicycle));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, courag));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, steamLocomotive));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, steamboat));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, car));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, motorcycle));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, fighter));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, airbus));
            knowledgeBase.Propositions.Add(new SubjectArea(transport, jetFighter));
            knowledgeBase.Propositions.Add(new HasSign(vehicle, motorType));
            knowledgeBase.Propositions.Add(new HasSign(vehicle, areaType));
            knowledgeBase.Propositions.Add(new Clasification(motorType, mtMucles));
            knowledgeBase.Propositions.Add(new Clasification(motorType, mtSteam));
            knowledgeBase.Propositions.Add(new Clasification(motorType, mtCombusion));
            knowledgeBase.Propositions.Add(new Clasification(motorType, mtJet));
            knowledgeBase.Propositions.Add(new Clasification(areaType, atGround));
            knowledgeBase.Propositions.Add(new Clasification(areaType, atWater));
            knowledgeBase.Propositions.Add(new Clasification(areaType, atAir));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, bicycle));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, courag));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, steamLocomotive));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, steamboat));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, car));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, motorcycle));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, fighter));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, airbus));
            knowledgeBase.Propositions.Add(new Clasification(vehicle, jetFighter));
            knowledgeBase.Propositions.Add(new SignValue(bicycle, motorType, mtMucles));
            knowledgeBase.Propositions.Add(new SignValue(courag, motorType, mtMucles));
            knowledgeBase.Propositions.Add(new SignValue(steamLocomotive, motorType, mtSteam));
            knowledgeBase.Propositions.Add(new SignValue(steamboat, motorType, mtSteam));
            knowledgeBase.Propositions.Add(new SignValue(car, motorType, mtCombusion));
            knowledgeBase.Propositions.Add(new SignValue(motorcycle, motorType, mtCombusion));
            knowledgeBase.Propositions.Add(new SignValue(fighter, motorType, mtCombusion));
            knowledgeBase.Propositions.Add(new SignValue(airbus, motorType, mtJet));
            knowledgeBase.Propositions.Add(new SignValue(jetFighter, motorType, mtJet));
            knowledgeBase.Propositions.Add(new SignValue(bicycle, areaType, atGround));
            knowledgeBase.Propositions.Add(new SignValue(courag, areaType, atWater));
            knowledgeBase.Propositions.Add(new SignValue(steamLocomotive, areaType, atGround));
            knowledgeBase.Propositions.Add(new SignValue(steamboat, areaType, atWater));
            knowledgeBase.Propositions.Add(new SignValue(car, areaType, atGround));
            knowledgeBase.Propositions.Add(new SignValue(motorcycle, areaType, atGround));
            knowledgeBase.Propositions.Add(new SignValue(fighter, areaType, atAir));
            knowledgeBase.Propositions.Add(new SignValue(airbus, areaType, atAir));
            knowledgeBase.Propositions.Add(new SignValue(jetFighter, areaType, atAir));
            knowledgeBase.Propositions.Add(new Composition(vehicle, motorType));

            return knowledgeBase;
        }

        public FormattedText DescribeRules()
        {
            var result = new FormattedText();
            foreach (var proposition in Propositions)
            {
                result.Add(proposition.DescribeTrue());
            }
            return result;
        }

        public FormattedText CheckConsistensy()
        {
            var result = new FormattedText();
            var language = LanguageEx.CurrentEx.Misc;

            // 1. check all duplicates
            foreach (var proposition in propositions)
            {
                if (!proposition.CheckUnique(propositions))
                {
                    result.Add(
                        () => language.ConsistencyErrorDuplicate,
                        new Dictionary<string, INamed>{ { "#PROPOSITION#", proposition } });
                }
            }

            // 2. check cyclic parents
            var clasifications = propositions.OfType<Clasification>().ToList();
            foreach (var clasification in clasifications)
            {
                if (!clasification.CheckCyclic(clasifications))
                {
                    result.Add(
                        () => language.ConsistencyErrorCyclic,
                        new Dictionary<string, INamed>{ { "#PROPOSITION#", clasification } });
                }
            }

            // 4. check multi values
            var signValues = propositions.OfType<SignValue>().ToList();
            foreach (var concept in concepts)
            {
                var parents = Clasification.GetParentsPlainList(clasifications, concept);
                foreach (var sign in HasSign.GetSigns(propositions, concept, true))
                {
                    if (signValues.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
                        parents.Select(p => SignValue.GetSignValue(propositions, p, sign.Sign)).Count(r => r != null) > 1)
                    {
                        result.Add(
                            () => language.ConsistencyErrorMultipleSignValue,
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
                if (!signValue.CheckHasSign(propositions))
                {
                    result.Add(
                        () => language.ConsistencyErrorSignWithoutValue,
                        new Dictionary<string, INamed> { { "#PROPOSITION#", signValue } });
                }
            }

            // 6. check sign duplications
            var hasSigns = propositions.OfType<HasSign>().ToList();
            foreach (var hasSign in hasSigns)
            {
                if (!hasSign.CheckSignDuplication(hasSigns, clasifications))
                {
                    result.Add(
                        () => language.ConsistencyErrorMultipleSign,
                        new Dictionary<string, INamed> { { "#PROPOSITION#", hasSign } });
                }
            }

            if (result.LinesCount == 0)
            {
                result.Add(() => LanguageEx.CurrentEx.Misc.CheckOk, new Dictionary<string, INamed>());
            }
            return result;
        }
    }
}
