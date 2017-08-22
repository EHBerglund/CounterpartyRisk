using Microsoft.VisualStudio.TestTools.UnitTesting;
using CounterpartyRisk.Module.DTO;
using System.Collections.Generic;
using CounterpartyRisk.Module.Contracts;

//Testing av funksjon ved å tilegne alle parametre/variabler verdier og se om koden regner ut endelig resultat riktig. 

namespace CounterpartyRisk.Module.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            CounterpartyRiskInput input = new CounterpartyRiskInput();
            input.Contracts = new List<ContractDTO>();
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Reinsurance, Counterpart = new CounterpartDTO() { Name = "Name 1", Rating = 2 }, MarketValueOfCollateral = 100000, ReinsuranceShare = 755600, StandardRE = 200000, UseSimplifiedCalculation = false, UseCollateralAgreement = true });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Reinsurance, Counterpart = new CounterpartDTO() { Name = "Name 2", Rating = 3 }, MarketValueOfCollateral = 43000, ReinsuranceShare = 58300, UseSimplifiedCalculation = true, UseCollateralAgreement = false });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Reinsurance, Counterpart = new CounterpartDTO() { Name = "Name 3", Rating = 1 }, MarketValueOfCollateral = 25000, ReinsuranceShare = 36400, UseSimplifiedCalculation = true, UseCollateralAgreement = true });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Reinsurance, Counterpart = new CounterpartDTO() { Name = "Name 4", Rating = 8 }, MarketValueOfCollateral = 21000, ReinsuranceShare = 27500, StandardRE = 58000, UseSimplifiedCalculation = false, UseCollateralAgreement = false });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Reinsurance, Counterpart = new CounterpartDTO() { Name = "Name 5", Rating = 7 }, MarketValueOfCollateral = 14000, ReinsuranceShare = 21100, UseSimplifiedCalculation = true, UseCollateralAgreement = true });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Reinsurance, Counterpart = new CounterpartDTO() { Name = "Name 6", Rating = 9 }, MarketValueOfCollateral = 13000, ReinsuranceShare = 19400, UseSimplifiedCalculation = true, UseCollateralAgreement = false });

            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.CashDeposits, Counterpart = new CounterpartDTO() { Name = "Name 1", Rating = 2 }, MarketValueOfCollateral = 16483913 });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.CashDeposits, Counterpart = new CounterpartDTO() { Name = "Name 2", Rating = 3 }, MarketValueOfCollateral = 8131090});
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.CashDeposits, Counterpart = new CounterpartDTO() { Name = "Name 7", Rating = 1 }, MarketValueOfCollateral = 736756 });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.CashDeposits, Counterpart = new CounterpartDTO() { Name = "Name 8", Rating = 2 }, MarketValueOfCollateral = 236903 });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.CashDeposits, Counterpart = new CounterpartDTO() { Name = "Name 9", Rating = 3 }, MarketValueOfCollateral = 233175 });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.CashDeposits, Counterpart = new CounterpartDTO() { Name = "Name 10", Rating = 1 }, MarketValueOfCollateral = 148366 });

            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Derivative, Counterpart = new CounterpartDTO() { Name = "Name 1", Rating = 2 }, MarketValueNotRRC = 150000, MarketValueRRC = 1905592, MarketValueOfCollateral = 100000, StandardRE = 8955889 });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Derivative, Counterpart = new CounterpartDTO() { Name = "Name 7", Rating = 1 }, MarketValueNotRRC = 141000, MarketValueRRC = 5980778, MarketValueOfCollateral = 120000, StandardRE = 516852});
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Derivative, Counterpart = new CounterpartDTO() { Name = "Name 11", Rating = 6 }, MarketValueNotRRC = 151000000, MarketValueRRC = 799247, MarketValueOfCollateral = 130000, StandardRE = 2442621 });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Derivative, Counterpart = new CounterpartDTO() { Name = "Name 12", Rating = 5 }, MarketValueNotRRC = 15000, MarketValueRRC = 313280, MarketValueOfCollateral = 140000, StandardRE = 17513258 });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Derivative, Counterpart = new CounterpartDTO() { Name = "Name 13", Rating = 4 }, MarketValueNotRRC = 15100, MarketValueRRC = 295760, MarketValueOfCollateral = 150000, StandardRE = 29938221 });
            input.Contracts.Add(new ContractDTO() { ContractType = ContractType.Derivative, Counterpart = new CounterpartDTO() { Name = "Name 14", Rating = 0 }, MarketValueNotRRC = 10000, MarketValueRRC = 95309, MarketValueOfCollateral = 60000, StandardRE = 1389041 });

            Parameters parameters = new Parameters()
            {
                LossPotentialWithoutReinsurance = 2000000,
                LossPotentialWithReinsurance = 1500000,
                SumReinsurersShareSimplified = 135200,
                EconomicFactorCollateral = 0.375,
                EconomicFactorNonCollateral = 0.375,
                EconomicFactorDerivative = 0.375,
                SumTypeTwoExposure = 1000000,
                SumReceivableOverdue = 500000,
                MortgageValue = 525000,
            };

            input.Parameters = parameters;

            CounterpartyRisk risk = new CounterpartyRisk();
            CounterpartyRiskOutput output = risk.GetCounterpartyRisk(input);

            double TotalK = 153268358.287;
            double delta = GetDelta(TotalK);

            Assert.AreEqual(TotalK, output.TotalLossPotentialCounterPartyRisk, delta);
        }

        [TestMethod]
        public void TestMethod2()
        {
            ContractDTO cdto = new ContractDTO()
            {
                ContractType = ContractType.Reinsurance,
                Counterpart = new CounterpartDTO() { Name = "Name 1", Rating = 0 },
                ReinsuranceShare = 755600,
                StandardRE = 200000,
                MarketValueOfCollateral = 0,
                UseSimplifiedCalculation = false,
                UseCollateralAgreement = true
            };

            Parameters param = new Parameters()
            {
                LossPotentialWithoutReinsurance = 2000000,
                LossPotentialWithReinsurance = 1500000,
                SumReinsurersShareSimplified = 918300,
                EconomicFactorCollateral = 0.375,
                EconomicFactorNonCollateral = 0.375,
            };

            Contract c = new ReinsuranceContract(cdto, param);

            Assert.AreEqual(770040, c.CalculateLGD(), 5);
        }

        private double GetDelta(double totalAmount)
        {
            double precision = 0.0001;
            return precision * totalAmount;
        }
    }
}
