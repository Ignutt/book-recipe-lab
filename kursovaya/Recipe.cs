using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemoffurniture
{
    class Recipe
    {
        private int _id;
        private string _name;
        private string _country;
        private int _difficult;

        public Recipe(int id, string name, string country, int difficult)
        {
            _id = id;
            _name = name;
            _country = country;
            _difficult = difficult;
        }

        public int ID 
        { 
            get => _id; 
            set => _id = value; 
        }

        public string Country 
        { 
            get => _country; 
            set => _country = value; 
        }

        public string Name 
        { 
            get => _name; 
            set => _name = value; 
        }

        public int Difficult 
        { 
            get => _difficult; 
            set => _difficult = value; 
        }
    }
}
