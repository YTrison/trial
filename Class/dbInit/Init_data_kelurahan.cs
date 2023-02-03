using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations;
//template namespace
using appglobal;
using System.IO;
using jasuindo_models;

namespace appglobal
{
    public static class Init_data_kelurahan
    {

        public static void Initialize()
        {
            var ob = new DbContextOptionsBuilder<model_user_managemen>();
            ob.UseNpgsql(AppGlobal.get_connection_string());

            using (var context = new model_user_managemen(ob.Options))
            {

                using (var transaction = context.Database.BeginTransaction())
                {

                    #region m_kelurahan_desa
                    try
                    {

                        var data_kelurahan = context.m_kelurahan_desa.ToList();
                        if (data_kelurahan.Count == 0)
                        {
                            string final_location = @"Models/data/" + "m_kelurahan_desa.dat";
                            string json_list = System.IO.File.ReadAllText(final_location);

                            List<m_kelurahan_desa> m_kelurahan_desa = JsonConvert.DeserializeObject<List<m_kelurahan_desa>>(json_list);

                            foreach (m_kelurahan_desa m_kelurahan_desa_data in m_kelurahan_desa)
                            {
                                context.m_kelurahan_desa.Add(m_kelurahan_desa_data);

                            }
                            context.SaveChanges();
                            transaction.Commit();
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine(e);
                    }
                    #endregion

                }

            }
        }
    }
}