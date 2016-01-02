using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;


namespace YrdceClient
{
    public class XmlReader
    {
        XmlDocument doc = new XmlDocument();

        public string GetTabButtonText(int number)
        {
            doc.Load("Yrdce.xml");
            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList Nodes = rootElem.GetElementsByTagName("TabButton"); //获取person子节点集合  
             
            foreach (XmlNode node in Nodes)
            {
                XmlNodeList subName = ((XmlElement)node).GetElementsByTagName("Text");   //获取name属性值  
                string strName = subName[number].InnerText;
                return strName;
                //    for (int i =0; i < subName.Count; i++)
                //    {
                //        List<string> strName =null;
                //        strName.Add(subName[i].InnerText);
                            
                //        return strName.ToArray();
                //    }
                //return null;
            }
                
                return "读取出错";
                //XmlNodeList subAgeNodes = ((XmlElement)node).GetElementsByTagName("age");  //获取age子XmlElement集合  
                //if (subAgeNodes.Count == 1)  
                //{  
                //
                //Console.WriteLine(strAge);  
                //}  
            }
           
            //加载Xml文件  

        public int getNumber(int childBtnNumber) 
        { 
            doc.Load("Yrdce.xml");
            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList Nodes = rootElem.GetElementsByTagName("ChildButton" + childBtnNumber);
            foreach (XmlNode node in Nodes)
            {
                XmlNodeList subText = ((XmlElement)node).GetElementsByTagName("Text");   //获取name属性值  
                int number = subText.Count;
                return number;
            }
            return -1;
        }
        public string GetChildButtonValue(int number, int childBtnNumber,string value)
        {
            doc.Load("Yrdce.xml");
            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList Nodes = rootElem.GetElementsByTagName("ChildButton"+childBtnNumber); //获取person子节点集合  

            foreach (XmlNode node in Nodes)
            {
                XmlNodeList subName = ((XmlElement)node).GetElementsByTagName(value);   //获取name属性值  
                string strName = subName[number].InnerText;
                return strName;
            }
            return "读取出错";      
        }


        public string GetThirdButtonValue(int number, int childBtnNumber, string value)
        {
            doc.Load("Yrdce.xml");
            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList Nodes = rootElem.GetElementsByTagName("ThirdButton" + childBtnNumber); //获取person子节点集合  

            foreach (XmlNode node in Nodes)
            {
                XmlNodeList subName = ((XmlElement)node).GetElementsByTagName(value);   //获取name属性值  
                string strName = subName[number].InnerText;
                return strName;
            }
            return "读取出错";
        }


        public int getThirdNumber(int childBtnNumber)
        {
            doc.Load("Yrdce.xml");
            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList Nodes = rootElem.GetElementsByTagName("ThirdButton" + childBtnNumber);
            foreach (XmlNode node in Nodes)
            {
                XmlNodeList subText = ((XmlElement)node).GetElementsByTagName("Text");   //获取name属性值  
                int number = subText.Count;
                return number;
            }
            return -1;
        }
        //加载Xml文件  
        }
    }

