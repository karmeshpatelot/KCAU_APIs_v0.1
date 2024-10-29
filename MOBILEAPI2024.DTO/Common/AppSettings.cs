namespace MOBILEAPI2024.DTO.Common
{
    public class AppSettings
    {
        public string JWTTokenGenKey { get; set; }
        public string FCM_API_Path { get; set; }
        public string Server_API_Key { get; set; }
        public string Sender_API_ID { get; set; }
        public string APIUri { get; set; }
        public string Source { get; set; }
        public string ExitPath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public string DocPath { get; set; }
        public string ClientName { get; set; }
        public EmailServiceOptionsFirst EmailServiceOptionsFirst { get; set; }
        public EmailServiceOptionsSecond EmailServiceOptionsSecond { get; set; }
    }
}
