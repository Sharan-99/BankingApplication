using System;

namespace Utilities
{
    public class BankingAppException : Exception
    {
        public BankingAppException()
        {

        }

        public BankingAppException(string message): base(message)
        {

        }
    }
}
