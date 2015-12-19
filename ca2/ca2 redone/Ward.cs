using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA2_14_15_exam
{
    public enum BloodType
    {
        //creating blood type values
        A, B, AB, O
    }

    public class Ward
    {
        private string _wardName;
        public string WardName { get { return _wardName; } set { _wardName = value; } }

        private static int _limit;
        public int Limit { get { return _limit; } set { _limit = value; } }

        public override string ToString()
        {
            return string.Format("{0}\t(Limit: {1})", _wardName, _limit);
        }


        public Ward(string theName, int theLimit)
        {
            Limit = theLimit;
            WardName = theName;
            
        }

    }


    public class Patient
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; } }
        private DateTime _dob;
        public DateTime Dob { get { return _dob; } set { _dob = value; } }
        private BloodType _bType;
        public BloodType bType { get { return _bType; } set { _bType = value; } }

        /// <summary>
        /// try and attach patient to the ward by adding ward (WardName) to the patient
        /// Mainly to use filter
        /// </summary>
        private string _pWard;
        public string pWardName { get { return _pWard; } set { _pWard = value; } }

        public Patient(string pName, string date , BloodType blt , string pWard)
        {
            Name = pName;
            DateTime.TryParse(date, out _dob);
            bType = blt;
            pWardName = pWard;            
        }

        public override string ToString()
        {
            return string.Format("{0} ({1:d})\t{2}", Name, Dob, bType);
        }

    }
}
