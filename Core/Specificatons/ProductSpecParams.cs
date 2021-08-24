namespace Core.Specificatons
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        private int _PageSize = 6;
        public int PageIndex { get; set; } = 1;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }
        private string _Search { get; set; }
        public string Search
        {
            get => _Search;
            set => _Search = value.ToLower();
        }
    }
}