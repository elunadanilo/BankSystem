using Banking;
using System;

namespace BankingSystem
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            BankingOperations bankingOperations = new BankingOperations();
            try
            {
                string csvFilePath = "C:/Users/elunadanilo/Desktop/dotnet-technical-test/customers.csv";
                bankingOperations.Initialise(csvFilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Private Methods
    }
}