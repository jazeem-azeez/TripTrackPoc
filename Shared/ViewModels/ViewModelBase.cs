namespace Shared.ViewModels
{
    public class ViewModel<T>
    {
        public ViewModel()
        {

        }
        public ViewModel(T data)
        {
            this.Data = data;
        }
        public string CorrelationId { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusCode { get; set; }
        public string Count { get; set; }
        public T Data { get; set; }
    }
}