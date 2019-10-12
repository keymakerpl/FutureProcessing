using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class PeselValidator : IPeselValidator
    {
        private string _pesel;
        private DateTime _date;

        public string Pesel { get { return _pesel; } }

        public void ChangePesel(string pesel)
        {
            _pesel = pesel;
        }

        public PeselStatuses CheckChars()
        {
            try
            {
                Int64.Parse(_pesel);
            }
            catch (FormatException)
            {
                return PeselStatuses.InvalidChars;
            }
            return PeselStatuses.Valid;
        }

        public PeselStatuses CheckLength()
        {
            if (_pesel.Length != 11)
                return PeselStatuses.InvalidLength;
            return PeselStatuses.Valid;
        }

        public PeselStatuses CheckCheckSum()
        {
            char[] sCtrl = _pesel.ToCharArray();
            Int64 sum = 0;

            int number = int.Parse(sCtrl[0].ToString()) * 1;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[1].ToString()) * 3;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[2].ToString()) * 7;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[3].ToString()) * 9;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[4].ToString()) * 1;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[5].ToString()) * 3;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[6].ToString()) * 7;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[7].ToString()) * 9;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[8].ToString()) * 1;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            number = int.Parse(sCtrl[9].ToString()) * 3;
            if (number > 9) number = int.Parse(number.ToString().Substring(1, 1));
            sum += number;

            if (10 - Int32.Parse(sum.ToString().Substring(sum.ToString().Length - 1, 1)) == 10 && int.Parse(sCtrl[10].ToString()) == 0) return PeselStatuses.Valid;
            else if (10 - Int32.Parse(sum.ToString().Substring(sum.ToString().Length - 1, 1)) != int.Parse(sCtrl[10].ToString())) return PeselStatuses.InvalidCheckSum;
            else return PeselStatuses.Valid;
        }

        public PeselStatuses CheckDate()
        {
            int year = 0;
            int month = 0;
            int day = 0;
            DateTime date;

            Int32 year34 = Int32.Parse(_pesel.Substring(0, 2));

            Int32 myear = Int32.Parse(_pesel.Substring(2, 2));

            day = Int32.Parse(_pesel.Substring(4, 2));

            if (myear < 13)
            {
                month = myear;
                year = year34 + 1900;
            }
            else if (myear > 20 && myear < 33)
            {
                month = myear - 20;
                year = year34 + 2000;
            }
            else if (myear > 40 && myear < 53)
            {
                month = myear - 40;
                year = year34 + 2100;
            }
            else if (myear > 60 && myear < 73)
            {
                month = myear - 60;
                year = year34 + 2200;
            }
            else
            {
                return PeselStatuses.InvalidDate;
            }

            try
            {
                date = new DateTime(year, month, day);
            }

            catch (Exception)
            {
                return PeselStatuses.InvalidDate;
            }

            _date = date;

            return PeselStatuses.Valid;
        }

        public PeselStatuses CheckSeries()
        {
            char[] sCtrl = _pesel.ToCharArray();

            if (sCtrl[6] == '0' && sCtrl[7] == '0' && sCtrl[8] == '0' && sCtrl[9] == '0')
                return PeselStatuses.InvalidSeries;
            else return PeselStatuses.Valid;
        }

        public PeselStatuses Verify()
        {
            if (CheckChars() == PeselStatuses.InvalidChars) return PeselStatuses.InvalidChars;
            else if (CheckLength() == PeselStatuses.InvalidLength) return PeselStatuses.InvalidLength;
            else if (CheckCheckSum() == PeselStatuses.InvalidCheckSum) return PeselStatuses.InvalidCheckSum;
            else if (CheckDate() == PeselStatuses.InvalidDate) return PeselStatuses.InvalidDate;

            else return PeselStatuses.Valid;
        }

        public DateTime GetBirthDate()
        {
            if (Verify() == PeselStatuses.Valid)
            {
                return _date;
            }
            else throw new Exception("Incorrect PESEL");
        }
    }

    public enum PeselStatuses
    {
        InvalidChars,
        InvalidLength,
        InvalidCheckSum,
        InvalidDate,
        Valid,
        InvalidSeries
    }
}
