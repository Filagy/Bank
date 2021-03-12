using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        // События
        protected internal event AccountStateHandler Withdrawed; // Вывод денег
        protected internal event AccountStateHandler Added; // Добавление денег
        protected internal event AccountStateHandler Opened; // Открытие счета
        protected internal event AccountStateHandler Closed; // Закрытие счета
        protected internal event AccountStateHandler Calculated; // Начисление процентов

        static int counter = 0;
        protected int _days = 0; // Время с момента открытия счета

        public decimal Sum { get; private set; } // текущая сумма на счету
        public int Persentage { get; private set; } // процент начисления
        public int Id { get; private set; } // Уникальный индификатор счета

        public Account(decimal sum, int persentage)
        {
            Sum = sum; Persentage = persentage; Id = ++counter;
        }
        // Вызов события
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null)
                handler?.Invoke(this, e);
        }

        // Вызов отдельных событий. Для каждого события определяется свой виртуальный метод
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        public virtual void Put(decimal sum)
        {
            Sum += sum;
            OnAdded(new AccountEventArgs("На счет поступило " + sum, sum));
        }

        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (Sum >= sum)
            {
                Sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs($"Сумма {sum} снята со счета {Id}", 0));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостаточно денег на счете {Id}", 0));
            }
            return result;
        }
        // Открытие счета.
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Открыт новый счет! Id счета: {Id}", Sum));
        }
        // Закрытие счета.
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Счет {Id} закрыт. Итоговая сумма: {Sum}", Sum));
        }
        protected internal void IncrementDays()
        {
            _days++;
        }
        // Начисление процентов
        protected internal virtual void Calculate()
        {
            decimal increment = Sum * Persentage / 100;
            Sum = Sum + increment;
            OnCalculated(new AccountEventArgs($"Начислены проценты в размере: {increment}", increment));
        }
    }
}
