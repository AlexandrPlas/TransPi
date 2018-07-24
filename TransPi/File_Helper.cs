using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
//using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using Interferometry.math_classes;

namespace rab1
{
    class File_Helper
    {


        public static ZaArrayDescriptor loadImage()
        {
            OpenFileDialog dialog1 = new OpenFileDialog();
            dialog1.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
            dialog1.FilterIndex = 1;
            dialog1.RestoreDirectory = true;
          
           
            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    dialog1.InitialDirectory = dialog1.FileName;

                    Image newImage = Image.FromFile(dialog1.FileName);
                    ZaArrayDescriptor z_array =new ZaArrayDescriptor(newImage.Width, newImage.Height);
                    z_array = Util_array.getArrayFromImage(newImage);

                    return z_array;
                    
                }
                catch (Exception ex) { MessageBox.Show("Ошибка при чтении изображения" + ex.Message); return null; }
            }
            return null;
        }

        public static void saveImage(PictureBox pictureBox01)
        {
            SaveFileDialog dialog1 = new SaveFileDialog();
            //dialog1.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
            dialog1.Filter = "Images (*.JPG)|*.JPG|" + "Images (*.BMP)|*.BMP|" + "All files (*.*)|*.*";
            dialog1.FilterIndex = 1;
            dialog1.RestoreDirectory = true;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox01.Image.Save(dialog1.FileName);
                    dialog1.InitialDirectory = dialog1.FileName;
                    //string_dialog = dialog1.FileName;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Ошибка при записи файла " + ex.Message);
                }
            }

        }

        public static void saveZArray(ZaArrayDescriptor arrayDescriptor)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream;
                    if ((stream = saveFileDialog.OpenFile()) != null)
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        serializer.Serialize(stream, arrayDescriptor);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при записи файла double : " + ex.Message);
                }
            }
        }

        public static ZaArrayDescriptor loadZArray()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            BinaryFormatter deserializer = new BinaryFormatter();
                            ZaArrayDescriptor savedArray = (ZaArrayDescriptor)deserializer.Deserialize(myStream);
                            myStream.Close();
                            return savedArray;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла double  :" + ex.Message);
                    return null;
                }
            }

            return null;
        }



    }
}
