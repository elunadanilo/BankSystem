using Banking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BankingUnitTest
{
    [TestClass]
    public class BankingTest
    {
        private const string path = "C:/Users/elunadanilo/Desktop/dotnet-technical-test/customers.csv";
        private BankingOperations operations;

        [TestInitialize]
        public void Setup()
        {
            operations = new BankingOperations();
            operations.Initialise(path);
        }

        [TestMethod]
        public void BankingDoubleInitialization()
        {
            var ex = Assert.ThrowsException<InvalidOperationException>(() => operations.Initialise(path));
            Assert.AreEqual("El archivo ya ha sido inicializado", ex.Message);
        }

        [TestMethod]
        public void SearchCustomerByName()
        {
            var result = operations.SearchCustomers(name: "meaghan");
            Assert.AreEqual(1, result.Length);
        }

        [TestMethod]
        public void FeeBothCustomerInternalAmount50()
        {
        }

        [TestMethod]
        public void FeeBothCustomerInternalAmount100()
        {
        }

        [TestMethod]
        public void FeeInternalGiverExternalBeneficiaryAmount75()
        {
        }



        [TestMethod]
        public void TransferExternalGiver()
        {
            var ex = Assert.ThrowsException<InvalidOperationException>(() => operations.Transfer(88,89,10));
            Assert.AreEqual("El donante debe ser un cliente interno", ex.Message);
        }


        [TestMethod]
        public void TransferNegativeAmount()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => operations.Transfer(1, 2, -5));
            Assert.AreEqual("El monto debe ser mayor a cero", ex.Message);
        }

        [TestMethod]
        public void TransferExceedBalance()
        {
            var ex = Assert.ThrowsException<InvalidOperationException>(() => operations.Transfer(1,2,500));
            Assert.AreEqual("Balance insuficiente", ex.Message);
        }


        [TestMethod]
        public void TransferInternalGiverExternalBeneficiary()
        {
            var ex = Assert.ThrowsException<InvalidOperationException>(() => operations.Transfer(90, 2, 500));
            Assert.AreEqual("El donante debe ser un cliente interno", ex.Message);
        }

        [TestMethod]
        public void TransferBothCustomerInternal()
        {
        }

    }
}
