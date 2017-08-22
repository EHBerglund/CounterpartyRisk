using CounterpartyRisk.Module.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Her skjer utregning av individuelle kontrakters LGD og LGDSq og RE (x1). Så her blir det masse arbeid (woohoo). 
//Dette er altså mellomregninger før CounterpartyRiskCalculator gjør sin ting. 


namespace CounterpartyRisk.Module.Contracts
{
    public abstract class Contract
    {
        protected CounterpartDTO counterpart;
        protected Parameters Parameters;
        protected double LGD;

        public Contract(ContractDTO contract, Parameters Parameters) 
        {
            this.counterpart = contract.Counterpart;
            this.Parameters = Parameters;
            this.LGD = -1;
        }
        //Dette (over) er kontruktøren til klassen. Denne angir ikke hvilken returverdi den har, men har samme navn som klassen.
        public int Rating
        {
            get
            {
                return counterpart.Rating;
            }
        }
        public virtual double CalculateLGD()
        {
            return 0;
        }

        public virtual double CalculateLGDSquared()
        {
            return 0;
        }

        public virtual double GetLGD()
        {
            if (LGD == -1)
            {
                return CalculateLGD();
            }
            else
            {
                return LGD;
            }
        }

        public virtual string GetCounterpartName()
        {
            return counterpart.Name;
        }

        public virtual void AddLGDToCounterpart()
        {
            this.counterpart.TotalLGD += GetLGD();
        }

        public virtual CounterpartDTO GetCounterpart()
        {
            return this.counterpart;
        }
    }

    public class CashDepositContract : Contract
    {
        public CashDepositContract(ContractDTO contract, Parameters Parameters)
            : base(contract, Parameters)
        {
            LGD = contract.MarketValueOfCollateral;
        }

        public override double CalculateLGD()
        {
            return LGD;
        }

        public override double CalculateLGDSquared()
        {
            return LGD * LGD; 
        }
    }

    public class DerivativeContract : Contract
    {
        private double MarketValueNotRRC;
        private double MarketValueRRC;
        private double MarketValueCollateral;
        private double StandardRE;

        public DerivativeContract(ContractDTO contract, Parameters Parameters) 
            : base(contract, Parameters)
        {
            MarketValueNotRRC = contract.MarketValueNotRRC;
            MarketValueRRC = contract.MarketValueRRC;
            MarketValueCollateral = contract.MarketValueOfCollateral;
            StandardRE = contract.StandardRE;
        }

        public override double CalculateLGD()
        {
            double MarketValue = MarketValueNotRRC + MarketValueRRC + StandardRE;
            double Percent90MV = 0.9 * MarketValue;
            double MarketValueCollateralFactorReducing = MarketValueCollateral * Parameters.EconomicFactorDerivative;
            double SumInsideMaxFunction = Percent90MV - MarketValueCollateralFactorReducing;
            LGD = Math.Max(SumInsideMaxFunction, 0);
            return LGD;
        }
        public override double CalculateLGDSquared()
        {
            LGD = CalculateLGD();
            return LGD * LGD;
        }
    }
    public class ReinsuranceContract : Contract
    { 
        private double ReinsurersShare;
        private double LossPotentialWithoutReinsurance;
        private double LossPotentialWithReinsurance;
        private double SumReinsurersShareSimplified;
        private double RiskReducingEffectStandard;
        private double MarketValueCollateral;
        private double CollateralFactorOver60Percent;
        private double CollateralFactorUnder60Percent;
        private bool UseSimplifiedCalculation;
        private bool UseCollateralAgreement;

        public ReinsuranceContract(ContractDTO contract, Parameters Parameters) 
            : base(contract, Parameters)
        {
            ReinsurersShare = contract.ReinsuranceShare;
            LossPotentialWithoutReinsurance = Parameters.LossPotentialWithoutReinsurance;
            LossPotentialWithReinsurance = Parameters.LossPotentialWithReinsurance;
            SumReinsurersShareSimplified = Parameters.SumReinsurersShareSimplified;
            RiskReducingEffectStandard = contract.StandardRE;
            MarketValueCollateral = contract.MarketValueOfCollateral;
            CollateralFactorOver60Percent = Parameters.EconomicFactorCollateral;
            CollateralFactorUnder60Percent = Parameters.EconomicFactorNonCollateral;
            UseSimplifiedCalculation = contract.UseSimplifiedCalculation;
            UseCollateralAgreement = contract.UseCollateralAgreement;
        }

        private double GetRE() 
        {

            double RE;

            if (UseSimplifiedCalculation)
            {
                RE = CalculateRESimplified();
            }
            else
            {
                RE = RiskReducingEffectStandard;
            }
            return RE;
        }
        

        private double CalculateRESimplified()
        {
            double TotalLossPotential = LossPotentialWithoutReinsurance - LossPotentialWithReinsurance;
            double ReinsurerShareShare = ReinsurersShare / SumReinsurersShareSimplified;
            double RRES = TotalLossPotential * ReinsurerShareShare;
            return RRES;
        }

        private double GetCollateralFactor()
        {
            double CF;

            if (UseCollateralAgreement)
            {
                CF = 0.9;
            }
            else
            {
                CF = 0.5;
            }
            return CF;
        }

        public override double CalculateLGD() 
        {
            double REmult50Percent = 0.5 * GetRE();
            double ReSharePlusRE = ReinsurersShare + REmult50Percent;
            double ReSharePlusREmult90Percent = GetCollateralFactor() * ReSharePlusRE;
            double MarketvalueFactorRRES = MarketValueCollateral * CollateralFactorOver60Percent;
            LGD = ReSharePlusREmult90Percent - MarketvalueFactorRRES;
            return LGD;
        }

        public override double CalculateLGDSquared()
        {
            LGD = GetLGD();
            return LGD * LGD;
        }
    }


}
