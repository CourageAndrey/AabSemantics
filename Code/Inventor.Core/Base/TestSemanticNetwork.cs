using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Base
{
	public class TestSemanticNetwork
	{
		#region Properties

		public SemanticNetwork SemanticNetwork
		{ get; }

		#region Subject Areas

		public Concept SubjectArea_Transport
		{ get; }

		public Concept SubjectArea_Numbers
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

		#region Car parts

		public IConcept Part_Engine
		{ get; }

		public IConcept Part_Wheels
		{ get; }

		public IConcept Part_Body
		{ get; }

		#endregion

		#region Comparable Values

		public Concept Number0
		{ get; }

		public Concept NumberZero
		{ get; }

		public Concept NumberNotZero
		{ get; }

		public Concept Number1
		{ get; }

		public Concept Number1or2
		{ get; }

		public Concept Number2
		{ get; }

		public Concept Number2or3
		{ get; }

		public Concept Number3
		{ get; }

		public Concept Number3or4
		{ get; }

		public Concept Number4
		{ get; }

		#endregion

		#endregion

		public TestSemanticNetwork(ILanguage language)
		{
			#region Semantic network

			SemanticNetwork = new SemanticNetwork(language);
			((LocalizedStringVariable) SemanticNetwork.Name).SetLocale("ru-RU", "Тестовая база знаний");
			((LocalizedStringVariable) SemanticNetwork.Name).SetLocale("en-US", "Test knowledgebase");

			#endregion

			#region Subject Areas

			SemanticNetwork.Concepts.Add(SubjectArea_Transport = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Транспорт" },
				{ "en-US", "Transport" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Средства передвижения." },
				{ "en-US", "Vehicles." },
			})));

			SemanticNetwork.Concepts.Add(SubjectArea_Numbers = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Числа" },
				{ "en-US", "Numbers" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Числа." },
				{ "en-US", "Numbers." },
			})));

			#endregion

			#region Base Concepts

			SemanticNetwork.Concepts.Add(Base_Vehicle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
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

			SemanticNetwork.Concepts.Add(Sign_MotorType = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Движитель" },
				{ "en-US", "Mover" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Система, обеспечивающая движение." },
				{ "en-US", "Initiator of movement." },
			})));

			SemanticNetwork.Concepts.Add(Sign_AreaType = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Среда передвижения" },
				{ "en-US", "Movement area" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Среда, для которой предназначено транспортное средство." },
				{ "en-US", "Place, where vehicles can move." },
			})));

			#endregion

			#region Motor types

			SemanticNetwork.Concepts.Add(MotorType_Muscles = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Мускульная сила" },
				{ "en-US", "Muscles" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использования для приведения в движение мускульной силы: собственной, других людей, животных." },
				{ "en-US", "To use own muscles in order to move." },
			})));

			SemanticNetwork.Concepts.Add(MotorType_Steam = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Паровая тяга" },
				{ "en-US", "Steam engine" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использование для движения расширяющей силы нагретого пара." },
				{ "en-US", "To use steam engine to move." },
			})));

			SemanticNetwork.Concepts.Add(MotorType_Combusion = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Внутреннее сгорание" },
				{ "en-US", "Combustion engine" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Использование для движения расширяющей силы топлива, сжигаемого в закрытых цилиндрах." },
				{ "en-US", "To use combustion engine to move." },
			})));

			SemanticNetwork.Concepts.Add(MotorType_Jet = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Реактивная тяга" },
				{ "en-US", "Jet engine" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Выталкивание вещества в обратном направлении, обычно сжигаемого топлива." },
				{ "en-US", "To use jet engine to move." },
			})));

			#endregion

			#region Area types

			SemanticNetwork.Concepts.Add(AreaType_Ground = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Земля" },
				{ "en-US", "Ground" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Наземный транспорт." },
				{ "en-US", "Plain ground." },
			})));

			SemanticNetwork.Concepts.Add(AreaType_Water = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Вода" },
				{ "en-US", "Water" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Плавучий транспорт." },
				{ "en-US", "Water surface." },
			})));

			SemanticNetwork.Concepts.Add(AreaType_Air = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Воздух" },
				{ "en-US", "Air" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Возможность полёта." },
				{ "en-US", "Fly in air." },
			})));

			#endregion

			#region Certain Transportation Devices

			SemanticNetwork.Concepts.Add(Vehicle_Bicycle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Велосипед" },
				{ "en-US", "Bicycle" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двухколёсный даритель радости." },
				{ "en-US", "Two wheels of fun." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Curragh = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Курага" },
				{ "en-US", "Curragh" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Первая человеческая потуга создать лодку." },
				{ "en-US", "It is not a bot itself yet." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_SteamLocomotive = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Паровоз" },
				{ "en-US", "Steam locomotive" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший тип локомотива." },
				{ "en-US", "Obsolete train." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Steamboat = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Пароход" },
				{ "en-US", "Steamboat" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший тип корабля." },
				{ "en-US", "Obsolete boat type." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Car = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Автомобиль" },
				{ "en-US", "Car" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Четырёхколёсное механическое т/с." },
				{ "en-US", "4-wheels standard vehicle." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Motorcycle = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Мотоцикл" },
				{ "en-US", "Motorcycle" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двухколёсное механическое т/с, возможно с коляской." },
				{ "en-US", "Half of a car." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Fighter = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Поршневой истребитель" },
				{ "en-US", "Fighter" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Устаревший самолёт для ведения воздушного боя." },
				{ "en-US", "Obsolete aircraft." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_Airbus = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Аэробус" },
				{ "en-US", "Airbus" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Гражданский самолёт для перевозки пассажиров." },
				{ "en-US", "Large civil airplane." },
			})));

			SemanticNetwork.Concepts.Add(Vehicle_JetFighter = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Реактивный истребитель" },
				{ "en-US", "Jet fighter" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Современный самолёт для ведения воздушного боя." },
				{ "en-US", "Modern aircraft." },
			})));

			#endregion

			#region Car parts

			SemanticNetwork.Concepts.Add(Part_Engine = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двигатель" },
				{ "en-US", "Engine" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Двигатель." },
				{ "en-US", "Engine." },
			})));

			SemanticNetwork.Concepts.Add(Part_Wheels = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Колёса" },
				{ "en-US", "Wheels" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Колёса." },
				{ "en-US", "Wheels." },
			})));

			SemanticNetwork.Concepts.Add(Part_Body = new Concept(new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Кузов" },
				{ "en-US", "Car body" },
			}), new LocalizedStringVariable(new Dictionary<String, String>
			{
				{ "ru-RU", "Кузов." },
				{ "en-US", "Car body." },
			})));

			#endregion

			#region Comparable Values

			Func<String, LocalizedStringVariable> getString = text => new LocalizedStringVariable(
				new Dictionary<String, String>
				{
					{ "ru-RU", text },
					{ "en-US", text },
				});
			Func<Int32, LocalizedStringVariable> getStringByNumber = number => getString(number.ToString());

			SemanticNetwork.Concepts.Add(Number0 = new Concept(getStringByNumber(0), getStringByNumber(0)));

			SemanticNetwork.Concepts.Add(NumberZero = new Concept(getString("_0_"), getString("_0_")));

			SemanticNetwork.Concepts.Add(NumberNotZero = new Concept(getString("!0"), getString("!0")));

			SemanticNetwork.Concepts.Add(Number1 = new Concept(getStringByNumber(1), getStringByNumber(1)));

			SemanticNetwork.Concepts.Add(Number1or2 = new Concept(getString("1 || 2"), getString("1 || 2")));

			SemanticNetwork.Concepts.Add(Number2 = new Concept(getStringByNumber(2), getStringByNumber(2)));

			SemanticNetwork.Concepts.Add(Number2or3 = new Concept(getString("2 || 3"), getString("2 || 3")));

			SemanticNetwork.Concepts.Add(Number3 = new Concept(getStringByNumber(3), getStringByNumber(3)));

			SemanticNetwork.Concepts.Add(Number3or4 = new Concept(getString("3 || 4"), getString("3 || 4")));

			SemanticNetwork.Concepts.Add(Number4 = new Concept(getStringByNumber(4), getStringByNumber(4)));

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

			Number0.Attributes.Add(IsValueAttribute.Value);
			NumberZero.Attributes.Add(IsValueAttribute.Value);
			NumberNotZero.Attributes.Add(IsValueAttribute.Value);
			Number1.Attributes.Add(IsValueAttribute.Value);
			Number1or2.Attributes.Add(IsValueAttribute.Value);
			Number2.Attributes.Add(IsValueAttribute.Value);
			Number2or3.Attributes.Add(IsValueAttribute.Value);
			Number3.Attributes.Add(IsValueAttribute.Value);
			Number3or4.Attributes.Add(IsValueAttribute.Value);
			Number4.Attributes.Add(IsValueAttribute.Value);

			#endregion

			#region Statements

			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Base_Vehicle));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Sign_MotorType));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, MotorType_Muscles));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, MotorType_Steam));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, MotorType_Combusion));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, MotorType_Jet));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Sign_AreaType));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, AreaType_Ground));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, AreaType_Water));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, AreaType_Air));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Bicycle));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Curragh));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_SteamLocomotive));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Steamboat));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Car));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Motorcycle));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Fighter));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_Airbus));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Vehicle_JetFighter));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Part_Body));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Part_Engine));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Transport, Part_Wheels));

			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, Number0));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, NumberZero));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, NumberNotZero));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, Number1));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, Number1or2));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, Number2));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, Number2or3));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, Number3));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, Number3or4));
			SemanticNetwork.Statements.Add(new GroupStatement(SubjectArea_Numbers, Number4));

			SemanticNetwork.Statements.Add(new HasSignStatement(Base_Vehicle, Sign_MotorType));
			SemanticNetwork.Statements.Add(new HasSignStatement(Base_Vehicle, Sign_AreaType));

			SemanticNetwork.Statements.Add(new IsStatement(Sign_MotorType, MotorType_Muscles));
			SemanticNetwork.Statements.Add(new IsStatement(Sign_MotorType, MotorType_Steam));
			SemanticNetwork.Statements.Add(new IsStatement(Sign_MotorType, MotorType_Combusion));
			SemanticNetwork.Statements.Add(new IsStatement(Sign_MotorType, MotorType_Jet));
			SemanticNetwork.Statements.Add(new IsStatement(Sign_AreaType, AreaType_Ground));
			SemanticNetwork.Statements.Add(new IsStatement(Sign_AreaType, AreaType_Water));
			SemanticNetwork.Statements.Add(new IsStatement(Sign_AreaType, AreaType_Air));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Bicycle));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Curragh));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_SteamLocomotive));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Steamboat));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Car));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Motorcycle));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Fighter));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_Airbus));
			SemanticNetwork.Statements.Add(new IsStatement(Base_Vehicle, Vehicle_JetFighter));

			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Bicycle, Sign_MotorType, MotorType_Muscles));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Curragh, Sign_MotorType, MotorType_Muscles));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_SteamLocomotive, Sign_MotorType, MotorType_Steam));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Steamboat, Sign_MotorType, MotorType_Steam));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Car, Sign_MotorType, MotorType_Combusion));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Motorcycle, Sign_MotorType, MotorType_Combusion));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Fighter, Sign_MotorType, MotorType_Combusion));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Airbus, Sign_MotorType, MotorType_Jet));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_JetFighter, Sign_MotorType, MotorType_Jet));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Bicycle, Sign_AreaType, AreaType_Ground));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Curragh, Sign_AreaType, AreaType_Water));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_SteamLocomotive, Sign_AreaType, AreaType_Ground));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Steamboat, Sign_AreaType, AreaType_Water));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Car, Sign_AreaType, AreaType_Ground));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Motorcycle, Sign_AreaType, AreaType_Ground));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Fighter, Sign_AreaType, AreaType_Air));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_Airbus, Sign_AreaType, AreaType_Air));
			SemanticNetwork.Statements.Add(new SignValueStatement(Vehicle_JetFighter, Sign_AreaType, AreaType_Air));

			SemanticNetwork.Statements.Add(new HasPartStatement(Vehicle_Car, Part_Body));
			SemanticNetwork.Statements.Add(new HasPartStatement(Vehicle_Car, Part_Engine));
			SemanticNetwork.Statements.Add(new HasPartStatement(Vehicle_Car, Part_Wheels));

			SemanticNetwork.Statements.Add(new ComparisonStatement(Number0, NumberZero, ComparisonSigns.IsEqualTo));
			SemanticNetwork.Statements.Add(new ComparisonStatement(NumberNotZero, NumberZero, ComparisonSigns.IsNotEqualTo));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number0, Number1, ComparisonSigns.IsLessThan));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number1, Number2, ComparisonSigns.IsLessThan));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number3, Number2, ComparisonSigns.IsGreaterThan));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number4, Number3, ComparisonSigns.IsGreaterThan));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number1, Number1or2, ComparisonSigns.IsLessThanOrEqualTo));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number1or2, Number2, ComparisonSigns.IsLessThanOrEqualTo));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number2, Number2or3, ComparisonSigns.IsLessThanOrEqualTo));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number3, Number2or3, ComparisonSigns.IsGreaterThanOrEqualTo));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number4, Number3or4, ComparisonSigns.IsGreaterThanOrEqualTo));
			SemanticNetwork.Statements.Add(new ComparisonStatement(Number3or4, Number3, ComparisonSigns.IsGreaterThanOrEqualTo));

			#endregion
		}
	}
}