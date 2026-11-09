namespace CY_WebApi.Models
{
    public class FileUploadModel
    {
        public required IFormFile File { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }

    }
}
