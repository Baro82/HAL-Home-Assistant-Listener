using System.Data;

namespace HAL {

    public partial class HALMain {

        private DataSet ds;

        private void ReadDatabase() {
            ds = new DataSet();
            ds.ReadXml("data.xml");
            dg.ItemsSource = new DataView(ds.Tables[0]);
        }

        private void WriteDatabase() {
            ds.Tables[0].WriteXml("data.xml", XmlWriteMode.WriteSchema);
        }

    }


}
