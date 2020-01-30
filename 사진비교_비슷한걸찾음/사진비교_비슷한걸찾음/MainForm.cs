/*
 * Created by SharpDevelop.
 * User: asdf-2345
 * Date: 2020-01-29
 * Time: 오후 2:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace 사진비교_비슷한걸찾음
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
				
		}
		
		void Button1Click(object sender, EventArgs e)//1의 이미지 불러옴
		{
			string image1 = string.Empty;
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.InitialDirectory = @"D:\";
			
			if(dialog.ShowDialog() == DialogResult.OK){
				image1 = dialog.FileName;
			}
			else if(dialog.ShowDialog() == DialogResult.Cancel){
				return;
			}

			pictureBox1.Image = Bitmap.FromFile(image1);
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
		}
		
		void Button2Click(object sender, EventArgs e)//2의 이미지 불러옴
		{
			string image2 = string.Empty;
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.InitialDirectory = @"D:\";
			
			if(dialog.ShowDialog() == DialogResult.OK){
				image2 = dialog.FileName;
			}
			else if(dialog.ShowDialog() == DialogResult.Cancel){
				return;
			}

			pictureBox2.Image = Bitmap.FromFile(image2);
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
		}
		
		int comparisonNum = 0;//ReSize에서 사용됨
		double AllowableMarginOfError = 0;
		void Button3Click(object sender, EventArgs e)//대충비교		
		{
			comparisonNum = 4;
			AllowableMarginOfError = 0.6;
			Bitmap image1 = new Bitmap(pictureBox1.Image);
			Bitmap image2 = new Bitmap(pictureBox2.Image);
			Bitmap[] image = {image1, image2};
			image = reducedResolution(image1, image2);
			string a = imageComparison(image[0], image[1]);
			textBox1.Text = a;
		}
		
		void Button4Click(object sender, EventArgs e)//비교
		{
			comparisonNum = 2;
			AllowableMarginOfError = 0.3;
			Bitmap image1 = new Bitmap(pictureBox1.Image);
			Bitmap image2 = new Bitmap(pictureBox2.Image);
			Bitmap[] image = {image1, image2};
			image = reducedResolution(image1, image2);
			string a = imageComparison(image[0], image[1]);
			textBox1.Text = a;
		}
		
		void Button5Click(object sender, EventArgs e)//완전비교
		{
			comparisonNum = 0;
			AllowableMarginOfError = 1;
			Bitmap image1 = new Bitmap(pictureBox1.Image);
			Bitmap image2 = new Bitmap(pictureBox2.Image);
			string a = imageComparison(image1, image2);
			textBox1.Text = a;
		}
		
		string imageComparison(Bitmap image1, Bitmap image2){
			string Image1, Image2;
			int count = 0;
			int size = image1.Width * image1.Height;
			for (int i = 0; i < image1.Width; i++){
				for (int j = 0; j < image1.Height; j++){
					Image1 = image1.GetPixel(i, j).ToString();
					Image2 = image2.GetPixel(i, j).ToString();
						
					if (Image1 != Image2){
						count++;
					}
				}
			}
			if(count > size * AllowableMarginOfError){
				return "다릅니다."; // + count.ToString() + " " + size.ToString();
			}
			else{
				if(comparisonNum == 0){
					return "똑같습니다.";
				}
				else{
					return "비슷합니다."; // + count + size * (AllowableMarginOfError / 10);
				}
			}
		}
		Bitmap[] reducedResolution(Bitmap image1, Bitmap image2){
			int _width = Math.Max(image1.Width, image2.Width);
			int _height = Math.Max(image1.Height, image2.Height);
			int width = ReSize(_width);
			int height = ReSize(_height);
			
			Size size = new Size(width, height);
			Bitmap resizeImage1 = new Bitmap(image1, size);
			
			Bitmap resizeImage2 = new Bitmap(image2, size);
			
			Bitmap[] resizeImage = {resizeImage1, resizeImage2};
			return resizeImage;
		}
		
		int ReSize(int size){
			if(size > 2000){
				return (size / (40 * comparisonNum));
			}
			else if(size > 1500){
				return (size / (25 * comparisonNum));
			}
			else if(size > 1000){
				return (size / (18 * comparisonNum));
			}
			else if(size > 500){
				return (size / (9 * comparisonNum));
			}
			else if(size > 250){
				return (size / (5 * comparisonNum));
			}
			else if(size > 100){
				return (size / (2 * comparisonNum));
			}
			else if(size > 50){
				return (size / comparisonNum);
			}
			
			return size;
		}
			
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			
		}
	}
}
