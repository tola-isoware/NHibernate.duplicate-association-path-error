using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using TestProject.Model;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            App_Start.NHibernateProfilerBootstrapper.PreStart();

            var cfg = new Configuration();
            cfg.DataBaseIntegration(properties =>
            {
                properties.Dialect<SQLiteDialect>();
                properties.ConnectionString = @"Data Source=:memory:";
            });
            cfg.AddAssembly(typeof(Machine).Assembly);

			var sessionFactory = cfg.BuildSessionFactory();

			using (var con = new SQLiteConnection("Data Source=:memory:"))
			{
				con.Open();

				new SchemaExport(cfg).Execute(false, true, false, con, null);

                // Preparing Data
				using (var session = sessionFactory.OpenSession(con))
				using (var tx = session.BeginTransaction())
				{


				    var pt1 = new PropertyType { Type = "color" };
				    var pt2 = new PropertyType { Type = "temperature" };
				    var pt3 = new PropertyType { Type = "weigh" };

				    var m1 = new Machine { Name = "Machine 1", PropertyValues = new HashSet<PropertyValue>() };
                    var m2 = new Machine { Name = "Machine 2", PropertyValues = new HashSet<PropertyValue>() };
                    var m3 = new Machine { Name = "Machine 3", PropertyValues = new HashSet<PropertyValue>() };

				    var pv1 = new PropertyValue
				    {
				        Value = "blue",
				        PropertyType = pt1,
				        Machine = m1
				    };
                    m1.PropertyValues.Add(pv1);

                    var pv2 = new PropertyValue
                    {
                        Value = "green",
                        PropertyType = pt1,
                        Machine = m2
                    };
                    m2.PropertyValues.Add(pv2);

                    var pv3 = new PropertyValue
                    {
                        Value = "red",
                        PropertyType = pt1,
                        Machine = m3
                    };
                    m3.PropertyValues.Add(pv3);

				    var pv4 = new PropertyValue
				    {
				        Value = "120°C",
				        PropertyType = pt2,
				        Machine = m1
				    };
                    m1.PropertyValues.Add(pv4);

				    var pv5 = new PropertyValue
				    {
				        Value = "150kg",
				        PropertyType = pt3,
				        Machine = m3
				    };
                    m3.PropertyValues.Add(pv5);

				    session.Save(pt1);
				    session.Save(pt2);
				    session.Save(pt3);

                    session.Save(m1);
                    session.Save(m2);
                    session.Save(m3);

                    session.Save(pv1);
                    session.Save(pv2);
                    session.Save(pv3);
                    session.Save(pv4);
                    session.Save(pv4);

                    

					tx.Commit();
				}

                // Querying data
				using (var session = sessionFactory.OpenSession(con))
				using (var tx = session.BeginTransaction())
				{
				    var query = session.Query<Machine>().FetchMany(m => m.PropertyValues).ToList();

				    foreach (var machine in query)
				    {
				        Console.WriteLine("Machine.Name: {0}",machine.Name);
				        foreach (var propertyValue in machine.PropertyValues)
				        {
				            Console.WriteLine("  {0}",propertyValue.Value);
				        }
				    }


				    var list = session.CreateQuery("select m.Name, pv1.Value, pv2.Value from Machine m " +
				                                   "left outer join m.PropertyValues pv1 left outer join pv1.PropertyType pt1 " +
				                                   "left outer join m.PropertyValues pv2 left outer join pv2.PropertyType pt2 " +
				                                   "where pt1.Type=:type1 and pt2.Type=:type2")
				        .SetParameter("type1", "color")
				        .SetParameter("type2", "temperature")
				        .List();

				    foreach (object[] item in list)
				    {
				        foreach (var i in item)
				        {
				            Console.Write("{0}, ", i == null ? "null" : i.ToString());
				        }
                        Console.WriteLine();
				    }

                    //var list =
                    //    session.CreateQuery("from Machine m " + 
                    //                            "join PropertyValue pv1 " + 
                    //                            "join pv1.PropertyType pt1 " + 
                    //                            "where pt1.Type=:type ")
                    //        .SetParameter("type", "color").List();
				}

                Console.WriteLine("done.");
			    Console.ReadLine();
			}
		}


    }
}

