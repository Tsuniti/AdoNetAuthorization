using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationFormWPF.Entities;


public class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public DateTime RegisteredAt { get; set; }

    public string RealName { get; set; }

    public string Email { get; set; }

    public uint Age { get; set; }


    public override string ToString()
    {
        return $"User #{Id} | {Username} | Real name: {RealName} | Email: {Email} | Age: {Age} | Registered at {RegisteredAt}";
    }

}