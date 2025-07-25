﻿namespace PCShop.Web.Infrastructure.Settings
{
    public class EmailSettings
    {
        public required string SmtpServer { get; set; } 
        public required int SmtpPort { get; set; }
        public required string SenderName { get; set; } 
        public required string SenderEmail { get; set; } 
        public required string Username { get; set; } 
        public required string Password { get; set; }
    }
}
