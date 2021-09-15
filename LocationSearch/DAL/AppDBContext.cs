using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LocationSearch.DBContext
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Note: Not recommended but enabled for debugging 
            // optionsBuilder.EnableDetailedErrors();
            // optionsBuilder.EnableSensitiveDataLogging();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        //    var assembly = Assembly.GetExecutingAssembly();
        //    using (var resourceStream = assembly.GetManifestResourceStream("LocationSearch.SeedData.locations.csv"))
        //    {
        //        using (var streamReader = new StreamReader(resourceStream, Encoding.UTF8))
        //        {
        //            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        //            {
        //                HasHeaderRecord = true,
        //                IgnoreBlankLines = true
        //            };

        //            using (var csvReader = new CsvReader(streamReader, csvConfiguration))
        //            {
        //                long id = 0;
        //                while(csvReader.Read())
        //                {
        //                    var csvRecord = csvReader.GetRecord<Model.CsvLocation>();

        //                }
        //                var csvRecordList = csvReader.GetRecords<Model.CsvLocation>();
        //                var spatialRecordList = csvRecordList.Select(r => new Model.Task(++id, r.Address, geometryFactory.CreatePoint(new Coordinate(r.Longitude, r.Latitude))));
        //                modelBuilder.Entity<Model.Task>().HasData(spatialRecordList.ToArray());
        //            }
        //        }
        //    }
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<Model.Task> Tasks { get; set; }
    }    
}
