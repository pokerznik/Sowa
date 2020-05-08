namespace ZanP.OrderBooks.Models.Data
{
    public class Balance
    {
        private static double m_EUR_BTC = .00011; // fixed value, 1 EUR = 0.00011 BTC, as it was on May 8

        public Balance(double p_eur)
        {
            EUR = p_eur;
            BTC = p_eur * m_EUR_BTC;
        }

        /// <summary>
        /// Convert Bitcoins quantity to EUR
        /// </summary>
        /// <param name="p_btc">Quantity of Bitcoins</param>
        /// <returns></returns>
        public static double ConvertBTC_EUR(double p_btc)
        {
            return p_btc / m_EUR_BTC;
        }

        public void DecreaseEUR(double p_val)
        {
            EUR -= p_val;
        }

        public void DecreaseBTC(double p_val)
        {
            BTC -= p_val;
        }

        public void IncreaseEUR(double p_val)
        {
            EUR += p_val;
        }

        public void IncreaseBTC(double p_val)
        {
            BTC += p_val;
        }

        public double BTC { get; private set; }
        public double EUR { get; private set; }
    }
}