namespace Shopping.API.DTO
{
    public class ResponseDto
    {
        public bool  IsSuccess { get; set; } = true;
        public string? Message { get; set; } = "Success";
        public List<string>? Error { get; set; } = new List<string>();
        public object? Result { get; set; }
    }
}
