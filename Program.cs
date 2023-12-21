using System;
using System.Collections.Generic;

namespace L43_shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            int playerMaxMoney = 300;
            int playerMassLimit = 80;

            Shop shop = new Shop();
            Player player = new Player(random.Next(playerMaxMoney), playerMassLimit);

            shop.Run(player);
        }
    }

    class Error
    {
        public static void Show()
        {
            Console.WriteLine("Вы ввели некоректное значение.");
        }
    }

    class Shop
    {
        private const int CommandVendorShowProducts = 1;
        private const int CommandPlayerByProduct = 2;
        private const int CommandPlayerShowInventory = 3;
        private const int CommandExit = 4;

        private Vendor _vendor;

        public Shop()
        {
            _vendor = new Vendor();
            _vendor.FillInventory();
        }

        public void Run(Player player)
        {
            bool isOpen = true;

            while (isOpen)
            {
                Console.Clear();
                Console.WriteLine($"Добропожаловать в магазин!\n{CommandVendorShowProducts} - Посмотреть товары продовца.\n" +
                                  $"{CommandPlayerByProduct} - Купить товар.\n{CommandPlayerShowInventory} - Посмотреть " +
                                  $"свой инвентарь.\n{CommandExit} - Выйти из магазина.\n");
                Console.Write("Выбирете действие: ");

                if (int.TryParse(Console.ReadLine(), out int numberMenu))
                {
                    Console.Clear();

                    switch (numberMenu)
                    {
                        case CommandVendorShowProducts:
                            _vendor.ShowInventory();
                            break;

                        case CommandPlayerByProduct:
                            Trade(player);
                            break;

                        case CommandPlayerShowInventory:
                            player.ShowInventory();
                            break;

                        case CommandExit:
                            isOpen = false;
                            continue;

                        default:
                            Error.Show();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Error.Show();
                }

                Console.WriteLine("\nДля возвращение в меню, нажмите любую кнопку.");
                Console.ReadKey(true);
            }
        }

        private void Trade(Player player)
        {
            Console.WriteLine("Введите наименование продукта: ");
            string nameProduct = Console.ReadLine();

            if (_vendor.TryGetProductIndex(nameProduct, out int productIndex))
            {
                int productMass = _vendor.GetProductMass(productIndex);
                int productPrice = _vendor.GetProductPrice(productIndex);

                if (player.IsEnoughWeightCapacity(productMass))
                {
                    if (player.IsEnoughMoney(productPrice))
                    {
                        player.BuyProduct(_vendor.SellProduct(productIndex));
                    }
                    else
                    {
                        Console.WriteLine("У вас недостаточно денег для покупки.");
                    }
                }
                else
                {
                    Console.WriteLine("Вам не хватает сил нести больше вещей.");
                }
            }
            else
            {
                Console.WriteLine("У продавца нет такого товара.");
            }
        }
    }

    class Character
    {
        protected List<Product> Inventory = new List<Product>();
        protected int Money;

        public Character(int money)
        {
            Money = money;
        }

        public virtual void ShowInventory()
        {
            foreach (var item in Inventory)
                Console.WriteLine($"Наименование: {item.Name}\tВесс: {item.Mass}");
        }
    }

    class Player : Character
    {
        private int _massLimit;
        private int _currentMass;

        public Player(int money, int massLimit) : base(money)
        {
            _currentMass = 0;
            _massLimit = massLimit;
        }

        public bool IsEnoughMoney(int needMoney)
        {
            return (Money >= needMoney);
        }

        public bool IsEnoughWeightCapacity(int objectWeight)
        {
            return (_massLimit >= (_currentMass + objectWeight));
        }

        public void BuyProduct(Product product)
        {
            Money -= product.Price;
            _currentMass += product.Mass;
            Inventory.Add(product);
        }

        public override void ShowInventory()
        {
            Console.WriteLine($"У вас в наличии: {Money} монет.");
            Console.WriteLine($"Общий вес предметов: {_currentMass}. Максимум сколько вы можете поднять: {_massLimit}\n");

            if (Inventory.Count > 0)
                base.ShowInventory();
            else
                Console.WriteLine("У вас нет предметов в инвентаре.");
        }
    }

    class Vendor : Character
    {
        public Vendor(int money = 0) : base(money) { }

        public Product SellProduct(int productIndex)
        {
            Product soldProduct = Inventory[productIndex];
            Inventory.RemoveAt(productIndex);
            Money += soldProduct.Price;

            return soldProduct;
        }

        public override void ShowInventory()
        {
            foreach (var item in Inventory)
                Console.WriteLine($"Наименование: {item.Name}\tВесс: {item.Mass}\tЦена: {item.Price}");
        }

        public int GetProductPrice(int productIndex)
        {
            return Inventory[productIndex].Price;
        }

        public int GetProductMass(int productId)
        {
            return Inventory[productId].Mass;
        }

        public bool TryGetProductIndex(string nameProduct, out int productIndex)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].Name == nameProduct)
                {
                    productIndex = i;
                    return true;
                }
            }

            productIndex = -1;
            return false;
        }

        public void FillInventory()
        {
            Inventory.Add(new Product("Меч", 5, 30));
            Inventory.Add(new Product("Лук", 3, 35));
            Inventory.Add(new Product("Посох", 2, 55));
            Inventory.Add(new Product("Щит", 8, 40));
            Inventory.Add(new Product("Доспех", 15, 55));
            Inventory.Add(new Product("Зелье лечения", 1, 10));
        }
    }

    class Product
    {
        public Product(string name, int mass, int price)
        {
            Name = name;
            Mass = mass;
            Price = price;
        }

        public string Name { get; private set; }

        public int Mass { get; private set; }

        public int Price { get; private set; }
    }
}