﻿namespace PoC.DistributedAspNetIdentity.Web.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }
}