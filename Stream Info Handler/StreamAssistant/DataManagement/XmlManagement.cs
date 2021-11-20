using System.IO;
using System.Xml.Linq;

namespace Stream_Info_Handler.StreamAssistant.DataManagement
{
    class XmlManagement
    {
        /// <summary>
        /// Resets the XML file to have empty values for almost all fields.
        /// </summary>
        /// <param name="outputDirectory">The directory where the XML file is located</param>
        /// <param name="format">The game format. Use "Best of 3" or "Best of 5"</param>
        public static void ResetXml(string outputDirectory, string format)
        {
            //Deletes the existing output xml and creates an empty one in its place
            string xml_file = outputDirectory + @"\MasterOrdersOutput.xml";
            if (File.Exists(xml_file))
                File.Delete(xml_file);
            XDocument doc = new XDocument(
                new XElement("master-orders-output",
                new XElement("match",
                     new XElement("tournament", ""),
                     new XElement("bracketurl", ""),
                     new XElement("date", ""),
                     new XElement("round", ""),
                     new XElement("format", format),
                     new XElement("score1", ""),
                     new XElement("score-image1", outputDirectory + @"\score1.png"),
                     new XElement("score2", ""),
                     new XElement("score-image2", outputDirectory + @"\score2.png"),
                     new XElement("message-1", ""),
                     new XElement("message-2", "")
                     )
                ));
            for (int i = 1; i <= 4; i++)
                doc.Root.Add(new XElement("player-" + i.ToString(),
                     new XElement("tag", ""),
                     new XElement("twitter", ""),
                     new XElement("character-icon", outputDirectory + @"\stockicon" + i.ToString() + ".png"),
                     new XElement("sponsor-image", outputDirectory + @"\sponsor" + i.ToString() + ".png"),
                     new XElement("region-image", outputDirectory + @"\region" + i.ToString() + ".png"),
                     new XElement("base-tag", ""),
                     new XElement("full-name", ""),
                     new XElement("sponsor-full", ""),
                     new XElement("sponsor-prefix", ""),
                     new XElement("character-name", ""),
                     new XElement("region-name", ""),
                     new XElement("losers-side", "false")
                     ));
            doc.Root.Add(new XElement("commentator-1",
                 new XElement("tag", ""),
                 new XElement("sponsor-prefix", ""),
                 new XElement("twitter", "")
                 ));
            doc.Root.Add(new XElement("commentator-2",
                 new XElement("tag", ""),
                 new XElement("sponsor-prefix", ""),
                 new XElement("twitter", "")
                 ));

            doc.Save(xml_file);
        }
    }
}
