﻿namespace UserManagementServices.Services.EmailService
{
    public class EmailConfiguration
    {
        public string? From { get; set; }
        public string? SmtpServer { get; set; }
        public string? Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
