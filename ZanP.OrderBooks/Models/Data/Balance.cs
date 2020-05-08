using System;

namespace ZanP.OrderBooks.Models.Data
{
    public class Balance
    {
        private static decimal m_EUR_BTC = 0.00011M; // fixed value, 1 EUR = 0.00011 BTC, as it was on May 8

        public Balance(decimal p_eur)
        {
            EUR = p_eur;
            BTC = p_eur * m_EUR_BTC;
        }

        public void DecreaseEUR(decimal p_val)
        {
            decimal toSet = p_val;

            if(EUR - p_val < 0)
            {
               toSet = Math.Round(EUR - p_val, 3);
            }

            EUR -= toSet;
        }

        public void DecreaseBTC(decimal p_val)
        {
            BTC -= p_val;
        }

        public void IncreaseEUR(decimal p_val)
        {
            EUR += p_val;
        }

        public void IncreaseBTC(decimal p_val)
        {
            BTC += p_val;
        }

        public decimal BTC { get; private set; }
        public decimal EUR { get; private set; }
    }
}