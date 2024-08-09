using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Banking
{
    public class BankingOperations : IBanking
    {
        #region Private Fields

        private List<Customer> customers = new List<Customer>();
        private bool isInitialized = false;

        #endregion Private Fields

        #region Public Methods

        public double CalculateFees(long giverId, long beneficiaryId, double amount)
        {
            var giver = customers.FirstOrDefault(x => x.Id == giverId);
            var beneficiary = customers.FirstOrDefault(x => x.Id == beneficiaryId);

            if (giver == null || beneficiary == null)
                throw new InvalidOperationException("Cliente no encontrado");

            if (!giver.IsInternal)
                throw new InvalidOperationException("El donante debe ser un cliente interno");

            if (giver.IsInternal && beneficiary.IsInternal)
            {
                return (amount >= 100 ? 5 : 0);
            }
            else if (giver.IsInternal && !beneficiary.IsInternal)
            {
                return 10;
            }
            else
            {
                throw new InvalidOperationException("Transferencia no es posible desde un proveedor externo");
            }
        }

        public long CountCustomers()
        {
            return customers.Count;
        }

        public void Initialise(string path)
        {
            if (isInitialized)
                throw new InvalidOperationException("El archivo ya ha sido inicializado");

            var lines = File.ReadAllLines(path);

            if (lines.Length == 0)
                throw new InvalidOperationException("El archivo csv se encuentra vacio");

            var headers = lines[0].Split(';');
            if (headers.Length != 15)
                throw new InvalidOperationException("El archivo csv no coincide con el formato esperado");

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(';');

                var customer = new Customer()
                {
                    Id = int.Parse(values[0]),
                    FirstName = values[1],
                    LastName = values[2],
                    Phone1 = values[9],
                    Phone2 = values[10],
                    IsInternal = values[13].ToLower() == "internal",
                    Balance = double.Parse(values[14])
                };

                customers.Add(customer);
            }

            isInitialized = true;
        }

        public Customer[] SearchCustomers(long? id = null, string name = null)
        {
            if (id.HasValue)
            {
                return customers.Where(x => x.Id == id.Value).ToArray();
            }
            else if (!string.IsNullOrEmpty(name))
            {
                return customers.Where(x => x.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        x.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
            }
            else
            {
                return customers.ToArray();
            }
        }

        public void Transfer(long giverId, long beneficiaryId, double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero");

            var giver = customers.FirstOrDefault(x => x.Id == giverId);
            var beneficiary = customers.FirstOrDefault(x => x.Id == beneficiaryId);

            if (giver == null || beneficiary == null)
                throw new InvalidOperationException("Cliente no encontrado");

            if (!giver.IsInternal)
                throw new InvalidOperationException("El donante debe ser un cliente interno");

            double fees = CalculateFees(giverId, beneficiaryId, amount);

            if (giver.Balance < amount + fees)
                throw new InvalidOperationException("Balance insuficiente");

            giver.Balance -= amount + fees;
            beneficiary.Balance += amount;
        }

        #endregion Public Methods
    }
}