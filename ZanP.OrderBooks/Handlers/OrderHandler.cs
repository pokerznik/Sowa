using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models.Data;
using System.Collections.Generic;
using ZanP.OrderBooks.Models;

namespace ZanP.OrderBooks.Handlers
{
    /// <summary>
    /// Responsible for making orders and setting best order price
    /// </summary>
    public class OrderHandler 
    {
        private OrderType m_type;
        private double m_amount;
        private DataHandler m_dataHandler;
        private List<Exchange> m_exchanges;
        private string m_path;

        public OrderHandler(string p_path = "data")
        {
            m_path = p_path;
            GetExchanges();
        }

        private void GetExchanges()
        {
            m_dataHandler = new DataHandler(m_path);
            m_exchanges = m_dataHandler.GetExchanges();
        }

        public void Process(Order p_order)
        {
            
        }
    }
}