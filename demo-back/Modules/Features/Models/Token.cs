using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models;

public class Token
{
    public string encryptedToken { get; set; }
}
