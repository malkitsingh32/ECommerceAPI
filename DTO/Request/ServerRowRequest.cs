namespace DTO.Request
{
    public class ServerRowRequest
    {
        public int StartRow { set; get; }
        public int EndRow { set; get; }
        public dynamic FilterModel { get; set; }
        public List<SortModel> SortModel { set; get; }
        public ServerRowRequest()
        {
            SortModel = new List<SortModel>();
        }
    }
}
