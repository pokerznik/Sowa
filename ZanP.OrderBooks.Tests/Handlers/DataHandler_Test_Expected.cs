using Xunit;
using ZanP.OrderBooks.Handlers;

namespace ZanP.OrderBooks.Tests.Handlers
{
    public class DataHandler_Test_Expected
    {
        private DataHandler m_dataHandler;

        public DataHandler_Test_Expected()
        {
            m_dataHandler = new DataHandler("data");
        }

        [Fact]
        public void GetExchanges_Valid_NotNull()
        {
            Assert.NotNull(m_dataHandler.GetExchanges());
        }

        [Fact]
        public void GetExchanges_NotEmpty_True()
        {
            var exchanges = m_dataHandler.GetExchanges();
            Assert.NotEmpty(exchanges);
        }
    }
}