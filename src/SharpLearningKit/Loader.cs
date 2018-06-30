using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharpLearningKit
{
    class Loader
    {
        string filename;
        BinaryFormatter fmtr;

        public Loader(string filename)
        {
            this.filename = filename;
            this.fmtr = new BinaryFormatter();
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
    }
}