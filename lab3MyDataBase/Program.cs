using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace lab3MyDataBase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Getting Connection...");
            MySqlConnection conn = DBUtils.GetDBConnection();

            try
            {
                Console.WriteLine("Openning Connection...");

                conn.Open();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connection successful!");
                Console.ResetColor();

                do
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Введіть цифру бажаного запиту до бази даних:");
                    Console.ResetColor();
                    Console.WriteLine("1. Переглянути всю інформацію про вузли");
                    Console.WriteLine("2. Переглянути всю інформацію про постачальників");
                    Console.WriteLine("3. Переглянути всю інформацію про замовлення");
                    Console.WriteLine("4. Роздрукувати рахунки за замовлення");
                    Console.WriteLine("Q. Вийти");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            QueryNode(conn);
                            break;
                        case "2":
                            QuerySupplier(conn);
                            break;
                        case "3":
                            QueryOrder(conn);
                            break;
                        case "4":
                            QueryInvoice(conn);
                            break;
                        case "Q":
                            Console.WriteLine("До побачення!");
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Помилка: Невідомий вибір");
                            Console.ResetColor();
                            break;
                    }
                } while (true);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error:" + e.Message);
                Console.ResetColor();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private static void QueryNode(MySqlConnection conn)
        {
            string node_name, node_code, node_price, node_min_supply, supplier_name, supplier_code;
            string sql = "SELECT * FROM a_node";

            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = sql;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ТАБЛИЦЯ ВУЗЛИ АВТОМОБІЛЯ");
                    Console.ResetColor();
                    while (reader.Read())
                    {
                        node_name = reader["node_name"].ToString();
                        node_code = reader["node_code"].ToString();
                        node_price = reader["node_price"].ToString();
                        node_min_supply = reader["node_min_supply"].ToString();
                        supplier_name = reader["a_supplier_supplier_name"].ToString();
                        supplier_code = reader["a_supplier_supplier_code"].ToString();
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Назва: " + node_name + "; Код: " + node_name + "; Ціна: " + node_price + 
                        "; Ім'я постачальника: " + supplier_name + "; Код постачальника: " + supplier_code);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
        }

        private static void QuerySupplier(MySqlConnection conn)
        {
            string supplier_name, supplier_code, supplier_address, supplier_phone, supplier_surname;
            string sql = "SELECT * FROM a_supplier";

            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = sql;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ТАБЛИЦЯ ПОСТАЧАЛЬНИКІВ ВУЗЛІВ АВТОМОБІЛЯ");
                    Console.ResetColor();
                    while (reader.Read())
                    {
                        supplier_name = reader["supplier_name"].ToString();
                        supplier_code = reader["supplier_code"].ToString();
                        supplier_address = reader["supplier_address"].ToString();
                        supplier_phone = reader["supplier_phone"].ToString();
                        supplier_surname = reader["supplier_surname"].ToString();
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Ім'я: " + supplier_name + "; Код: " + supplier_code + "; Адреса: " + supplier_address +
                        "; Телефон постачальника: +380" + supplier_phone + "; Прізвище постачальника: " + supplier_surname);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
        }

        private static void QueryOrder(MySqlConnection conn)
        {
            string order_code, order_date_fill, order_amount, order_date_delivery, node_code, node_name;
            string sql = "SELECT * FROM a_order";

            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = sql;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ТАБЛИЦЯ ЗАМОВЛЕНЬ ВУЗЛІВ АВТОМОБІЛЯ");
                    Console.ResetColor();
                    while (reader.Read())
                    {
                        order_code = reader["order_code"].ToString();
                        order_date_fill = reader["order_date_fill"].ToString();
                        order_amount = reader["order_amount"].ToString();
                        order_date_delivery = reader["order_date_delivery"].ToString();
                        node_code = reader["a_node_node_code"].ToString();
                        node_name = reader["a_node_node_name"].ToString();
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Код замовлення: " + order_code + "; Дата замовлення: " + order_date_fill + "; Кількість: " + order_amount +
                        "; Дата доставки: " + order_date_delivery + "; Код вузла: " + node_code 
                        + "; Назва вузла: " + node_name);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
        }
        private static void QueryInvoice(MySqlConnection conn)
        {
            string sql = "SELECT CONCAT('Рахунок №', CONVERT(order_code, CHAR), ' від ', CONVERT(order_date_fill, CHAR), ' на суму ', CONVERT(order_amount * node_price, CHAR)) AS Рахунки FROM a_order JOIN a_node ON a_order.a_node_node_code = a_node.node_code ORDER BY order_code";

            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = sql;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("РАХУНКИ");
                    Console.ResetColor();
                    while (reader.Read())
                    {
                        string invoice = reader["Рахунки"].ToString();
                        Console.WriteLine(invoice);
                    }
                }
            }
        }

    }
}
