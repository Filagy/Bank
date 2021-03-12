using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);
    public class AccountEventArgs 
    {
        public string Message { get; private set; } // сообщение
        public decimal Sum { get; private set; } // сумма на которую изменился счет
        public AccountEventArgs(string _mes, decimal _sum)
        {
            Message = _mes; Sum = _sum;
        }
    }
}
