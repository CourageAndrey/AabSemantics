using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Base
{
	public class TestKnowledgeBase
	{
		#region Properties

		public KnowledgeBase KnowledgeBase
		{ get; }

		#endregion

		public TestKnowledgeBase(ILanguage language)
		{
			// knowledge base
			KnowledgeBase = new KnowledgeBase(language);
			((LocalizedStringVariable) KnowledgeBase.Name).SetLocale("ru-RU", "Тестовая база знаний");
			((LocalizedStringVariable) KnowledgeBase.Name).SetLocale("en-US", "Test knowledgebase");

			// subject areas
			Concept transport;
			KnowledgeBase.Concepts.Add(transport = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Транспорт" },
				{ "en-US", "Transport" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Средства передвижения." },
				{ "en-US", "Vehicles." },
			})));

			// concepts
			Concept vehicle;
			KnowledgeBase.Concepts.Add(vehicle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Транспортное средство" },
				{ "en-US", "Vehicle" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устройство для перевозки людей и/или грузов." },
				{ "en-US", "System which is indended for transportation of humans and cargo." },
			})));

			Concept motorType, mtMucles, mtSteam, mtCombusion, mtJet;
			KnowledgeBase.Concepts.Add(motorType = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Движитель" },
				{ "en-US", "Mover" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Система, обеспечивающая движение." },
				{ "en-US", "Initiator of movement." },
			})));
			KnowledgeBase.Concepts.Add(mtMucles = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Мускульная сила" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использования для приведения в движение мускульной силы: собственной, других людей, животных." },
			})));
			KnowledgeBase.Concepts.Add(mtSteam = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Паровая тяга" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использование для движения расширяющей силы нагретого пара." },
			})));
			KnowledgeBase.Concepts.Add(mtCombusion = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Внутреннее сгорание" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использование для движения расширяющей силы топлива, сжигаемого в закрытых цилиндрах." },
			})));
			KnowledgeBase.Concepts.Add(mtJet = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Реактивная тяга" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Выталкивание вещества в обратном направлении, обычно сжигаемого топлива." },
			})));

			Concept areaType, atGround, atWater, atAir;
			KnowledgeBase.Concepts.Add(areaType = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Среда передвижения" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Среда, для которой предназначено транспортное средство." },
			})));
			KnowledgeBase.Concepts.Add(atGround = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Земля" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Наземный транспорт." },
			})));
			KnowledgeBase.Concepts.Add(atWater = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Вода" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Плавучий транспорт." },
			})));
			KnowledgeBase.Concepts.Add(atAir = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Воздух" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Возможность полёта." },
			})));

			Concept bicycle, curragh, steamLocomotive, steamboat, car, motorcycle, fighter, airbus, jetFighter;
			KnowledgeBase.Concepts.Add(bicycle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Велосипед" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двухколёсный даритель радости." },
			})));
			KnowledgeBase.Concepts.Add(curragh = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Курага" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Первая человеческая потуга создать лодку." },
			})));
			KnowledgeBase.Concepts.Add(steamLocomotive = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Паровоз" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший тип локомотива." },
			})));
			KnowledgeBase.Concepts.Add(steamboat = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Пароход" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший тип корабля." },
			})));
			KnowledgeBase.Concepts.Add(car = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Автомобиль" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Четырёхколёсное механическое т/с." },
			})));
			KnowledgeBase.Concepts.Add(motorcycle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Мотоцикл" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двухколёсное механическое т/с, возможно с коляской." },
			})));
			KnowledgeBase.Concepts.Add(fighter = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Поршневой истребитель" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший самолёт для ведения воздушного боя." },
			})));
			KnowledgeBase.Concepts.Add(airbus = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Аэробус" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Гражданский самолёт для перевозки пассажиров." },
			})));
			KnowledgeBase.Concepts.Add(jetFighter = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Реактивный истребитель" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Современный самолёт для ведения воздушного боя." },
			})));

			// sign attributes
			motorType.Attributes.Add(IsSignAttribute.Value);
			areaType.Attributes.Add(IsSignAttribute.Value);

			// value attributes
			mtMucles.Attributes.Add(IsValueAttribute.Value);
			mtSteam.Attributes.Add(IsValueAttribute.Value);
			mtCombusion.Attributes.Add(IsValueAttribute.Value);
			mtJet.Attributes.Add(IsValueAttribute.Value);
			atGround.Attributes.Add(IsValueAttribute.Value);
			atWater.Attributes.Add(IsValueAttribute.Value);
			atAir.Attributes.Add(IsValueAttribute.Value);

			// statements
			KnowledgeBase.Statements.Add(new GroupStatement(transport, vehicle));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, motorType));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, mtMucles));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, mtSteam));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, mtCombusion));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, mtJet));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, areaType));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, atGround));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, atWater));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, atAir));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, bicycle));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, curragh));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, steamLocomotive));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, steamboat));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, car));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, motorcycle));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, fighter));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, airbus));
			KnowledgeBase.Statements.Add(new GroupStatement(transport, jetFighter));
			KnowledgeBase.Statements.Add(new HasSignStatement(vehicle, motorType));
			KnowledgeBase.Statements.Add(new HasSignStatement(vehicle, areaType));
			KnowledgeBase.Statements.Add(new IsStatement(motorType, mtMucles));
			KnowledgeBase.Statements.Add(new IsStatement(motorType, mtSteam));
			KnowledgeBase.Statements.Add(new IsStatement(motorType, mtCombusion));
			KnowledgeBase.Statements.Add(new IsStatement(motorType, mtJet));
			KnowledgeBase.Statements.Add(new IsStatement(areaType, atGround));
			KnowledgeBase.Statements.Add(new IsStatement(areaType, atWater));
			KnowledgeBase.Statements.Add(new IsStatement(areaType, atAir));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, bicycle));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, curragh));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, steamLocomotive));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, steamboat));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, car));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, motorcycle));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, fighter));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, airbus));
			KnowledgeBase.Statements.Add(new IsStatement(vehicle, jetFighter));
			KnowledgeBase.Statements.Add(new SignValueStatement(bicycle, motorType, mtMucles));
			KnowledgeBase.Statements.Add(new SignValueStatement(curragh, motorType, mtMucles));
			KnowledgeBase.Statements.Add(new SignValueStatement(steamLocomotive, motorType, mtSteam));
			KnowledgeBase.Statements.Add(new SignValueStatement(steamboat, motorType, mtSteam));
			KnowledgeBase.Statements.Add(new SignValueStatement(car, motorType, mtCombusion));
			KnowledgeBase.Statements.Add(new SignValueStatement(motorcycle, motorType, mtCombusion));
			KnowledgeBase.Statements.Add(new SignValueStatement(fighter, motorType, mtCombusion));
			KnowledgeBase.Statements.Add(new SignValueStatement(airbus, motorType, mtJet));
			KnowledgeBase.Statements.Add(new SignValueStatement(jetFighter, motorType, mtJet));
			KnowledgeBase.Statements.Add(new SignValueStatement(bicycle, areaType, atGround));
			KnowledgeBase.Statements.Add(new SignValueStatement(curragh, areaType, atWater));
			KnowledgeBase.Statements.Add(new SignValueStatement(steamLocomotive, areaType, atGround));
			KnowledgeBase.Statements.Add(new SignValueStatement(steamboat, areaType, atWater));
			KnowledgeBase.Statements.Add(new SignValueStatement(car, areaType, atGround));
			KnowledgeBase.Statements.Add(new SignValueStatement(motorcycle, areaType, atGround));
			KnowledgeBase.Statements.Add(new SignValueStatement(fighter, areaType, atAir));
			KnowledgeBase.Statements.Add(new SignValueStatement(airbus, areaType, atAir));
			KnowledgeBase.Statements.Add(new SignValueStatement(jetFighter, areaType, atAir));
			KnowledgeBase.Statements.Add(new HasPartStatement(vehicle, motorType));
		}
	}
}