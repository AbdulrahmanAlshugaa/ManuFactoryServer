using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Edex.DAL.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class SystemSettings
    {
       /// <summary>
       /// Add comments to the following function
       /// </summary>
       /// <param name="Path"></param>
       /// <returns></returns>   
        public static string GetSkinName(string Path)
        {
            // Create a new instance of the XmlDocument class
            XmlDocument xmlDoc = new XmlDocument();

            // Append the file name "Skin.xml" to the input path to get the full path to the skin data file
            Path = Path + @"\DataXml\Skin.xml";

            // Load the skin data file into the XmlDocument object
            xmlDoc.Load(Path);

            // Loop through each element in the root node of the document
            foreach (XmlElement element in xmlDoc.DocumentElement)
            {
                // Check if the element is an "appSettings" element
                if (element.Name.Equals("appSettings"))
                {
                    // Loop through each child node of the "appSettings" element
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        // Return the value of the first attribute of the first child node
                        return node.Attributes[0].Value;

                    }
                }
            }

            // If no skin name is found, return "Black" as the default skin name
            return "Black";
        }
      /// <summary>
      /// Add comments to the following function
      /// </summary>
      /// <param name="SkinName"></param>
      /// <param name="Path"></param>   
        public static void SetSkinName(string SkinName, string Path)
        {
            // Create a new instance of the XmlDocument class
            XmlDocument xmlDoc = new XmlDocument();

            // Append the file name "Skin.xml" to the input path to get the full path to the skin data file
            Path = Path + @"\DataXml\Skin.xml";

            // Load the skin data file into the XmlDocument object
            xmlDoc.Load(Path);

            // Loop through each element in the root node of the document
            foreach (XmlElement element in xmlDoc.DocumentElement)
            {
                // Check if the element is an "appSettings" element
                if (element.Name.Equals("appSettings"))
                {
                    // Loop through each child node of the "appSettings" element
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        // Set the value of the first attribute of the first child node to the specified skin name
                        node.Attributes[0].Value = SkinName;
                    }
                }
            }
            // Save the changes made to the skin data file
            xmlDoc.Save(Path);
        }
  
    }
}
