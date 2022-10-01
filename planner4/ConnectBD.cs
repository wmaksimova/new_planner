using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace planner4
{
    public class DataBD
    {
        public DateTime data { get; set; }
        public string Plan1 { get; set; }
        public string Plan2 { get; set; }
        public string Plan3 { get; set; }
        public string Plan4 { get; set; }
        public string Plan5 { get; set; }
        public string Plan6 { get; set; }
        public string Plan7 { get; set; }
        public int CountOfPlan { get; set; }
        public float Water { get; set; }
        public float Mood { get; set; }
        public float Sleep { get; set; }
        public float Steps { get; set; }
        public string Motivation { get; set; }
    }
    public class ConnectBD: DbContext
    {
        public DbSet<DataBD> AppBD { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("workstation id=baseplaner.mssql.somee.com;packet size=4096;user id=xdashie_SQLLogin_1;pwd=juo9s5krmv;data source=baseplaner.mssql.somee.com;persist security info=False;initial catalog=baseplaner");
        }
    }
}
