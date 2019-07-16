using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ДТП__Курсовая_работа_
{
    class Driver
    {
        public int dKey { get;set;}
        public string Name { get;  set;  }
        public uint Experience { get; set; }
        public string DriverLicense { get; set; }
        public Driver() {}

        public Driver(int dk, string n, uint exp, string ln) {
            dKey = dk;
            Name = n;
            Experience = exp;
            DriverLicense = ln;
        }
    }
    class Car
    {
        public int cKey { get; set; }
        public string Firm { get; set; }
        public string Brend { get; set; }
        public string BodyType { get; set; }
        public string StateCarNumber { get; set; }

        public Car() { }
        public Car(int c_key, string firm, string brend, string body_type, string state_car_number)
        {
            cKey = c_key;
            Firm = firm;
            Brend = brend;
            BodyType = body_type;
            StateCarNumber = state_car_number;
        }
    }
    class RoadAccident
    {
        enum TypeAccident { Collision, Overturn, H_StandVehicle, H_Obstacle, H_Pedestrian, H_Cyclist, CollisionHorseVehicles, PassengerFell, Other };

        static Dictionary<string, TypeAccident> TypesOfAccidents = new Dictionary<string, TypeAccident>
        {
            {  "столкновение",  TypeAccident.Collision},
            {  "опрокидывание",  TypeAccident.Overturn},
            {  "наезд на стоящее транспортное средство",  TypeAccident.H_StandVehicle},
            {  "наезд на препятствие",  TypeAccident.H_Obstacle },
            {  "наезд на пешехода",  TypeAccident.H_Pedestrian},
            {  "наезд на велосипедиста", TypeAccident.H_Cyclist},
            {  "наезд на гужевой транспорт", TypeAccident.CollisionHorseVehicles},
            {  "пассажир упал ", TypeAccident.PassengerFell},
        };

        private TypeAccident type_of_accident;
        private uint date;
        public int rKey { get; set; }//связь ДТП с водителями и машинами, которые в нем участвовали
        public int tpd_Key { get; set; } //связь ДТП с отделом ГИБДД, в котором оно рассматривается
        public uint Date
        {
            get { return date; }
            set
            {
                if (0 < value && value <= 31) date = value;
                else throw new Exception();
            }
        }
        public int RoadAccidentNumber { get; set; }
        public string Place { get; set; }
        public uint CountOfVicttim {get ;  set ; }
        public string TypeOfAccident
        {
            get
            {
                return GetTypeOfAccident();
            }
            set
            {
                SetTypeOfAccident(value);
            }
        }
        public string ReasonOfAccident { get; set; }

        public RoadAccident() { }
        public RoadAccident(int r_key, int tpd_key, uint _date, int road_assident_number, string place,
            uint count_victim, string type, string reason)
        {
            rKey = r_key;
            tpd_Key = tpd_key;
            date = _date;
            RoadAccidentNumber = road_assident_number;
            Place = place;
            CountOfVicttim = count_victim;
            SetTypeOfAccident(type);//установка тип ДТП
            ReasonOfAccident = reason;
        }

        private string GetTypeOfAccident()
        {
            switch (type_of_accident)
            {
                case TypeAccident.Collision:
                    return "Столкновение";
                case TypeAccident.Overturn:
                    return "Опрокидывание";
                case TypeAccident.H_StandVehicle:
                    return "Наезд на стоящее транспортное средство";
                case TypeAccident.H_Obstacle:
                    return "Наезд на препятствие";
                case TypeAccident.H_Pedestrian:
                    return "Наезд на пешехода";
                case TypeAccident.H_Cyclist:
                    return "Наезд на велосипедиста";
                case TypeAccident.CollisionHorseVehicles:
                    return "Наезд на гужевой транспорт";
                case TypeAccident.PassengerFell:
                    return "Пассажир упал";
                default:
                    return "Другой вид ДТП";
            }
        }
        private void SetTypeOfAccident(string Type)
        {

            IEnumerable<char> lst_sym = Type.SkipWhile(t => t == ' ').Reverse()
                .SkipWhile(t => t == ' ').Reverse();

            Type = "";
            foreach (char sym in lst_sym) Type += sym; //преобразование списка в строку

            try
            {
                type_of_accident = TypesOfAccidents[Type.ToLower()];
            }
            catch (KeyNotFoundException) //строка не совпадает со стандартной ситуацией,
                                        //определенной в словаре
            {
                type_of_accident = TypeAccident.Other;
            }
        }

        public string[] GetTypesOfRoadAccidents()
        {
            return TypesOfAccidents.Keys.ToArray();
        }
    }
    class TrafficPoliceDepartament
    {
        public int tKey { get; set; }
        public int ra_Key { get; set; }
        public string Name { get; set; }

        public TrafficPoliceDepartament() { }
        public TrafficPoliceDepartament(int t_key, int ra_key, string name)
        {
            tKey = t_key;
            ra_Key = ra_key;
            Name = name;
        }
    }

    class Program
    {
        static string[] paths = { "Водители.txt", "Автомобили.txt","ДТП.txt", "Отделы ГИБДД.txt" };
        static List<Driver> drivers = new List<Driver>();
        static List<Car> cars = new List<Car>();
        static List<RoadAccident> road_accidents = new List<RoadAccident>();
        static List<TrafficPoliceDepartament> tpds = new List<TrafficPoliceDepartament>();
        static int[] CountFields = { 4, 5, 8, 3 };
        static void File_Load( string path, int NumberOfList, int Count_Fields)
        {
            StreamReader FileR = new StreamReader(path, Encoding.Default);
            string str;
            while ((str = FileR.ReadLine()) != null)//пока не конец файла
            {
                string[] data = new string[Count_Fields];
                data = str.Split(';');
                switch(NumberOfList)
                {
                    case 0://Водитель
                        Driver d = new Driver();
                        try
                        {
                            d.dKey = int.Parse(data[0]);
                            d.Experience = uint.Parse(data[2]);
                        }
                        catch
                        {
                            Console.WriteLine("Ошибка в строке:\n", str);
                            Console.WriteLine("Для продолжения нажмите на любую клавишу...");
                            Console.ReadKey();
                            return;
                        }
                        d.Name = data[1];
                        d.DriverLicense = data[3];

                        drivers.Add(d);
                        break;

                    case 1://Машина
                        cars.Add(new Car(Convert.ToInt32(data[0]), data[1], data[2], data[3], data[4]));
                        break;
                    case 2:
                        RoadAccident ra = new RoadAccident();
                        try
                        {
                            ra.rKey = int.Parse(data[0]);
                            ra.tpd_Key = int.Parse(data[1]);
                            ra.Date = uint.Parse(data[2]);
                            ra.RoadAccidentNumber = int.Parse(data[3]);
                            ra.CountOfVicttim = uint.Parse(data[5]);
                        }
                        catch
                        {
                            Console.WriteLine("Ошибка в строке:\n", str);
                            Console.WriteLine("Для продолжения нажмите на любую клавишу...");
                            Console.ReadKey();
                            return;
                        }
                        ra.Place = data[4];
                        ra.TypeOfAccident = data[6];
                        ra.ReasonOfAccident = data[7];

                        road_accidents.Add(ra);
                        break;


                    case 3://Отдел ГИБДД
                        TrafficPoliceDepartament tpd = new TrafficPoliceDepartament();
                        try
                        {
                            tpd.tKey = int.Parse(data[0]);
                            tpd.ra_Key = int.Parse(data[1]);
                        }
                        catch
                        {
                            Console.WriteLine("Ошибка в строке\n", str);
                            Console.ReadKey();
                            return;
                        }
                        tpd.Name = data[2];

                        tpds.Add(tpd);
                        break;
                      
                }
            }
        }

        static void Main(string[] args)
        {
            int name_l = 0, exp_l = 0, l;
            string str = "";
            string[] lens;

            //загружаем Базу Данных
            for (int i = 0; i < CountFields.Length; i++)
                File_Load(paths[i], i, CountFields[i]);

            //Запросы
            while (true)
            {
                Console.Clear();
                Console.WriteLine(
                    "1.Добавить информацию о ДТП\n" +
                    "2.Удалить информацию о ДТП\n" +
                    "3.Список водителей, совершивших более одного ДТП.\n" +
                     "4.Список водителей, участвующих в ДТП в заданном месте.\n" +
                      "5.Список водителей, участвующих в ДТП на заданную дату.\n" +
                       "6.ДТП с максимальным количеством потерпевших.\n" +
                        "7.Список водителей, участвующих в ДТП с наездом на пешеходов.\n" +
                         "8.Причины ДТП в порядке убывания их количества.\n" +
                          "9.Выход.");
                char UserChoise = Console.ReadKey().KeyChar;
                switch (UserChoise)
                {
                    case '1'://Добавить информацию о ДТП
                        Console.Clear();
                        Random rand = new Random();
                        int Key = 0, RA_Number = 0;
                        bool IsUnique = false;

                        //Ключ не должен быть Использован ранее в базе !!!!!!
                        //Поэтому проверяем наличие сгенерированного 
                        //ключа в списке с данными о ДТП
                        while (!IsUnique)
                        {
                            Key = rand.Next(-2147483648, 2147483647);
                            IsUnique = true;
                            foreach (RoadAccident ra in road_accidents)
                                if (Key == ra.rKey)
                                    IsUnique = false;
                        }
                        //По той же логике
                        while (!IsUnique)
                        {
                            RA_Number = rand.Next(-2147483648, 2147483647);
                            IsUnique = true;
                            foreach (RoadAccident ra in road_accidents)
                                if (RA_Number == ra.RoadAccidentNumber) IsUnique = false;
                        }

                        RoadAccident ra1 = new RoadAccident();
                        Driver d1 = new Driver();
                        Car c1 = new Car();
                        Console.WriteLine("Введите данные о ДТП:\n");

                        bool IsTpdSelect = false;
                        while (!IsTpdSelect)
                        {
                            Console.WriteLine("Введите название отдела ГИБДД, в котором"
                                + " рассматривается данное ДТП (выберите из списка): ");
                            int i = 1;
                            foreach (TrafficPoliceDepartament tpd in tpds)
                                Console.WriteLine("{0}. {1}", ++i, tpd.Name);
                            string PD_Name = Console.ReadLine();
                            foreach (TrafficPoliceDepartament tpd in tpds)
                                if (PD_Name == tpd.Name)
                                {
                                    IsTpdSelect = true;
                                    ra1.tpd_Key = tpd.ra_Key;
                                }
                        }

                        ra1.rKey = d1.dKey = c1.cKey = Key;//Устанавливаем значение ключа
                        ra1.RoadAccidentNumber = RA_Number;//Уст. номер ДТП

                        //ввод ДТП
                        string[] Types_RA = ra1.GetTypesOfRoadAccidents();
                        while (true)
                        {
                            Console.WriteLine("Выберите причину ДТП:\n" +
                                " 1. {0}\n 2. {1}\n 3. {2}\n 4. {3}\n5. {4}\n" +
                                " 6. {5}\n 7. {6}\n 8. {7}\n 9. {8}\n",
                                Types_RA[0], Types_RA[1], Types_RA[2], Types_RA[3], Types_RA[4],
                                Types_RA[5], Types_RA[6], Types_RA[7], Types_RA[8]);
                            try
                            {
                                ra1.ReasonOfAccident = Types_RA[int.Parse(Console.ReadLine()) - 1];
                            }
                            catch
                            {
                                Console.WriteLine("Введите число из списка!\n");
                                continue;
                            }

                            break;
                        }

                        while (true)
                        {
                            Console.Write("Введите дату: ");
                            try
                            {
                                ra1.Date = uint.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Введите число от 1 до 31!\n");
                                continue;
                            }
                            break;
                        }

                        while (true)
                        {
                            Console.Write("Введите дату: ");
                            try
                            {
                                ra1.CountOfVicttim = uint.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Введите натуральное число!\n");
                                continue;
                            }
                            break;
                        }
                        Console.Write("Введите место ДТП: ");
                        ra1.Place = Console.ReadLine();
                        Console.Write("\nВведите причину ДТП: ");
                        ra1.ReasonOfAccident = Console.ReadLine();

                        //ввод Водитель
                        while (true)
                        {

                            string dl = Console.ReadLine();
                            int a = 0;
                            if (dl.Length > 12 && dl[2] == ' ' && dl[5] == ' ' &&
                                int.TryParse(dl.Substring(0, 1), out a) &&
                                int.TryParse(dl.Substring(3, 4), out a) &&
                                int.TryParse(dl.Substring(6, 11), out a)
                                )
                            {
                                Console.WriteLine("Введите номер формата \"ХХ ХХ ХХХХХХ\", где Х - цифра!\n");
                                continue;
                            }
                            d1.DriverLicense = dl;
                            break;
                        }
                        while (true)
                        {
                            Console.Write("Введите стаж водителя (в годах): ");
                            try
                            {
                                d1.Experience = uint.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Введите натуральное число!\n");
                                continue;
                            }

                            break;
                        }
                        Console.WriteLine("Введите имя водителя");
                        d1.Name = Console.ReadLine();

                        //ввод Машина
                        while (true)
                        {
                            Console.Write("Введите номер автомобиля: ");
                            try
                            {
                                c1.StateCarNumber = Console.ReadLine();
                            }
                            catch
                            {
                                Console.WriteLine("Длина номера не может превышать 9!\n");
                                continue;
                            }

                            break;
                        }

                        Console.Write("Введите фирму автомобиля: ");
                        c1.Firm = Console.ReadLine();

                        Console.Write("Введите марку автомобиля: ");
                        c1.Brend = Console.ReadLine();

                        Console.Write("Введите тип кузова автомобиля");
                        c1.BodyType = Console.ReadLine();

                        Console.WriteLine("Информация о ДТП успешно добавлена!");
                        Console.WriteLine("Для продолжения нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case '2'://Удалить информацию о ДТП
                        Console.Clear();

                        bool IsDeleted = false;
                        int UserChoiseNumber = 0;

                        //Вывод исходной таблицы
                        lens = new string[6];

                        lens[0] = (-"Количество пострадавших".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.RoadAccidentNumber.ToString().Length);
                        lens[1] =
                             l > "Номер ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Номер ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.TypeOfAccident.Length);
                        lens[2] =
                             l > "Тип ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Тип ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.ReasonOfAccident.Length);
                        lens[3] =
                             l > "Причина ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Причина ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.Place.Length);
                        lens[4] =
                             l > "Место".Length
                             ? (-l - 1).ToString()
                             : (-"Место".Length - 1).ToString();

                        lens[5] = (-"Дата".Length - 1).ToString();

                        for (int i = 0; i < lens.Length; i++)
                            str += "{" + i.ToString() + "," + lens[i] + "}";
                        str += "\n";

                        Console.WriteLine(str,
                            "Количество пострадавших", "Номер ДТП",
                            "Тип ДТП", "Причина ДТП", "Место", "Дата");

                        foreach (RoadAccident ra in road_accidents)
                            Console.WriteLine(str,
                                ra.CountOfVicttim, ra.RoadAccidentNumber,
                                ra.TypeOfAccident, ra.ReasonOfAccident, ra.Place, ra.Date);


                        //Удаление
                        while (!IsDeleted)
                        {
                            Console.WriteLine(
                                "\nВыберите номер ДТП (см. таблицу), которое вы хотите удалить" +
                                 " (если вы хотите отменить удаление, введите слово \"Отменить\")\n");

                            string user_choise = Console.ReadLine();
                            if (user_choise.ToLower() == "отменить") break;
                            if (!int.TryParse(user_choise, out UserChoiseNumber))
                            {
                                Console.WriteLine(
                                    "\nВы можете ввести целое число или слово \"Отменить\"\n"
                                    + user_choise + " не является допустимым значением\n"
                                    + "Чтобы продолжить нажмите любую клавишу...");
                                Console.ReadKey();
                                continue;
                            }

                            int i = -1;
                            foreach (RoadAccident ra in road_accidents)
                            {
                                i++;
                                if (UserChoiseNumber == ra.RoadAccidentNumber)
                                {
                                    road_accidents.RemoveAt(i);
                                    IsDeleted = true;
                                    break;//Выход из foreach
                                }
                            }
                        }

                        //Вывод результатов удаления
                        lens = new string[6];

                        lens[0] = (-"Количество пострадавших".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.RoadAccidentNumber.ToString().Length);
                        lens[1] =
                             l > "Номер ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Номер ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.TypeOfAccident.Length);
                        lens[2] =
                             l > "Тип ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Тип ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.ReasonOfAccident.Length);
                        lens[3] =
                             l > "Причина ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Причина ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.Place.Length);
                        lens[4] =
                             l > "Место".Length
                             ? (-l - 1).ToString()
                             : (-"Место".Length - 1).ToString();

                        lens[5] = (-"Дата".Length - 1).ToString();

                        for (int i = 0; i < lens.Length; i++)
                            str += "{" + i.ToString() + "," + lens[i] + "}";
                        str += "\n";

                        Console.WriteLine(str,
                            "Количество пострадавших", "Номер ДТП",
                            "Тип ДТП", "Причина ДТП", "Место", "Дата");
                        foreach (RoadAccident ra in road_accidents)
                            Console.WriteLine(str,
                                ra.CountOfVicttim, ra.RoadAccidentNumber,
                                ra.TypeOfAccident, ra.ReasonOfAccident, ra.Place, ra.Date);

                        Console.WriteLine("\nИнформация о ДТП с номером {0} успешно удалена!", UserChoiseNumber);
                        Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case '3'://Список водителей, совершивших более одного ДТП
                        Console.Clear();
                        var Drivers_Most1RA =
                            //Нужна ли сортировка?
                            drivers.GroupJoin(road_accidents, d => d.dKey, ra => ra.rKey, (d, ra) =>
                                new
                                {
                                    d.DriverLicense,
                                    d.Name,
                                    d.Experience,
                                    lst = ra
                                }).Where(group => group.lst.Count() > 1);

                        if (Drivers_Most1RA.Count() == 0)
                        {
                            Console.WriteLine("Нет данных!");
                            Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                            Console.ReadKey();
                            break;
                        }

                        name_l = -Drivers_Most1RA.Max(d => d.Name.Length) - 1;
                        exp_l = -Drivers_Most1RA.Max(d => d.Experience.ToString().Length) - 1;

                        str =
                            "{0, " + ("Имя".Length > -name_l ? (-"Имя".Length - 1).ToString()
                            : name_l.ToString()) + "}" +
                            "{1, " + ("Опыт".Length > -exp_l ? (-"Опыт".Length - 1).ToString()
                            : exp_l.ToString()) + "}" +
                            "{2, " + (-"№ водительского уд.".Length).ToString() + "}\n";

                        Console.WriteLine(str,
                            "Имя", "Опыт", "№ водительского уд.");
                        foreach (var dr in Drivers_Most1RA)
                        {

                            Console.WriteLine(str,
                                dr.Name,
                                dr.Experience, dr.DriverLicense);
                        }

                        Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case '4'://Список водителей, участвующих в ДТП в заданном месте
                        Console.Clear();
                        Console.Write("Введите место ДТП: ");
                        string place = Console.ReadLine();

                        var  Driver_InPlace =
                            road_accidents.Join(drivers, ra => ra.rKey, d => d.dKey,
                            (ra, d) => new
                            {
                                d.DriverLicense,
                                d.Experience,
                                d.Name,
                                ra.Place
                            }
                            ).Where(dp => dp.Place == place);

                        if (Driver_InPlace.Count() == 0)
                        {
                            Console.WriteLine("\nНет информации по заданному месту");
                            Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                            Console.ReadKey();
                            break;
                        }

                        name_l = -Driver_InPlace.Max(d => d.Name.Length) - 1;
                        exp_l = -Driver_InPlace.Max(d => d.Experience.ToString().Length) - 1;

                        str =
                            "{0, " + ("Имя".Length > -name_l ? (-"Имя".Length - 1).ToString()
                            : name_l.ToString()) + "}" +
                            "{1, " + ("Опыт".Length > -exp_l ? (-"Опыт".Length - 1).ToString()
                            : exp_l.ToString()) + "}" +
                            "{2, " + (-"№ водительского уд.".Length).ToString() + "}\n";

                        Console.WriteLine(str,
                            "Имя", "Опыт", "№ водительского уд.");
                        foreach (var dr in Driver_InPlace)
                        {

                            Console.WriteLine(str,
                                dr.Name,
                                dr.Experience, dr.DriverLicense);
                        }
                        Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case '5'://Список водителей, участвующих в ДТП на заданную дату

                        uint date = 0;

                        while (true)
                        {
                            Console.Clear();
                            Console.Write("Введите дату ДТП: ");
                            try
                            {
                                date = uint.Parse(Console.ReadLine());
                                if (date > 31) throw new Exception();
                            }
                            catch
                            {
                                Console.WriteLine("Дата должна лежать в диапазоне от"
                                    + "1 до 31 и являться целым числом!"
                                    + "\nДля продолжения нажмите любую клавишу... ");
                                Console.ReadKey();
                                continue;
                            }
                            break;
                        }
                        var Driver_ByDate =
                               road_accidents.Join(drivers, ra => ra.rKey, d => d.dKey,
                                (ra, d) => new
                                {
                                    d.DriverLicense,
                                    d.Experience,
                                    d.Name,
                                    ra.Date
                                }
                        ).Where(dd => dd.Date == date);
                        //Вывод таблицы
                        if (Driver_ByDate.Count() == 0)
                        {
                            Console.WriteLine("\nНет информации по заданной дате");
                            Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                            Console.ReadKey();
                            break;
                        }


                        name_l = -Driver_ByDate.Max(d => d.Name.Length) - 1;
                        exp_l = -Driver_ByDate.Max(d => d.Experience.ToString().Length) - 1;

                        str =
                            "{0, " + ("Имя".Length > -name_l ? (-"Имя".Length - 1).ToString()
                            : name_l.ToString()) + "}" +
                            "{1, " + ("Опыт".Length > -exp_l ? (-"Опыт".Length - 1).ToString()
                            : exp_l.ToString()) + "}" +
                            "{2, " + (-"№ водительского уд.".Length).ToString() + "}\n";

                        Console.WriteLine(str,
                            "Имя", "Опыт", "№ водительского уд.");
                        foreach (var dr in Driver_ByDate)
                        {

                            Console.WriteLine(str,
                                dr.Name,
                                dr.Experience, dr.DriverLicense);
                        }

                        Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case '6'://ДТП с максимальным количеством потерпевших.
                        Console.Clear();
                        uint max_count = road_accidents.Max(ra => ra.CountOfVicttim);
                        //Если сделать запрос Where, то условный оператор при выводе не понадобится
                        //Надо ли?


                        road_accidents =
                            road_accidents.Where(Ra => Ra.CountOfVicttim == max_count).ToList();
                        //Вывод результатов
                        lens = new string[6];
                        lens[0] = (-"Количество пострадавших".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.RoadAccidentNumber.ToString().Length);
                        lens[1] =
                             l > "Номер ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Номер ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.TypeOfAccident.Length);
                        lens[2] =
                             l > "Тип ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Тип ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.ReasonOfAccident.Length);
                        lens[3] =
                             l > "Причина ДТП".Length
                             ? (-l - 1).ToString()
                             : (-"Причина ДТП".Length - 1).ToString();

                        l = road_accidents.Max(Ra => Ra.Place.Length);
                        lens[4] =
                             l > "Место".Length
                             ? (-l - 1).ToString()
                             : (-"Место".Length - 1).ToString();

                        lens[5] = (-"Дата".Length - 1).ToString();

                        for(int i = 0; i < lens.Length; i++)
                              str += "{" + i.ToString() + "," + lens[i] + "}";
                        str += "\n";

                        Console.WriteLine(str,
                            "Количество пострадавших","Номер ДТП",
                            "Тип ДТП", "Причина ДТП", "Место", "Дата");
                        foreach (RoadAccident ra in road_accidents)
                                Console.WriteLine(str,
                                    ra.CountOfVicttim, ra.RoadAccidentNumber, 
                                    ra.TypeOfAccident, ra.ReasonOfAccident,  ra.Place, ra.Date);
                        Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case '7': //Список водителей, участвующих в ДТП с наездом на пешеходов.
                        Console.Clear();
                        var Drivers_Filtered = road_accidents.Join(drivers,
                            ra=>ra.rKey, d => d.dKey, (ra, d) =>
                            new
                            {
                                d.DriverLicense,
                                d.Experience,
                                d.Name,
                                ra.TypeOfAccident
                            }).Where(df=>df.TypeOfAccident == "Наезд на пешехода");

                        if (Drivers_Filtered.Count() == 0)
                        {
                            Console.WriteLine("\nНет информации по заданной дате");
                            Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                            Console.ReadKey();
                            break;
                        }


                        name_l = -Drivers_Filtered.Max(d => d.Name.Length) - 1;
                        exp_l = -Drivers_Filtered.Max(d => d.Experience.ToString().Length) - 1;

                        str =
                            "{0, " + ("Имя".Length > -name_l ? (-"Имя".Length - 1).ToString()
                            : name_l.ToString()) + "}" +
                            "{1, " + ("Опыт".Length > -exp_l ? (-"Опыт".Length - 1).ToString()
                            : exp_l.ToString()) + "}" +
                            "{2, " + (-"№ водительского уд.".Length).ToString() + "}\n";

                        Console.WriteLine(str,
                            "Имя", "Опыт", "№ водительского уд.");
                        foreach (var dr in Drivers_Filtered)
                        {

                            Console.WriteLine(str,
                                dr.Name,
                                dr.Experience, dr.DriverLicense);
                        }
                        Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case '8'://Причины ДТП в порядке убывания их количества.
                        Console.Clear();
                        var list_of_groups = road_accidents.GroupBy(ra => ra.ReasonOfAccident)
                            .Select(ra_grp => new
                        {
                            grp_name = ra_grp.Key,
                            lst = ra_grp
                        }).OrderByDescending(ra_grps => ra_grps.lst.Count());

                        int l1 = list_of_groups.Max(gr => gr.grp_name.Length);
                        str = "{0, " +
                            (l1 > "Причина ДТП".Length? (-l1 - 1).ToString()
                            : (-"Причина ДТП".Length - 1).ToString()) +
                            "} {1}";

                        Console.WriteLine(str, "Причина ДТП","Количество ДТП");
                        foreach (var ra_1grp in list_of_groups)
                            Console.WriteLine(str, ra_1grp.grp_name, ra_1grp.lst.Count());
                        Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                        Console.ReadKey();
                        break;

                    case '9'://Выход из программы
                        Console.Clear();
                        do
                        {
                            Console.WriteLine("Вы уверены, что хотите выйти? \n1.Да \n2.Нет");
                            UserChoise = Console.ReadKey().KeyChar;
                        } while (UserChoise != '1' && UserChoise != '2');
                        if (UserChoise == '1') return;
                        break;
                }
            }
        }
    }
}