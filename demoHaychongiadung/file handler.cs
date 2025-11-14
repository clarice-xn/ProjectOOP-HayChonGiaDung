using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== FILE HANDLER ====================
    public class FileHandler
    {
        private string filePath;

        public FileHandler(string path)
        {
            this.filePath = path;
        }

        private bool FileExists()
        {
            return File.Exists(this.filePath);
        }

        private void CreateBackup()
        {
            try
            {
                string backupFileName = Path.GetFileNameWithoutExtension(filePath)
                                        + "_backup_"
                                        + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                                        + Path.GetExtension(filePath);
                string backupPath = Path.Combine(Path.GetDirectoryName(filePath) ?? "", backupFileName);

                File.Copy(filePath, backupPath, true);
                Console.WriteLine($"✓ Đã tạo bản sao dự phòng: {backupFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠ Lỗi khi tạo bản sao dự phòng: " + ex.Message);
            }
        }

        public void SaveToFile(List<Product> products)
        {
            FileStream fs = null;
            try
            {
                if (FileExists())
                {
                    CreateBackup();
                }

                fs = new FileStream(this.filePath, FileMode.Create);
#pragma warning disable SYSLIB0011
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, products);
#pragma warning restore SYSLIB0011

                Console.WriteLine("✓ Đã lưu dữ liệu ra file thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠ Lỗi khi lưu file: " + ex.Message);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        public List<Product> LoadFromFile()
        {
            if (!FileExists())
            {
                Console.WriteLine("ℹ File chưa tồn tại, trả về danh sách rỗng.");
                return new List<Product>();
            }

            FileStream fs = null;
            List<Product> products = null;

            try
            {
                fs = new FileStream(this.filePath, FileMode.Open);
#pragma warning disable SYSLIB0011
                BinaryFormatter formatter = new BinaryFormatter();
                products = (List<Product>)formatter.Deserialize(fs);
#pragma warning restore SYSLIB0011
                Console.WriteLine("✓ Đọc dữ liệu từ file thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠ Lỗi khi đọc file: " + ex.Message);
                products = new List<Product>();
            }
            finally
            {
                if (fs != null) fs.Close();
            }

            return products;
        }
    }
}
