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

        Vendor _vendor;

        public Shop()
        {
            _vendor = new Vendor();
            _vendor.ReceiveGoods();
        }

        public void Run(Player player)
        {
            bool isOpen = true;

            while (isOpen)
            {
                Console.Clear();
                Console.WriteLine($"Добропожаловать в магазин!\n{CommandVendorShowProducts} - Посмотреть товары продовца.\n" +
                                  $"{CommandPlayerByProduct} - Купить товар.\n{CommandPlayerShowInventory} - Посмотреть свой инвентарь.\n" +
                                  $"{CommandExit} - Выйти из магазина.\n\nУ вас в наличии: {player.Money} монет.");
                Console.Write("Выбирете действие: ");

                if (int.TryParse(Console.ReadLine(), out int numberMenu))
                {
                    Console.Clear();

                    switch (numberMenu)
                    {
                        case CommandVendorShowProducts:
                            _vendor.ShowAllProducts();
                            break;

                        case CommandPlayerByProduct:
                            SellGoods(player);
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

        private void SellGoods(Player player)
        {
            Console.WriteLine("Введите наименование продукта: ");
            string nameProduct = Console.ReadLine();

            if (_vendor.TryGetProductId(nameProduct, out int productId))
            {
                if ((_vendor.GetProductMass(productId) + player.CurrentMass) <= player.MassLimit)
                {
                    if (player.Money >= _vendor.GetProductPrice(productId))
                        player.BuyProduct(_vendor.SellProduct(productId));
                    else
                        Console.WriteLine("У вас недостаточно денег для покупки.");
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

    class Player
    {
        private List<Product> _bag = new List<Product>();

        public Player(int money, int massLimit)
        {
            Money = money;
            CurrentMass = 0;
            MassLimit = massLimit;
        }

        public int Money { get; private set; }

        public int MassLimit { get; private set; }

        public int CurrentMass { get; private set; }

        public void BuyProduct(Product product)
        {
            Money -= product.Price;
            CurrentMass += product.Mass;
            _bag.Add(product);
        }

        public void ShowInventory()
        {
            Console.WriteLine($"Общий вес предметов: {CurrentMass}. Максимум сколько вы можете поднять: {MassLimit}");

            if (_bag.Count > 0)
                foreach (Product item in _bag)
                    Console.WriteLine($"Наименовкание: {item.Name}\t Вес:{item.Mass}");
            else
                Console.WriteLine("У вас нет предметов в инвентаре.");
        }
    }

    class Vendor
    {
        private int _money;
        private int _productId;
        private Dictionary<int, Product> _storage = new Dictionary<int, Product>();

        public Vendor()
        {
            _productId = 0;
            _money = 0;
        }

        public Product SellProduct(int productId)
        {
            Product soldProduct = _storage[productId];
            _storage.Remove(productId);
            _money += soldProduct.Price;

            return soldProduct;
        }

        public void ShowAllProducts()
        {
            foreach (var item in _storage)
                Console.WriteLine($"Наименование: {item.Value.Name}\tВесс: {item.Value.Mass}\tЦена: {item.Value.Price}");
        }

        public int GetProductPrice(int productId)
        {
            return _storage[productId].Price;
        }

        public int GetProductMass(int productId)
        {
            return _storage[productId].Mass;
        }

        public bool TryGetProductId(string nameProduct, out int productId)
        {
            foreach (var item in _storage)
                if (item.Value.Name == nameProduct)
                {
                    productId = item.Key;
                    return true;
                }

            productId = -1;
            return false;
        }

        public void ReceiveGoods()
        {
            _storage.Add(_productId++, new Product("Меч", 5, 30));
            _storage.Add(_productId++, new Product("Лук", 3, 35));
            _storage.Add(_productId++, new Product("Посох", 2, 55));
            _storage.Add(_productId++, new Product("Щит", 8, 40));
            _storage.Add(_productId++, new Product("Доспех", 15, 55));
            _storage.Add(_productId++, new Product("Зелье лечения", 1, 10));
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