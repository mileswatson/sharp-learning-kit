using System.IO;
using System.Xml.Serialization;

namespace SharpLearningKit
{
    class Loader
    {
        string filename;
        XmlSerializer fmtr;

        public Loader(string filename)
        {
            this.filename = filename;
            this.fmtr = new XmlSerializer(typeof(Model));
        }

        public Model Load()
        {
            FileStream rs = new FileStream(this.filename, FileMode.Open, FileAccess.Read);
            Model x = (Model) this.fmtr.Deserialize(rs);
            rs.Close();
            return x;
        }

        public void Save(Model x)
        {
            FileStream ws = new FileStream(this.filename, FileMode.Create, FileAccess.Write);
            this.fmtr.Serialize(ws, x);
            ws.Close();
        }

        public void Savex(Model x)
        {
            FileStream ws = new FileStream(this.filename, FileMode.Create, FileAccess.Write);
            XmlSerializer writer = new XmlSerializer(typeof(Model));
            writer.Serialize(ws, x);
            ws.Close();
        }
    }
}
