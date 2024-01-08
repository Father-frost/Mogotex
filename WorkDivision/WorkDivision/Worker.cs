using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkDivision
{
    public class Worker
    {
        public int _id;
        public string _name;
        public string _description;
        public int _rank;
        public bool _isDriver;

        public bool CanBeTheBoss(int rank) 
        {
            if ((rank > 3) && (rank < 10))
            {
                return true;
            }
            else if ((rank > 10))
            {
                throw new ArgumentException();
            }
            else { 
                return false;
            }
        
        }

        public int GetBonusPercent(int rank) 
        {
            switch (rank)
            {
                case 1: return 10;
                case 2: return 20;
                case 3: return 30;
                default: return 0;
            }
            
        }
    }

}
