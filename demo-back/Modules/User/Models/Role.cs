using System;
using System.Collections.Generic;

namespace back.Models;

public  class  Role
{
    public int Id { get; set; }

    public string Title { get; set; }
    public DateTime Created_at { get; set; } = DateTime.UtcNow;

    public DateTime Updated_at { get; set; } = DateTime.UtcNow;
    public virtual ICollection<User> Users { get; } = new List<User>();
}
