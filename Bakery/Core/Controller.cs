using Bakery.Core.Contracts;
using Bakery.Models.BakedFoods;
using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables;
using Bakery.Models.Tables.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bakery.Core
{
    public class Controller : IController
    {
        private List<IBakedFood> bakedFoods;
        private List<IDrink> drinks;
        private List<ITable> tables;
        private List<decimal> bills;

        public Controller()
        {
            this.bakedFoods = new List<IBakedFood>();
            this.drinks = new List<IDrink>();
            this.tables = new List<ITable>();
            this.bills = new List<decimal>();
        }
        public string AddDrink(string type, string name, int portion, string brand)
        {
            IDrink drink;

            if (type == "Tea")
            {
                drink = new Tea(name, portion, brand);
            }
            else if (type == "Water")
            {
                drink = new Water(name, portion, brand);
            }
            else
            {
                throw new ArgumentException("Invalid drink type");
            }

            this.drinks.Add(drink);
            return $"Added {drink.Name} ({drink.Brand}) to the drink menu";
        }

        public string AddFood(string type, string name, decimal price)
        {
            IBakedFood bakedFood;
            if (type == "Bread")
            {
                bakedFood = new Bread(name, price);
            }
            else if (type == "Cake")
            {
                bakedFood = new Cake(name, price);
            }
            else
            {
                throw new ArgumentException("Invalid food type");
            }

            this.bakedFoods.Add(bakedFood);
            return $"Added {bakedFood.Name} ({bakedFood.GetType().Name}) to the menu";
        }

        public string AddTable(string type, int tableNumber, int capacity)
        {
            ITable table;

            if (type == "InsideTable")
            {
                table = new InsideTable(tableNumber, capacity);
            }
            else if (type == "OutsideTable")
            {
                table = new OutsideTable(tableNumber, capacity);
            }
            else
            {
                throw new ArgumentException("Invalid table type");
            }

            this.tables.Add(table);

            return $"Added table number {table.TableNumber} in the bakery";
        }

        public string GetFreeTablesInfo()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in this.tables.Where(t => t.IsReserved == false))
            {
                sb.AppendLine(item.GetFreeTableInfo());
            }

            return sb.ToString().Trim();
        }

        public string GetTotalIncome()
        {
            decimal income = this.bills.Sum();

            return $"Total income: {income:f2}lv";
        }

        public string LeaveTable(int tableNumber)
        {
            decimal bill = 0;
            StringBuilder sb = new StringBuilder();

            foreach (var item in this.tables)
            {
                if (item.TableNumber == tableNumber)
                {
                    bill = item.GetBill();
                    item.Clear();
                }
            }

            this.bills.Add(bill);

            sb.AppendLine($"Table: {tableNumber}");
            sb.AppendLine($"Bill: {bill:f2}");

            return sb.ToString().Trim();
        }

        public string OrderDrink(int tableNumber, string drinkName, string drinkBrand)
        {
            ITable table = this.tables.FirstOrDefault(t => t.TableNumber == tableNumber);
            IDrink drink = this.drinks.FirstOrDefault(d => d.Name == drinkName && d.Brand == drinkBrand);

            if (table == null)
            {
                return $"Could not find table {tableNumber}";
            }
            else if (drink == null)
            {
                return $"There is no {drinkName} {drinkBrand} available";
            }
            else
            {
                table.OrderDrink(drink);

                return $"Table {tableNumber} ordered {drinkName} {drinkBrand}";
            }
        }

        public string OrderFood(int tableNumber, string foodName)
        {
            ITable table = this.tables.FirstOrDefault(t => t.TableNumber == tableNumber);
            IBakedFood bakedFood = this.bakedFoods.FirstOrDefault(bf => bf.Name == foodName);

            if (table == null)
            {
                return $"Could not find table {tableNumber}";
            }
            else if (bakedFood == null)
            {
                return $"No {foodName} in the menu";
            }
            else
            {
                table.OrderFood(bakedFood);

                return $"Table {tableNumber} ordered {foodName}";
            }
        }

        public string ReserveTable(int numberOfPeople)
        {
            foreach (var item in this.tables)
            {
                if (!item.IsReserved && item.Capacity >= numberOfPeople)
                {
                    item.Reserve(numberOfPeople);

                    return $"Table {item.TableNumber} has been reserved for {numberOfPeople} people";
                }
            }

            return $"No available table for {numberOfPeople} people";
        }
    }
}
