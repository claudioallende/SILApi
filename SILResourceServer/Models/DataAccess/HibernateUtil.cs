using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using ResourceServer.Models.Error;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class HibernateUtil
    {
        private static ISessionFactory SessionFactorySingleton;
        //private static Configuration ConfiguracionSingleton;
        private static IPathProvider pathProvider = new ServerPathProvider();

        public static ISession OpenSession()
        {
            SessionFactorySingleton = GetSessionFactorySingleton();
            return GetSession(GetSessionFactorySingleton());
        }

        public static ISession OpenSession(string mappingFile)
        {
            SessionFactorySingleton = GetSessionFactorySingleton();
            return GetSession(GetSessionFactorySingleton());
        }

        public static ISession OpenSession(IList<string> mappingFiles)
        {
            SessionFactorySingleton = GetSessionFactorySingleton();
            return GetSession(GetSessionFactorySingleton());
        }

        public static Configuration GetConfiguration()
        {
            var configuration = new Configuration();
            var configurationPath = GetMappingFile(@"~\Models\nhibernate.cfg.xml");
            configuration.Configure(configurationPath);
            configuration.CurrentSessionContext<WebSessionContext>();
            AddMappingToConfiguration(configuration);
            return configuration;
        }

        public static void AddMappingToConfiguration(Configuration Config)
        {
            var files = Directory.GetFiles(GetMappingFile(@"~\Mappings\"), "*.mpg.xml");
            foreach (var file in files)
            {
                Config.AddFile(file);
            }
        }

        public static ISessionFactory GetSessionFactorySingleton()
        {
            try
            {
                if (SessionFactorySingleton == null)
                {
                    SessionFactorySingleton = GetConfiguration().BuildSessionFactory();
                }
                return SessionFactorySingleton;
            }
            catch (Exception e)
            {
                ErrorLog.Write(e);
                throw e;
            }
        }

        public static string GetMappingFile(string FileName)
        {
            return pathProvider.MapPath(FileName);
        }

        public static ISession GetSession(ISessionFactory Factory)
        {
            try
            {
                ISession session;
                if (CurrentSessionContext.HasBind(Factory))
                {
                    session = Factory.GetCurrentSession();
                    if (!session.IsOpen)
                    {
                        HibernateUtil.Dispose();
                        session = Factory.OpenSession();
                        CurrentSessionContext.Bind(session);
                    }
                }
                else
                {
                    session = Factory.OpenSession();
                    CurrentSessionContext.Bind(session);
                }
                return session;
            }
            catch (Exception e)
            {
                ErrorLog.Write(e);
                throw e;
            }
        }

        public static void Dispose()
        {
            if (SessionFactorySingleton != null && CurrentSessionContext.HasBind(GetSessionFactorySingleton()))
            {
                var session = CurrentSessionContext.Unbind(GetSessionFactorySingleton());
                session.Dispose();
            }
        }

        public static void Refresh<T>(T objeto, ISession Session)
        {
            Session.Refresh(objeto);
        }

        public static void RefreshBeforeUpdate<T>(T objeto, ISession Session)
        {
            Session.Refresh(objeto, LockMode.Upgrade);
        }

        public static void Evict<T>(T objeto, ISession Session)
        {
            Session.Evict(objeto);
        }

        public static void SetPathProvider(IPathProvider provider) {
            pathProvider = provider;
        }
    }
}