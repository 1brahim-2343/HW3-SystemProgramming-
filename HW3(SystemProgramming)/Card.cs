using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW3_SystemProgramming_
{
    public class Card
    {
        private string pan;

        public string Pan
        {
            get { return pan; }
            set
            {
                if (value.Length == 16 || value.Length == 19)
                {
                    if (value.Contains('-'))
                    {
                        pan = value.Replace("-", "");
                    }
                    else
                    {
                        pan = value;
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid pan");
                }

            }
        }

        public string? OwnerName { get; set; }
        public decimal Balance { get; set; }
        private string? pin;

        public string? Pin
        {
            get { return pin; }
            set
            {
                if (value != null && value.Length == 4)
                {
                    pin = value;
                }
                else
                {
                    throw new ArgumentException("Invalid pin");
                }
            }
        }
        public Card(string pan, string name, decimal balance, string pin)
        {
            Pan = pan;
            OwnerName = name;
            Balance = balance;
            Pin = pin;
        }


    }
}
