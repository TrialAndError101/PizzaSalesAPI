using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace PizzaSalesAPI.Models
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options)
            : base(options)
        {
        }

        public DbSet<SalesEntity> SalesEntities { get; set; }
    }

    public class SalesEntity
    {
        [Key]
        public int order_details_id { get; set; } = 0;
        public int order_id { get; set; } = 0;
        public string pizza_id { get; set; } = string.Empty;
        public int quantity { get; set; } = 0;
    }
}
