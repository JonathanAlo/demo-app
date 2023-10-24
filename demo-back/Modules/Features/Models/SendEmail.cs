using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models;

public class SendEmail
{

    public string Subject { get; set; }
    public string NameTo { get; set; }
    public string To { get; set; }
    public string User { get; set; }
    public string Url { get; set; }
    public string Button { get; set; }
    public string Message { get; set; }
  
}
