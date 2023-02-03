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
    public static class Init_data_user_management
    {

        public async static void Initialize()
        {
            var ob = new DbContextOptionsBuilder<model_user_managemen>();
            ob.UseNpgsql(AppGlobal.get_connection_string());

            using (var context = new model_user_managemen(ob.Options))
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    #region Organisasi
                    try
                    {


                        if (!context.m_organisasi.Any())
                        {
                            List<m_organisasi> m_organisasi = new List<m_organisasi>
                            {
                            new m_organisasi{
                                m_organisasi_id=1,
                                nama_oganisasi="PT Jasuindo Tiga Perkasa",
                                alamat_organisasi="Jalan Raya Betro No. 21",
                                email_organisasi="sales.sub@jasuindo.com",
                                npwp_organiasi="-",
                                telepon_organisasi="+62-31-8910919",
                                website_organisasi="https://jasuindo.com/",
                                path_logo_organiasi="-",
                                status_aktif = true},
                           
                            };
                            foreach (m_organisasi m_organisasi_data in m_organisasi) { context.m_organisasi.Add(m_organisasi_data); }
                            context.SaveChanges();
                           
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine(e);
                    }
                    #endregion

                    #region Project-Application
                    try
                    {


                        if (!context.m_project_application.Any())
                        {

                            List<m_project_application> m_project_application = new List<m_project_application>
                            {
                            new m_project_application{
                                m_project_application_id=1,
                                m_project_application_name ="Inventory Accounting",
                                key_project = "6E910CF0-BC18-4437-A55A-4DFE07801D0A",
                                status_aktif = true},
                            };

                            foreach (m_project_application m_project_application_data in m_project_application) { context.m_project_application.Add(m_project_application_data); }
                            context.SaveChanges();
                            
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine(e);
                    }
                    #endregion

                    #region Group-Feature
                    try
                    {


                        if (!context.m_group_feature.Any())
                        {

                            List<m_group_feature> m_group_feature = new List<m_group_feature>
                            {
                                new m_group_feature{
                                    m_group_feature_id=1,
                                    group_feature_name ="Master-User",
                                    index = 1,
                                    status = true
                                },
                                new m_group_feature
                                {
                                    m_group_feature_id = 2,
                                    group_feature_name = "Master-Data",
                                    index = 2,
                                    status = true
                                },
                                new m_group_feature
                                {
                                    m_group_feature_id = 3,
                                    group_feature_name = "Akses-User",
                                    index = 3,
                                    status = true
                                },
                                new m_group_feature
                                {
                                    m_group_feature_id = 4,
                                    group_feature_name = "Transasksi",
                                    index = 4,
                                    status = true
                                },
                                new m_group_feature
                                {
                                    m_group_feature_id = 5,
                                    group_feature_name = "Laporan",
                                    index = 5,
                                    status = true
                                }
                            };
                            foreach (m_group_feature m_group_feature_data in m_group_feature) { context.m_group_feature.Add(m_group_feature_data); }
                            context.SaveChanges();
                           
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
                            new m_feature{m_feature_id=1,m_feature_name="Organisasi",feature_icon="-",name_link_feature="-",index=1,status = true },
                            new m_feature{m_feature_id=2,m_feature_name="Aplikasi",feature_icon="-",name_link_feature="-",index=2,status = true},
                            new m_feature{m_feature_id=3,m_feature_name="lokasi",feature_icon="-",name_link_feature="-",index=3,status = true },
                            new m_feature{m_feature_id=4,m_feature_name="Feature Group ",feature_icon="-",name_link_feature="-",index=4,status = true },
                            new m_feature{m_feature_id=5,m_feature_name="Feature",feature_icon="-",name_link_feature="-",index=5,status = true},
                            new m_feature{m_feature_id=6,m_feature_name="Feature Action",feature_icon="-",name_link_feature="-",index=6,status = true },
                            new m_feature{m_feature_id=7,m_feature_name="Group User",feature_icon="-",name_link_feature="-",index=7,status = true },
                            };
                            foreach (m_feature m_feature_data in m_feature) { context.m_feature.Add(m_feature_data); }
                            context.SaveChanges();
                          
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine(e);
                    }
                    #endregion

                    #region Group-User
                    try
                    {


                        if (!context.m_group_user.Any())
                        {
                            List<m_group_user> m_group_user = new List<m_group_user>
                            {
                            new m_group_user{m_group_user_id=1,m_group_user_name="Administrator",status = true },
                           
                            };
                            foreach (m_group_user m_group_user_data in m_group_user) { context.m_group_user.Add(m_group_user_data); }
                            context.SaveChanges();
                           
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine(e);
                    }
                    #endregion

                    #region map_feature_group_project
                    try
                    {


                        if (!context.map_feature_group_project.Any())
                        {
                            List<map_feature_group_project> map_feature_group_project = new List<map_feature_group_project>
                            {
                            new map_feature_group_project{map_feature_group_project_id=Guid.NewGuid(), m_project_application_id=1,m_group_feature_id=1,m_feature_id=1,feature_icon="-",status_aktif=true},
                            new map_feature_group_project{map_feature_group_project_id=Guid.NewGuid(), m_project_application_id=1,m_group_feature_id=1,m_feature_id=2,feature_icon="-",status_aktif=true},
                            new map_feature_group_project{map_feature_group_project_id=Guid.NewGuid(), m_project_application_id=1,m_group_feature_id=1,m_feature_id=3,feature_icon="-",status_aktif=true},
                            new map_feature_group_project{map_feature_group_project_id=Guid.NewGuid(), m_project_application_id=1,m_group_feature_id=1,m_feature_id=4,feature_icon="-",status_aktif=true},
                            new map_feature_group_project{map_feature_group_project_id=Guid.NewGuid(), m_project_application_id=1,m_group_feature_id=1,m_feature_id=5,feature_icon="-",status_aktif=true},
                            new map_feature_group_project{map_feature_group_project_id=Guid.NewGuid(), m_project_application_id=1,m_group_feature_id=1,m_feature_id=6,feature_icon="-",status_aktif=true},
                            new map_feature_group_project{map_feature_group_project_id=Guid.NewGuid(), m_project_application_id=1,m_group_feature_id=1,m_feature_id=7,feature_icon="-",status_aktif=true},
                            };

                            var m_feature = context.m_feature.ToList();
                            if(m_feature.Count() != 0)
                            {
                                foreach(var item in m_feature)
                                {

                                }
                            }

                            foreach (map_feature_group_project map_feature_group_project_data in map_feature_group_project) { context.map_feature_group_project.Add(map_feature_group_project_data); }
                            await context.SaveChangesAsync();
                            
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine(e);
                    }
                    #endregion

                    #region Map-Feature-Group-User
                    try
                    {


                        if (!context.map_feature_group.Any())
                        {
                            var map_feature_group_project = context.map_feature_group_project.Where(e=> e.m_project_application_id == 1).ToList();
                            List<map_feature_group> map_feature_group = new List<map_feature_group>();
                            foreach (var item in map_feature_group_project)
                            {
                                map_feature_group item_data = new map_feature_group()
                                {
                                    map_feature_group_id = Guid.NewGuid(),
                                    map_feature_group_project_id = item.map_feature_group_project_id,
                                    m_group_user_id = 1,
                                    action_feature = "-"
                                };

                                context.map_feature_group.Add(item_data);
                            }
                                 
                            context.SaveChanges();
                          
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