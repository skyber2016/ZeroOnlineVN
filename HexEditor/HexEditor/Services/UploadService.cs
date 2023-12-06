using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace HexEditor.Services
{
    internal class UploadService
    {
        private readonly OpenFileDialog OpenDialogService = new OpenFileDialog();

        public void DoUpload()
        {
            this.OpenDialogService.Filter = "All files (*.exe) | *.exe";
            if (this.OpenDialogService.ShowDialog() == DialogResult.OK)
            {
                var ext = Path.GetExtension(this.OpenDialogService.FileName);
                if (!".exe".Equals(ext))
                {
                    MessageBox.Show("Vui lòng chọn đúng file 1xxx.exe", "Thông báo!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.OpenDialogService.Reset();
                    return;
                }
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(this.OpenDialogService.OpenFile()), "file", "upload.exe");

                        using (var message = client.PostAsync("https://update.zeroonlinevn.com/patches", content))
                        {
                            message.Wait();
                            if (!message.Result.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Đã xảy ra lỗi.", "Thông báo!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("OK", "Thông báo!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }

            }
            this.OpenDialogService.Reset();
        }
    }
}
