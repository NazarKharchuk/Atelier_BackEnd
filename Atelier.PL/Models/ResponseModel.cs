namespace Atelier.PL.Models
{
    public class ResponseModel<T>
    {
        public T Data { get; set; }
        public bool Seccessfully { get; set; }
        public int? Code { get; set; }
        public string Message { get; set; }
    }
}
