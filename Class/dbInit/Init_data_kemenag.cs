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
    public static class Init_data_kemenag
    {

        public static void Initialize()
        {
            var ob = new DbContextOptionsBuilder<model_user_managemen>();
            ob.UseNpgsql(AppGlobal.get_connection_string());

            using (var context = new model_user_managemen(ob.Options))
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    #region group_user
                    try
                    {


                        if (!context.m_group_user.Any())
                        {
                            List<m_group_user> m_group_user = new List<m_group_user>
                            {
                            new m_group_user{m_group_user_id=1,m_group_user_name="kemenag",status = true},
                            new m_group_user{m_group_user_id=2,m_group_user_name="madrasah",status = true},
                            };
                            foreach (m_group_user m_group_user_data in m_group_user) { context.m_group_user.Add(m_group_user_data); }
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

                    #region m_feature
                    try
                    {


                        if (!context.m_feature.Any())
                        {
                            List<m_feature> m_feature = new List<m_feature>
                            {
                            new m_feature{m_feature_id=1,m_feature_name="Madrasah",status = true },
                            new m_feature{m_feature_id=2,m_feature_name="Registrasi Ijasah",status = true},
                            new m_feature{m_feature_id=3,m_feature_name="Registrasi Ulang Ijasah",status = true },
                            };
                            foreach (m_feature m_feature_data in m_feature) { context.m_feature.Add(m_feature_data); }
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

                    //#region map_feature_group
                    //try
                    //{


                    //    if (!context.map_feature_group.Any())
                    //    {
                    //        List<map_feature_group> map_feature_group = new List<map_feature_group>
                    //        {
                    //        new map_feature_group{map_feature_group_id=1, m_group_user_id=1,m_feature_id=1},
                    //        new map_feature_group{map_feature_group_id=2, m_group_user_id=2,m_feature_id=2},
                    //        new map_feature_group{map_feature_group_id=3, m_group_user_id=2,m_feature_id=3},
                    //        };
                    //        foreach (map_feature_group map_feature_group_data in map_feature_group) { context.map_feature_group.Add(map_feature_group_data); }
                    //        context.SaveChanges();
                    //        transaction.Commit();
                    //    }

                    //}
                    //catch (Exception e)
                    //{
                    //    transaction.Rollback();
                    //    Console.WriteLine(e);
                    //}
                    //#endregion
                }
            }
        }
    }
}