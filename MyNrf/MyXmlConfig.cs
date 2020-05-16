using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;  


namespace MyNrf
{
    public class XmlInfo//配置文件格式  
    {
        public string Name;
        public string Value;
        public XmlInfo(string Name, string Value) 
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
    public class MyXmlConfig
    {
        public List<XmlInfo> MyXml = new List<XmlInfo>();
        System.Configuration.Configuration config=null ;

        public MyXmlConfig()//默认初始化时自动读取配置文件  若没有则创建配置文件
        {
            config = ConfigurationManager.OpenExeConfiguration("MyNrf.exe");
            
            ConnectionStringsSection conSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            if (conSection.ConnectionStrings["配置文件"] != null)
            {
                conSection.ConnectionStrings["配置文件"].ConnectionString = "第十三届恩智浦智能车";
                conSection.ConnectionStrings["配置文件"].ProviderName = "杭电智能车";
            }
            else 
            {
                conSection.ConnectionStrings.Add(new ConnectionStringSettings("配置文件", "第十三届恩智浦智能车", "杭电智能车"));
            }

            MyXml.RemoveRange(0, MyXml.Count);
            try
            {
                MyXml.Add(new XmlInfo("幽魂", GetValue("幽魂")));              
            }
            catch 
            {
                ;
            }
            if (MyXml[0].Value == "")
            {
                XmlClear();
                SetValue(MyXml[0].Name, MyXml[0].Value);
                return;
            }
            ReadXml(); 

        }
        /// <summary>
        /// 删除一个appsettings节点
        /// </summary>
        /// <param name="key"></param>
        public void RemoveAppSettingsSection(string key)
        {
            if (config.AppSettings.Settings[key] != null) 
            {
                config.AppSettings.Settings.Remove(key);
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("MyXmlSettingFie");//重新加载新的配置文件   
            }
        }
        public void WriteXml(XmlInfo XmlValue, bool Flag) 
        {
            SetValue(XmlValue.Name, XmlValue.Value);
            bool Add_Flag = false;
            string stmp = MyXml[0].Value;
            while (stmp != "")
            {
                if (stmp.Substring(0, stmp.IndexOf('|')).Equals(XmlValue.Name)==true) 
                {
                    
                    Add_Flag = true;
                    break;
                }
                stmp = stmp.Substring(stmp.IndexOf('|') + 1);
            }
            
            if (Flag == true)
            {
                if (Add_Flag == false)
                {
                    MyXml[0].Value += XmlValue.Name + "|";
                    SetValue(MyXml[0].Name, MyXml[0].Value);
                }
            }
            else 
            {
                return;
            }
        }
        public void ReadXml() 
        {
            string stmp=MyXml[0].Value;
            List<string> xmltmp = new List<string>();
            while (stmp != "" )
            {
                xmltmp.Add(stmp.Substring(0, stmp.IndexOf('|')));
                stmp = stmp.Substring(stmp.IndexOf('|')+1);
            }
            for (int i = 0; i < xmltmp.Count; i++) 
            {
                MyXml.Add(new XmlInfo(xmltmp[i], GetValue(xmltmp[i])));
            }
        }
        public void XmlClear()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration("MyNrf.exe");
            config.ConnectionStrings.ConnectionStrings.Clear();
        }
        /// <summary>  
        /// 写入值  
        /// </summary>  
        /// <param name="Xmlkey"></param>  
        /// <param name="value"></param>  
        public  void SetValue(string Xmlkey, string value)
        {
            //增加的内容写在appSettings段下 <add key="RegCode" value="0"/>  
            if (config.AppSettings.Settings[Xmlkey] == null)//检索配置文件
            {
                config.AppSettings.Settings.Add(Xmlkey, value);
            }
            else
            {
                config.AppSettings.Settings[Xmlkey].Value = value;
            }
            config.Save();
            ConfigurationManager.RefreshSection("MyXmlSettingFie");//重新加载新的配置文件   
        }

        /// <summary>  
        /// 读取指定key的值  
        /// </summary>  
        /// <param name="Xmlkey"></param>  
        /// <returns></returns>  
        public  string GetValue(string Xmlkey)
        {

            if (config.AppSettings.Settings[Xmlkey] == null)
                return "";
            else
                return config.AppSettings.Settings[Xmlkey].Value;
        }

    }





}
