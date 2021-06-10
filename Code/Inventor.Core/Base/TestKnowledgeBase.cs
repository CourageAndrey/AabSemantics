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

		#region Subject Areas

		public Concept SubjectArea_Transport
		{ get; }

		#endregion

		#region Base Concepts

		public Concept Base_Vehicle
		{ get; }

		#endregion

		#region Signs

		public Concept Sign_MotorType
		{ get; }

		public Concept Sign_AreaType
		{ get; }

		#endregion

		#region Motor Types

		public Concept MotorType_Muscles
		{ get; }

		public Concept MotorType_Steam
		{ get; }

		public Concept MotorType_Combusion
		{ get; }

		public Concept MotorType_Jet
		{ get; }

		#endregion

		#region Area Types

		public Concept AreaType_Ground
		{ get; }

		public Concept AreaType_Water
		{ get; }

		public Concept AreaType_Air
		{ get; }

		#endregion

		#region Certain Transportation Devices

		public Concept Vehicle_Bicycle
		{ get; }

		public Concept Vehicle_Curragh
		{ get; }

		public Concept Vehicle_SteamLocomotive
		{ get; }

		public Concept Vehicle_Steamboat
		{ get; }

		public Concept Vehicle_Car
		{ get; }

		public Concept Vehicle_Motorcycle
		{ get; }

		public Concept Vehicle_Fighter
		{ get; }

		public Concept Vehicle_Airbus
		{ get; }

		public Concept Vehicle_JetFighter
		{ get; }

		#endregion

		#endregion

		public TestKnowledgeBase(ILanguage language)
		{
			#region Knowledge Base

			KnowledgeBase = new KnowledgeBase(language);
			((LocalizedStringVariable) KnowledgeBase.Name).SetLocale("ru-RU", "Тестовая база знаний");
			((LocalizedStringVariable) KnowledgeBase.Name).SetLocale("en-US", "Test knowledgebase");

			#endregion

			#region Subject Areas

			KnowledgeBase.Concepts.Add(SubjectArea_Transport = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Транспорт" },
				{ "en-US", "Transport" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Средства передвижения." },
				{ "en-US", "Vehicles." },
			})));

			#endregion

			#region Base Concepts

			KnowledgeBase.Concepts.Add(Base_Vehicle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Транспортное средство" },
				{ "en-US", "Vehicle" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устройство для перевозки людей и/или грузов." },
				{ "en-US", "System which is indended for transportation of humans and cargo." },
			})));

			#endregion

			#region Signs

			KnowledgeBase.Concepts.Add(Sign_MotorType = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Движитель" },
				{ "en-US", "Mover" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Система, обеспечивающая движение." },
				{ "en-US", "Initiator of movement." },
			})));

			KnowledgeBase.Concepts.Add(Sign_AreaType = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Среда передвижения" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Среда, для которой предназначено транспортное средство." },
			})));

			#endregion

			#region Motor types

			KnowledgeBase.Concepts.Add(MotorType_Muscles = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Мускульная сила" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использования для приведения в движение мускульной силы: собственной, других людей, животных." },
			})));

			KnowledgeBase.Concepts.Add(MotorType_Steam = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Паровая тяга" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использование для движения расширяющей силы нагретого пара." },
			})));

			KnowledgeBase.Concepts.Add(MotorType_Combusion = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Внутреннее сгорание" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использование для движения расширяющей силы топлива, сжигаемого в закрытых цилиндрах." },
			})));

			KnowledgeBase.Concepts.Add(MotorType_Jet = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Реактивная тяга" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Выталкивание вещества в обратном направлении, обычно сжигаемого топлива." },
			})));

			#endregion

			#region Area types

			KnowledgeBase.Concepts.Add(AreaType_Ground = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Земля" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Наземный транспорт." },
			})));

			KnowledgeBase.Concepts.Add(AreaType_Water = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Вода" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Плавучий транспорт." },
			})));

			KnowledgeBase.Concepts.Add(AreaType_Air = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Воздух" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Возможность полёта." },
			})));

			#endregion

			#region Certain Transportation Devices

			KnowledgeBase.Concepts.Add(Vehicle_Bicycle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Велосипед" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двухколёсный даритель радости." },
			})));

			KnowledgeBase.Concepts.Add(Vehicle_Curragh = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Курага" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Первая человеческая потуга создать лодку." },
			})));

			KnowledgeBase.Concepts.Add(Vehicle_SteamLocomotive = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Паровоз" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший тип локомотива." },
			})));

			KnowledgeBase.Concepts.Add(Vehicle_Steamboat = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Пароход" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший тип корабля." },
			})));

			KnowledgeBase.Concepts.Add(Vehicle_Car = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Автомобиль" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Четырёхколёсное механическое т/с." },
			})));

			KnowledgeBase.Concepts.Add(Vehicle_Motorcycle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Мотоцикл" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двухколёсное механическое т/с, возможно с коляской." },
			})));

			KnowledgeBase.Concepts.Add(Vehicle_Fighter = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Поршневой истребитель" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший самолёт для ведения воздушного боя." },
			})));

			KnowledgeBase.Concepts.Add(Vehicle_Airbus = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Аэробус" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Гражданский самолёт для перевозки пассажиров." },
			})));

			KnowledgeBase.Concepts.Add(Vehicle_JetFighter = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Реактивный истребитель" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Современный самолёт для ведения воздушного боя." },
			})));

			#endregion

			#region Concept Attributes

			Sign_MotorType.Attributes.Add(IsSignAttribute.Value);
			Sign_AreaType.Attributes.Add(IsSignAttribute.Value);

			MotorType_Muscles.Attributes.Add(IsValueAttribute.Value);
			MotorType_Steam.Attributes.Add(IsValueAttribute.Value);
			MotorType_Combusion.Attributes.Add(IsValueAttribute.Value);
			MotorType_Jet.Attributes.Add(IsValueAttribute.Value);
			AreaType_Ground.Attributes.Add(IsValueAttribute.Value);
			AreaType_Water.Attributes.Add(IsValueAttribute.Value);
			AreaType_Air.Attributes.Add(IsValueAttribute.Value);

			#endregion

			#region Statements

			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Base_Vehicle));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Sign_MotorType));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, MotorType_Muscles));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, MotorType_Steam));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, MotorType_Combusion));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, MotorType_Jet));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Sign_AreaType));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, AreaType_Ground));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, AreaType_Water));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, AreaType_Air));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Bicycle));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Curragh));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_SteamLocomotive));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Steamboat));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Car));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Motorcycle));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Fighter));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Airbus));
			KnowledgeBase.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_JetFighter));

			KnowledgeBase.Statements.Add(new HasSignStatement(Base_Vehicle, Sign_MotorType));
			KnowledgeBase.Statements.Add(new HasSignStatement(Base_Vehicle, Sign_AreaType));

			KnowledgeBase.Statements.Add(new IsStatement(Sign_MotorType, MotorType_Muscles));
			KnowledgeBase.Statements.Add(new IsStatement(Sign_MotorType, MotorType_Steam));
			KnowledgeBase.Statements.Add(new IsStatement(Sign_MotorType, MotorType_Combusion));
			KnowledgeBase.Statements.Add(new IsStatement(Sign_MotorType, MotorType_Jet));
			KnowledgeBase.Statements.Add(new IsStatement(Sign_AreaType, AreaType_Ground));
			KnowledgeBase.Statements.Add(new IsStatement(Sign_AreaType, AreaType_Water));
			KnowledgeBase.Statements.Add(new IsStatement(Sign_AreaType, AreaType_Air));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Bicycle));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Curragh));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_SteamLocomotive));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Steamboat));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Car));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Motorcycle));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Fighter));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Airbus));
			KnowledgeBase.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_JetFighter));

			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Bicycle, Sign_MotorType, MotorType_Muscles));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Curragh, Sign_MotorType, MotorType_Muscles));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_SteamLocomotive, Sign_MotorType, MotorType_Steam));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Steamboat, Sign_MotorType, MotorType_Steam));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Car, Sign_MotorType, MotorType_Combusion));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Motorcycle, Sign_MotorType, MotorType_Combusion));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Fighter, Sign_MotorType, MotorType_Combusion));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Airbus, Sign_MotorType, MotorType_Jet));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_JetFighter, Sign_MotorType, MotorType_Jet));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Bicycle, Sign_AreaType, AreaType_Ground));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Curragh, Sign_AreaType, AreaType_Water));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_SteamLocomotive, Sign_AreaType, AreaType_Ground));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Steamboat, Sign_AreaType, AreaType_Water));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Car, Sign_AreaType, AreaType_Ground));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Motorcycle, Sign_AreaType, AreaType_Ground));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Fighter, Sign_AreaType, AreaType_Air));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_Airbus, Sign_AreaType, AreaType_Air));
			KnowledgeBase.Statements.Add(new SignValueStatement(Vehicle_JetFighter, Sign_AreaType, AreaType_Air));

			KnowledgeBase.Statements.Add(new HasPartStatement(Base_Vehicle, Sign_MotorType));

			#endregion
		}
	}
}