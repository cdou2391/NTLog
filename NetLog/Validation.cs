using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLog
{
    class Validation
    {
        public bool validation(string name, string surname, string email, string number, string address, string floorNum,
                                 string building, string computerType, string make, string model, string serial)

        {
            string aName = name;
            string sName = surname;
            string aEmail = email;
            string phoneNo = number;
            string addr = address;
            string numFloor = floorNum;
            string build = building;
            string comType = computerType;
            string mak = make;
            string mod = model;
            string ser = serial;
            bool errorMessage = true;
            

            if ((aName == "") || (sName == "") || (aEmail == "") || (build == "") || (mak == "") || (mod == "") || (ser == "") ||
                (addr == "") || (phoneNo == "") || (numFloor == "") || (comType == ""))
            {

                errorMessage = false;

            }
            else
            {
                errorMessage = true;
            }

            return errorMessage;

        }
    }
}
