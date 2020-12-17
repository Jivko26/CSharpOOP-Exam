using NUnit.Framework;
using System;

namespace BankSafe.Tests
{
    public class BankVaultTests
    {


        [Test]
        public void ConstrucotorTest()
        {
            BankVault bankVault = new BankVault();

            Assert.That(bankVault.VaultCells.Count == 12);
        }
        [Test]
        public void AddItemMethodTest()
        {
            BankVault bankVault = new BankVault();
            Item item = new Item("az", "26");

            Assert.Throws<ArgumentException>(() => bankVault.AddItem("a12", item));
        }

        [Test]
        public void AddItemMethodTest1()
        {
            BankVault bankVault = new BankVault();
            Item item = new Item("az", "26");

            string result = bankVault.AddItem("A1", item);

            Assert.That(bankVault.VaultCells["A1"] == item);
            Assert.That(result == $"Item:{item.ItemId} saved successfully!");
        }

        [Test]
        public void AddItemMethodTest2()
        {
            BankVault bankVault = new BankVault();
            Item item = new Item("az", "26");

            bankVault.AddItem("A1", item);

            Assert.Throws<ArgumentException>(() => bankVault.AddItem("A1", item));

        }

        [Test]
        public void AddItemMethodTest3()
        {
            BankVault bankVault = new BankVault();
            Item item = new Item("az", "26");

            bankVault.AddItem("A1", item);

            Assert.Throws<InvalidOperationException>(() => bankVault.AddItem("A2", item));

        }

        [Test]
        public void RemoveItemMethodTest()
        {
            BankVault bankVault = new BankVault();
            Item item = new Item("az", "26");
            bankVault.AddItem("A1", item);

            string result = bankVault.RemoveItem("A1", item);

            Assert.That(bankVault.VaultCells["A1"] == null);
            Assert.That(result == $"Remove item:{item.ItemId} successfully!");
        }

        [Test]
        public void RemoveItemMethodTest1()
        {
            BankVault bankVault = new BankVault();
            Item item = new Item("az", "26");

            Assert.Throws<ArgumentException>(() => bankVault.RemoveItem("s", item));
        }

        [Test]
        public void RemoveItemMethodTest2()
        {
            BankVault bankVault = new BankVault();
            Item item = new Item("az", "26");

            Assert.Throws<ArgumentException>(() => bankVault.RemoveItem("A1", item));
        }
    }

}
