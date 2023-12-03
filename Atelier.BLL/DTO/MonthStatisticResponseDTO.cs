namespace Atelier.BLL.DTO
{
    public class MonthStatisticResponseDTO
    {
        public string MonthName { get; set; }
        public int NewOrdersCount { get; set; }
        public int InProcessOrdersCount { get; set; }
        public int CompletedOrdersCount { get; set; }
    }
}
