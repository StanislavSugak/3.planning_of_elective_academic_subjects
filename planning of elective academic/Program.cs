using System;
using System.Drawing;
using System.IO;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

class Program
{

    static void Main(string[] args)
    {
        MainMenu();
    }

    static int Select(string[] menu)
    {

        Console.Clear();

        for (int i = 0; i < menu.Length; i++)
        {
            if (i == 0 || i == menu.Length - 1)
            {
                Console.WriteLine("\n   " + menu[i] + "\n");
            }
            else
            {
                Console.WriteLine("   " + menu[i]);
            }
        }



        int number;
        Console.SetCursorPosition(50, 2 + menu.Length);
        while (!int.TryParse(Console.ReadLine(), out number) || (number < 0 || number > menu.Length - 2))
        {
            menu[0] = menu[0].Replace("Неверно введен номер! ", "");
            Console.Clear();
            menu[0] = "Неверно введен номер! " + menu[0];
            return Select(menu);
        }
        menu[0] = menu[0].Replace("Неверно введен номер! ", "");

        return number;
    }

    static bool EnterMenu()
    {
        bool showmenu = true;
        while (showmenu)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Enter:
                    showmenu = false;
                    return true;
                default:
                    break;
            }
        }

        return true;
    }


    static void MainMenu()
    {
        string[] menu = new string[] { "Выберите пункт из меню", "1. Администратор", "2. Пользователь", "3. Выход", "Введите номер и нажмите Enter для продолжения: " };

        bool showmenu = true;

        string path = @"Data";
        string[] electives = new string[] { "База данных", "Английский язык", "Математика", "Физика", "Программирование" };

        Student[][] students = new Student[5][];
        ReadStudent(ref students, path);

        LP admin = new LP();
        ReadAuthorization(ref admin, path);

        while (showmenu)
        {
            int number = Select(menu);
            showmenu = false;

            if (number == 1)
            {
                showmenu = Administrator(students, electives, admin, path);
            }
            if (number == 2)
            {
                showmenu = User(students, path);
            }
            if (number == 3)
            {
                showmenu = Exit();
            }

        }

    }

    struct Student
    {
        public string name;
        public string group;
        public double result;
        public Optional electives;
    }

    struct Optional
    {
        public int database;
        public int englishlanguage;
        public int mathematics;
        public int physics;
        public int programming;
    }

    struct LP
    {
        public string login;
        public string password;
    }

    static void ReadStudent(ref Student[][] students, string path)
    {
        Array.Clear(students);

        students[0] = new Student[0];
        students[1] = new Student[0];
        students[2] = new Student[0];
        students[3] = new Student[0];
        students[4] = new Student[0];

        string pathd = path + @"\Statement";
        string[] statements = Directory.GetFiles(pathd);
        int count = -1;

        foreach (string statement in statements)
        {
            Student student = new Student();
            count++;

            string lines = "";
            using (StreamReader sr = new StreamReader(statement))
            {
                while (!sr.EndOfStream)
                {
                    lines = string.Concat(lines, sr.ReadLine());
                }

            }

            string[] data = lines.Split(';');
            string[] electiv = data[3].Split(',');

            student.name = data[0];
            student.group = data[1];
            student.result = double.Parse(data[2]);

            for (int i = 0; i < electiv.Length; i++)
            {
                if (int.Parse(electiv[i]) == 1)
                {
                    Array.Resize(ref students[i], students[i].Length + 1);
                    students[i][students[i].Length - 1] = student;
                }
            }

        }
    }

    static void ReadAuthorization(ref LP admin, string path)
    {

        string patha = path + @"\Admin\Authorization.txt";
        using (StreamReader sr = new StreamReader(patha))
        {
            admin.login = sr.ReadLine();
            admin.password = sr.ReadLine();
        }

    }

    static bool Authorization(LP admin)
    {
        Console.Clear();

        LP temp = new LP();

        Console.WriteLine("\n   Введите данные для авторизации...");
        Console.WriteLine("\n   Введите логин: ");
        Console.WriteLine("\n   Введите пароль: ");
        Console.WriteLine("\n   Нажмите Enter чтобы продолжить...");

        Console.SetCursorPosition(18, 3);
        temp.login = Console.ReadLine();

        Console.SetCursorPosition(19, 5);
        temp.password = Console.ReadLine();

        return CorrectAuthorization(temp, admin);
    }

    static bool CorrectAuthorization(LP temp, LP admin)
    {
        Console.Clear();
        if (temp.login == admin.login && temp.password == admin.password)
        {
            return true;
        }
        else
        {
            string[] menu = new string[] { "Неверно введены данные, желаете повторить?", "1. Да", "2. Нет", "Введите номер и нажмите Enter для продолжения: " };

            int number = Select(menu);
            if (number == 1)
            {
                return Authorization(admin);
            }
            else
            {
                return false;
            }

        }


    }

    static bool Administrator(Student[][] students, string[] electives, LP admin, string path)
    {
        if (Authorization(admin))
        {
            string[] menu = new string[] { "Выберите пункт из меню", "1. Просмотр различной информации", "2. Изменить данные входа", "3. Выход", "Введите номер и нажмите Enter для продолжения: " };

            bool showmenu = true;

            while (showmenu)
            {
                int number = Select(menu);
                showmenu = false;

                if (number == 1)
                {
                    showmenu = Viewing(students, electives);
                }
                if (number == 2)
                {
                    showmenu = EditAuthorization(admin, path);
                }
                if (number == 3)
                {
                    showmenu = Exit();
                }

            }
        }

        return true;
    }

    static bool Viewing(Student[][] students, string[] electives)
    {

        string[] menu = new string[] { "Выберите пункт из меню", "1. Просмотр факультатива", "2. Просмотр претендующих", "3. Популярность", "4. Назад", "Введите номер и нажмите Enter для продолжения: " };

        bool showmenu = true;

        while (showmenu)
        {
            int number = Select(menu);
            showmenu = false;

            if (number == 1)
            {
                showmenu = Electives(students);
            }
            if (number == 2)
            {
                showmenu = Applicants(students);
            }
            if (number == 3)
            {
                showmenu = Popularity(students, electives);
            }
            if (number == 4)
            {
                showmenu = false;
            }


        }
        return true;
    }

    static int SelectElectives()
    {
        string[] menu = new string[] { "Выберите пункт из меню", "1. База данных", "2. Английский язык", "3. Математика", "4. Физика", "5. Программирование", "6. Назад", "Введите номер и нажмите Enter для продолжения: " };

        int number = Select(menu);

        if (number != menu.Length - 2)
        {
            return number;
        }
        else
        {
            return -1;
        }

    }

    static bool Electives(Student[][] students)
    {
        bool showmenu = true;

        while (showmenu)
        {
            int number = SelectElectives();
            showmenu = false;

            if (number != -1)
            {
                showmenu = ShowElectives(students, number - 1);
            }
            else
            {
                showmenu = false;
            }

        }
        return true;
    }

    static bool ShowElectives(Student[][] students, int number)
    {
        Console.Clear();

        Console.WriteLine("\n   Общее количество заявок: " + students[number].Length);
        for (int i = 0; i < students[number].Length; i++)
        {
            Console.Write($"\n   {i + 1}. {students[number][i].name}   {students[number][i].group}");
            if (i == students[number].Length - 1)
            {
                Console.Write("\n");
            }
        }
        Console.WriteLine("\n   Нажмите Enter для продолжения...");

        return EnterMenu();
    }

    static bool EditAuthorization(LP admin, string path)
    {
        string[] menu = new string[] { "Что желаете изменить?", "1. Логин", "2. Пароль", "3. Назад", "Введите номер и нажмите Enter для продолжения: " };

        bool showmenu = true;

        while (showmenu)
        {
            int number = Select(menu);
            showmenu = false;

            if (number != 3)
            {
                if (number == 1)
                {
                    ChangeLogin(ref admin);
                }
                if (number == 2)
                {
                    ChangePassword(ref admin);
                }
                showmenu = ChangeAuthorization(admin, path);
            }
            if (number == 3)
            {
                showmenu = false;
            }
        }

        return true;
    }

    static bool ChangeAuthorization(LP admin, string path)
    {
        Console.Clear();
        using (StreamWriter sw = new StreamWriter(path + @"\Admin\Authorization.txt"))
        {
            sw.WriteLine(admin.login);
            sw.WriteLine(admin.password);
        }

        Console.WriteLine("\n   Вы успешно поменяли данные авторизации...");
        Console.WriteLine("\n   Нажмите Enter чтобы продолжить...");
        return EnterMenu();
    }

    static void ChangeLogin(ref LP admin)
    {
        Console.Clear();
        Console.Write("\n   Введите логин: ");
        admin.login = Console.ReadLine();
    }

    static void ChangePassword(ref LP admin)
    {
        Console.Clear();
        Console.Write("\n   Введите пароль: ");
        admin.password = Console.ReadLine();
    }

    static bool Applicants(Student[][] students)
    {
        bool showmenu = true;

        while (showmenu)
        {
            int number = SelectElectives();
            showmenu = false;

            if (number != -1)
            {
                showmenu = ShowApplicants(students, number - 1);
            }
            else
            {
                showmenu = false;
            }

        }
        return true;

    }

    static bool ShowApplicants(Student[][] students, int number)
    {

        Student[] student = students[number];
        SortApplicants(ref student);

        return SwowApplicants(student);
    }

    static void SortApplicants(ref Student[] students)
    {
        for (int i = 0; i < students.Length - 1; i++)
        {
            double max = students[i].result;
            for (int j = i + 1; j < students.Length; j++)
            {
                if (students[j].result > max)
                {
                    Student temp = students[i];
                    students[i] = students[j];
                    students[j] = temp;
                }
            }
        }
    }

    static bool SwowApplicants(Student[] student)
    {
        Console.Clear();
        Console.WriteLine("\n   Топ участников, которые подали заявление...");

        for (int i = 0; i < student.Length && i < 16; i++)
        {
            Console.WriteLine($"\n   {i + 1}. {student[i].name}");
            Console.WriteLine($"   Средний балл: {student[i].result}");
        }

        Console.WriteLine("\n   Нажмите Enter чтобы продолжить...");
        return EnterMenu();
    }

    static bool Popularity(Student[][] students, string[] electives)
    {

        int[] numorder = new int[5];
        SortPopularity(students, ref numorder);

        return ShowPopularity(students, numorder, electives);
    }

    static void SortPopularity(Student[][] students, ref int[] numorder)
    {
        int[] order = new int[5];

        for (int i = 0; i < students.Length; i++)
        {
            order[i] = students[i].Length;
            numorder[i] = i;
        }

        for (int i = 0; i < order.Length - 1; i++)
        {
            int max = order[i];
            for (int j = i + 1; j < order.Length; j++)
            {
                if (order[j] > max)
                {
                    int temp = order[i];
                    order[i] = order[j];
                    order[j] = temp;

                    int temp1 = numorder[i];
                    numorder[i] = numorder[j];
                    numorder[j] = temp1;
                }
            }
        }

    }

    static bool ShowPopularity(Student[][] students, int[] numorder, string[] electives)
    {
        Console.Clear();

        Console.WriteLine("\n   Факультативы по популярности...");

        for (int i = 0; i < students.Length; i++)
        {
            Console.Write($"\n   {i + 1}. {electives[numorder[i]]}");
            Console.Write($"   Общее количество: {students[numorder[i]].Length}\n");
        }

        Console.WriteLine("\n   Нажмите Enter чтобы продолжить...");
        return EnterMenu();
    }

    static bool User(Student[][] students, string path)
    {
        string[] menu = new string[] { "Выберите пункт из меню", "1. Подать заявление для посещения факультативных занятий", "2. Назад", "Введите номер и нажмите Enter для продолжения: " };

        bool showmenu = true;

        while (showmenu)
        {
            int number = Select(menu);
            showmenu = false;

            if (number == 1)
            {
                showmenu = Statement(path);
                ReadStudent(ref students, path);
            }
            else
            {
                showmenu = false;
            }
        }

        return true;
    }

    static bool Statement(string path)
    {
        Console.Clear();

        Student student = new Student();

        Console.WriteLine("\n   Заполните бланк чтобы подать заявлениев группу...");
        StatementAddBio(ref student);
        StatementAddElectives(ref student);

        AddStatement(student, path);

        return true;
    }

    static void StatementAddBio(ref Student student)
    {

        Console.WriteLine("\n   Введите ФИО: ");
        Console.WriteLine("\n   Введите группу: ");
        Console.WriteLine("\n   Введите средний балл: ");

        Console.SetCursorPosition(16, 3);
        student.name = Console.ReadLine();

        Console.SetCursorPosition(19, 5);
        student.group = Console.ReadLine();

        Console.SetCursorPosition(25, 7);
        int position = 7;
        while (!double.TryParse(Console.ReadLine(), out student.result) || (student.result > 10.1 || student.result < 0))
        {
            Console.WriteLine("\n   Неверно введен средний балл! Введите средний балл повторно: ");
            position += 2;
            Console.SetCursorPosition(63, position);
        }
    }

    static void StatementAddElectives(ref Student student)
    {
        Console.Clear();
        Console.WriteLine("\n   Введите 0 - если не хотите выбрать факультатив, 1 - если хотите выбрать факультатив...");

        Console.Write("\n   База данных: ");
        int position = 3;
        while (!int.TryParse(Console.ReadLine(), out student.electives.database) || (student.electives.database < 0 || student.electives.database > 1))
        {
            Console.WriteLine("\n   Неверно введены данные! Введите повторно: ");
            position += 2;
            Console.SetCursorPosition(45, position);
        }
        position += 2;

        Console.Write("\n   Английский язык: ");
        while (!int.TryParse(Console.ReadLine(), out student.electives.englishlanguage) || (student.electives.englishlanguage < 0 || student.electives.englishlanguage > 1))
        {
            Console.WriteLine("\n   Неверно введены данные! Введите повторно: ");
            position += 2;
            Console.SetCursorPosition(45, position);
        }
        position += 2;

        Console.Write("\n   Математика: ");
        while (!int.TryParse(Console.ReadLine(), out student.electives.mathematics) || (student.electives.mathematics < 0 || student.electives.mathematics > 1))
        {
            Console.WriteLine("\n   Неверно введены данные! Введите повторно: ");
            position += 2;
            Console.SetCursorPosition(45, position);
        }
        position += 2;

        Console.Write("\n   Физика: ");
        while (!int.TryParse(Console.ReadLine(), out student.electives.physics) || (student.electives.physics < 0 || student.electives.physics > 1))
        {
            Console.WriteLine("\n   Неверно введены данные! Введите повторно: ");
            position += 2;
            Console.SetCursorPosition(45, position);
        }
        position += 2;

        Console.Write("\n   Программирование: ");
        while (!int.TryParse(Console.ReadLine(), out student.electives.programming) || (student.electives.programming < 0 || student.electives.programming > 1))
        {
            Console.WriteLine("\n   Неверно введены данные! Введите повторно: ");
            position += 2;
            Console.SetCursorPosition(45, position);
        }
        position += 2;

    }

    static bool AddStatement(Student student, string path)
    {

        Console.Clear();
        int count = CountStatement(path);

        string paths = path + @"\Statement\" + $"Statement {count}.txt";
        File.Create(paths).Close();

        using (StreamWriter sw = new StreamWriter(paths))
        {
            sw.WriteLine(student.name + ";");
            sw.WriteLine(student.group + ";");
            sw.WriteLine(student.result + ";");

            sw.Write(student.electives.database + ",");
            sw.Write(student.electives.englishlanguage + ",");
            sw.Write(student.electives.mathematics + ",");
            sw.Write(student.electives.physics + ",");
            sw.Write(student.electives.programming);
        }

        Console.WriteLine("\n   Вы подали заяление! Нажмите Enter чтобы продолжить...");
        return EnterMenu();
    }

    static int CountStatement(string path)
    {
        int count = 0;
        string pathc = path + @"\Admin\Count.txt";

        using (StreamReader sr = new StreamReader(pathc))
        {
            count = int.Parse(sr.ReadLine()) + 1;
        }

        using (StreamWriter sw = new StreamWriter(pathc))
        {
            sw.WriteLine(count);
        }
        return count;
    }

    static bool Exit()
    {

        string[] menu = new string[] { "Вы желаете выйти? Выберите пункт из меню", "1. Да", "2. Нет", "Введите номер и нажмите Enter для продолжения: " };

        bool showmenu = true;

        while (showmenu)
        {
            int number = Select(menu);
            showmenu = false;

            if (number == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return true;
    }

}
