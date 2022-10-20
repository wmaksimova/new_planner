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
        public int count_of_plan { get; set; }
        public string check_plan { get; set; }
    }
    public class trackerModel
    {
        [Key]
        public int rel_tracker_day_id { get; set; }
        public int water { get; set; }
        public int mood { get; set; }
        public int sleep { get; set; }
        public int steps { get; set; }
    }
    public class ConnectBD: DbContext
    {
        public DbSet<dayModel> days { get; set; }
        public DbSet<planModel> plans { get; set; }
        public DbSet<trackerModel> tracker { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("workstation id=baseplaner.mssql.somee.com;packet size=4096;user id=xdashie_SQLLogin_1;pwd=juo9s5krmv;data source=baseplaner.mssql.somee.com;persist security info=False;initial catalog=baseplaner");
        }
    }
}
