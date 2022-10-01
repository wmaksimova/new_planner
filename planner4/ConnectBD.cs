using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace planner4
{
    
    public class dayModel
    {
        [Key]
        public int id { get; set; }
        public DateTime date { get; set; }
    }
    public class planModel
    {
        [Key]
        public int id { get; set; }
        public int rel_day_id { get; set; }
        public string plan { get; set; }
        public int plan_position { get; set; }
    }
    public class ConnectBD: DbContext
    {
        public DbSet<dayModel> days { get; set; }
        public DbSet<planModel> plans { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("workstation id=baseplaner.mssql.somee.com;packet size=4096;user id=xdashie_SQLLogin_1;pwd=juo9s5krmv;data source=baseplaner.mssql.somee.com;persist security info=False;initial catalog=baseplaner");
        }
    }
}
