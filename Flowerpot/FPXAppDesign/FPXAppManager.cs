using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using FPXAppDesign.DesignerClass;
using FPXApplicationInterface.Interface;
using System.Data.Sql;

namespace FPXAppDesign
{
    public class FPXAppManager
    {
        public UserApp GetUserApp(string userAppPath = @"Flowerpot\FPXProcessorUI\Xml\1_UserApp_20131022.xml")
        {
            //const string path = @"C:\Wen's Assignments\Flowerpot\Flowerpot\FPXProcessorUI\Xml\UserApp.xml";
            var path = System.Environment.CurrentDirectory;
            var userApp = DeserializeUserApp(path);
            return userApp;
        }

        /// <summary>
        /// Get UserApp xml from DB and translate into UserApp
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IUserApp GetUserApp(int userId, string name) 
        {
            IUserApp app = null;
            if(name!=null)
            {
                //XmlDocument doc = GetAppFromDB(userId, name);
                //app = DeserializeUserApp(doc);
            }
            return app;
        }

        /// <summary>
        /// Save UserApp into DB
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appName"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public bool SaveUserAppToDB(int userId, string appName,IUserApp app)
        {
            var result = false;
            if (app != null) 
            {
                var doc = InstanceToXml(app);
                result = SaveAppToDB(userId, appName, app);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appName"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public bool UpdateUserAppToDB(int userId, string appName,IUserApp app)
        {
            var result = false;
            if (app != null) 
            {
                var doc = InstanceToXml(app);
                result = UpdateAppToDB(userId, appName, app);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        public bool DeleteUserAppFromDB(int userId, string appName)
        {
            var result = false;
            if (appName != null)
            {
                result = DeleteAppToDB(userId, appName);
            }
            return result;
        }

        public static UserApp DeserializeUserApp(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            var serializer = new XmlSerializer(typeof(UserApp));
            var reader = new StreamReader(path);

            //MemoryStream stream = new MemoryStream();
            //XmlDocument xml = new XmlDocument();
            //xml.Load(path);
            //xml.Save(stream);
            //stream.Seek(0, SeekOrigin.Begin);

            var userApp = serializer.Deserialize(reader) as UserApp;
            return userApp;
        }

        public XmlDocument InstanceToXml(IUserApp userApp)
        {
            var xmlDoc = new XmlDocument();
            var serializer = new XmlSerializer(typeof(IUserApp));
            var stream = new MemoryStream();

            serializer.Serialize(stream, userApp);
            stream.Position = 0;
            xmlDoc.Load(stream);
            return xmlDoc;
        }

        private XmlDocument GetAppFromDB(int userId, string appName) 
        {
            //string connection = @".\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true";
            return null;
        }

        private bool SaveAppToDB(int userId,string appName, IUserApp app) 
        {
            return false;
        }

        private bool UpdateAppToDB(int userId, string appName, IUserApp app)
        {
            return false;
        }

        private bool DeleteAppToDB(int userId, string appName)
        {
            return false;
        }
    }
}
