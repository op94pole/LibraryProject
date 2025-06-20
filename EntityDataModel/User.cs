﻿namespace EntityDataModel
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public enum UserRole { User, Admin };
    }
}