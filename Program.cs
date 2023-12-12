﻿using System;
using System.Collections.Generic;

namespace L43_shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int CommandVendorShowProducts = 1;
            const int CommandByProduct = 2;
            const int CommandShowInventory = 3;
            const int CommandExit = 4;

            Player player = new Player(200, 80);
            Vendor vendor = new Vendor();

            bool isOpen = true;

            ReceiveGoods(vendor);

            while (isOpen)
            {
                int numberMenu;

                Console.Clear();
                Console.WriteLine($"Добропожаловать в магазин!\n{CommandVendorShowProducts} - Посмотреть товары продовца.\n" +
                                  $"{CommandByProduct} - Купить товар.\n{CommandShowInventory} - Посмотреть свой инвентарь.\n" +
                                  $"{CommandExit} - Выйти из магазина.\n\nУ вас в наличии: {player.Money} монет.");
                Console.Write("Выбирете действие: ");

                if (int.TryParse(Console.ReadLine(), out numberMenu))
                {
                    Console.Clear();

                    switch (numberMenu)
                    {
                        case CommandVendorShowProducts:
                            vendor.ShowAllProducts();
                            break;

                        case CommandByProduct:
                            player.BuyProduct(vendor);
                            break;

                        case CommandShowInventory:
                            player.ShowInventory();
                            break;

                        case CommandExit:
                            isOpen = false;
                            continue;

                        default:
                            ShowError();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    ShowError();
                }

                Console.WriteLine("\nДля возвращение в меню, нажмите любую кнопку.");
                Console.ReadKey(true);
            }
        }

        static void ReceiveGoods(Vendor vendor)
        {
            vendor.AddProduct(new Product("Меч", 5, 30));
            vendor.AddProduct(new Product("Лук", 3, 35));
            vendor.AddProduct(new Product("Посох", 2, 55));
            vendor.AddProduct(new Product("Щит", 8, 40));
            vendor.AddProduct(new Product("Доспех", 15, 55));
            vendor.AddProduct(new Product("Зелье лечения", 1, 10));
        }

        static void ShowError()
        {
            Console.WriteLine("Вы ввели неизвестное значение.");
        }
    }

    class Player
    {
        private int _currentMass;
        private int _massLimit;

        private List<Product> _bag = new List<Product>();

        public Player(int money, int massLimit)
        {
            Money = money;
            _currentMass = 0;
            _massLimit = massLimit;
        }

        public int Money { get; private set; }

        public void BuyProduct(Vendor vendor)
        {
            Console.WriteLine("Введите наименование продукта: ");
            string nameProduct = Console.ReadLine();

            if (vendor.TryGetProductInfo(nameProduct, out Product product))
            {
                if ((product.Mass + _currentMass) <= _massLimit)
                {
                    if (Money >= product.Price)
                    {
                        vendor.SellProduct(product);
                        Money -= product.Price;
                        _currentMass += product.Mass;
                        _bag.Add(product);
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

        public void ShowInventory()
        {
            Console.WriteLine($"Общий вес предметов: {_currentMass}. Максимум сколько вы можете поднять: {_massLimit}");

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
        private List<Product> _storage = new List<Product>();

        public Vendor()
        {
            _money = 0;
        }

        public void AddProduct(Product product)
        {
            _storage.Add(product);
        }

        public void SellProduct(Product product)
        {
            _money += product.Price;
            _storage.Remove(product);
        }

        public void ShowAllProducts()
        {
            foreach (Product item in _storage)
                Console.WriteLine($"Наименование: {item.Name}\tВесс: {item.Mass}\tЦена: {item.Price}");
        }

        public bool TryGetProductInfo(string nameProduct, out Product product)
        {
            product = null;
            int indexProduct = GetIndexProduct(nameProduct);

            if (indexProduct >= 0)
            {
                product = _storage[indexProduct];
                return true;
            }

            return false;
        }

        private int GetIndexProduct(string nameProduct)
        {
            for (int i = 0; i < _storage.Count; i++)
                if (_storage[i].Name == nameProduct)
                    return i;

            return -1;
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