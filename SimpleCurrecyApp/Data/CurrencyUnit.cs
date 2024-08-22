using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCurrecyApp.Data
{
    internal class CurrencyUnit
    {
        private string _name;
        private decimal _rate;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public decimal Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        public CurrencyUnit(string name, decimal rate)
        {
            this._name = name;
            this._rate = rate;
        }
        public override string ToString()
        {
            return $"{_name} ({_rate})";
        }

       
    }
}
