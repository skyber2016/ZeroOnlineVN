using HexEditor.Structures;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HexEditor.Services
{
    internal class RobotTypeService
    {
        private readonly OpenFileDialog OpenDialogService = new OpenFileDialog();
        private readonly SaveFileDialog SaveDialogService = new SaveFileDialog();
        public void ImportDat()
        {
            this.OpenDialogService.FileName = "robottype.dat";
            this.OpenDialogService.Filter = "All files (*.dat) | *.dat";
            if (this.OpenDialogService.ShowDialog() == DialogResult.OK)
            {
                var ext = Path.GetExtension(this.OpenDialogService.FileName);
                if (!".dat".Equals(ext))
                {
                    MessageBox.Show("Vui lòng chọn đúng file robottype.dat", "Thông báo!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.OpenDialogService.Reset();
                    return;
                }
                var stream = this.OpenDialogService.OpenFile();
                using (var mem = new MemoryStream())
                {
                    stream.CopyTo(mem);
                    this.ExportToJson(mem.ToArray());
                }
            }
            this.OpenDialogService.Reset();
        }

        private void ExportToJson(byte[] sourceBytes)
        {
            var wrapper = ROBOT_TYPE_WRAPPER.Initialize(sourceBytes);
            var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            this.SaveDialogService.Filter = "All files (*.*) | *.json";
            this.SaveDialogService.Title = "Save a file";
            this.SaveDialogService.FileName = "robottype.json";
            this.SaveDialogService.OverwritePrompt = true;
            if (this.SaveDialogService.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(this.SaveDialogService.FileName, json);
            }
            this.SaveDialogService.Reset();
        }


        public void ImportJson()
        {
            this.OpenDialogService.FileName = "robottype.json";
            this.OpenDialogService.Filter = "All files (*.json) | *.json";
            if (this.OpenDialogService.ShowDialog() == DialogResult.OK)
            {
                var ext = Path.GetExtension(this.OpenDialogService.FileName);
                if (!".json".Equals(ext))
                {
                    MessageBox.Show("Vui lòng chọn đúng file robottype.json", "Thông báo!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.OpenDialogService.Reset();
                    return;
                }
                var stream = this.OpenDialogService.OpenFile();
                using (var mem = new MemoryStream())
                {
                    stream.CopyTo(mem);
                    this.ExportToDat(mem.ToArray());
                }
            }
            this.OpenDialogService.Reset();
        }

        private void ExportToDat(byte[] byteSources)
        {
            var json = Encoding.UTF8.GetString(byteSources);
            var wrapper = JsonConvert.DeserializeObject<ROBOT_TYPE_WRAPPER>(json);
            this.SaveDialogService.FileName = "robottype.dat";
            var convertBytes = wrapper.Build();
            if (convertBytes.Any())
            {
                if (this.SaveDialogService.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(this.SaveDialogService.FileName, convertBytes);
                }
            }
            this.SaveDialogService.Reset();
        }
    }
}
