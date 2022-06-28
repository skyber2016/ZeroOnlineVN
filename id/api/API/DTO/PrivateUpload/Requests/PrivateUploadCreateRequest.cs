using Microsoft.AspNetCore.Http;

namespace API.DTO.PrivateUpload.Requests
{
    public class PrivateUploadCreateRequest
    {
        public IFormFile File { get; set; }
        public string SecretKey { get; set; }
        public bool IsValid()
        {
            return this.SecretKey == "QWERTY";
        }
    }

    public class PrivateUploadBytesCreateRequest
    {
        public string Base64String { get; set; }
        public long FileLength { get; set; }
        public string FileName { get; set; }
        public string SecretKey { get; set; }
        public bool IsValid()
        {
            return this.SecretKey == "QWERTY";
        }
    }
}
