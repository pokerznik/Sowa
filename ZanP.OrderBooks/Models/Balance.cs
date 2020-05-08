namespace ZanP.OrderBooks.Models
{
    public class Balance
    {
        private double m_EUR_BTC = .00011; // fixed value, 1 EUR = 0.00011 BTC, as it was on May 8

        public Balance(double p_eur)
        {
            EUR = p_eur;
            BTC = p_eur * m_EUR_BTC;
        }

        public double BTC { get; private set; }
        public double EUR { get; private set; }
    }
}