using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP255FinalProject
{
    class AccountTransaction
    {
        public AccountTransaction(int TransactionNumber,
                                      int AccountNumber,
                                      DateTime TransactionDate,
                                      decimal TransactionAmount)

        {
            this.TransactionNumber = TransactionNumber;
            this.AccountNumber = AccountNumber;
            this.TransactionDate = TransactionDate;
            this.TransactionAmount = TransactionAmount;

        }

        public int TransactionNumber { get; set; }
        public int AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }

        public override string ToString() => $"{TransactionNumber} {TransactionDate} {TransactionAmount}";
    }
}