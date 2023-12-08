using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace L43_shop
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class Player    // Дописать
    {
        private int _money;
        private int _massLimit;
        private List<Product> _bag = new List<Product>();

        public Player(int money, int massLimit) // Дописать
        {
            _money = money;
            _massLimit = massLimit;
        }

        public void BuyProduct()            // Дописать
        {

        }

        public void ShowInventory()     // Дописать
        {

        }
    }

    class Vendor    // Дописать
    {
        private int _money;
        private List<Product> _storage = new List<Product>();

        public Vendor()
        {
            _money = 0;
        }

        public void AddProduct()    // Дописать
        {

        }
        public void SellProduct()  // Дописать. Поиск ближайшего продукта по имени
        {

        }

        public void ShowProducts()   // Дописать
        {

        }
    }

    class Product
    {
        private string _name;
        private int _mass;

        public Product(string name, int mass)
        {
            _name = name;
            _mass = mass;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int Mass
        {
            get
            {
                return _mass;
            }
        }
    }
}
