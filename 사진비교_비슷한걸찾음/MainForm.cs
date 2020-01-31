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
using System.Threading;

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
		
		int comparisonNum = 0;//colorComparison, ReSize에서 사용됨
		double AllowableMarginOfError = 0;
		void Button3Click(object sender, EventArgs e)//대충비교		
		{
			comparisonNum = 4;
			AllowableMarginOfError = 0.6;
			string str = "";
			Bitmap image1 = new Bitmap(pictureBox1.Image);
			Bitmap image2 = new Bitmap(pictureBox2.Image);
			Bitmap[] image = {image1, image2};
			image = photoConversion(image1, image2);
			for(int a = 0; a < 4; a++){
				image[1] = image2;
				str = imageComparison(image[0], image[1]);
				if(str != "다릅니다."){
					break;
				}
				image2.RotateFlip(RotateFlipType.Rotate90FlipNone);
			}
			textBox1.Text = str;
		}
		
		void Button4Click(object sender, EventArgs e)//비교
		{
			comparisonNum = 2;
			AllowableMarginOfError = 0.8;
			string str = "";
			Bitmap image1 = new Bitmap(pictureBox1.Image);
			Bitmap image2 = new Bitmap(pictureBox2.Image);
			Bitmap[] image = {image1, image2};
			image = photoConversion(image1, image2);
			for(int a = 0; a < 4; a++){
				image[1] = image2;
				str = imageComparison(image[0], image[1]);
				if(str != "다릅니다."){
					break;
				}
				image2.RotateFlip(RotateFlipType.Rotate90FlipNone);
			}
			textBox1.Text = str;
		}
		
		string imageComparison(Bitmap image1, Bitmap image2){
			string Image1, Image2;
			int count = 0;
			int size = image1.Width * image1.Height;
			for (int i = 0; i < image1.Width; i++){
				for (int j = 0; j < image1.Height; j++){
					Image1 = image1.GetPixel(i, j).ToString();
					Image2 = image2.GetPixel(i, j).ToString();
					if(Image1 != Image2){
						count++;
					}
				}
			}
			if(count >= size * AllowableMarginOfError){
				return "다릅니다."; // + count.ToString() + " " + size.ToString();
			}
			else{
				return "비슷합니다."; // + count + size * (AllowableMarginOfError / 10);
			}
		}
		
		
		Bitmap[] photoConversion(Bitmap image1, Bitmap image2){ //이미지 두개를 같은크기로 만들어줌
			int _width = Math.Max(image1.Width, image2.Width);
			int _height = Math.Max(image1.Height, image2.Height);
			int width = ReSize(_width); //더 크기가 큰 영상을 기준으로 크기를 변경
			int height = ReSize(_height);
			Size size = new Size(width, height);
			
			Bitmap resizeImage1 = new Bitmap(image1, size);
			pictureBox1.Image = resizeImage1;
			
			Bitmap resizeImage2 = new Bitmap(image2, size);
			pictureBox2.Image = resizeImage2;
			
			//recolorImage1 =
			Bitmap[] outputImage = ReColor(resizeImage1, resizeImage2);
			return outputImage;
		}
		
		Bitmap[] ReColor(Bitmap img1, Bitmap img2){
			Color img1Color;
			Color img2Color;
			double comparisonNum2 = (comparisonNum * 0.1);
			for (int i = 0; i < img1.Width; i++){
				for (int j = 0; j < img2.Height; j++){
					img1Color = img1.GetPixel(i, j);
					img2Color = img2.GetPixel(i, j);
					
					img1Color = getReColor(img1Color);
					img2Color = getReColor(img2Color);
					img1.SetPixel(i, j, img1Color);
					img2.SetPixel(i, j, img2Color);
				}
			}
			Bitmap[] img = {img1, img2};
			return img;
		}
		
		Color getReColor(Color color){
			int colorR = 0;
			int colorG = 0;
			int colorB = 0;
			
			if(color.R > 200 && color.G > 200 && color.B > 200){
				colorR = 255; // 하양
				colorG = 255;
				colorB = 255;
			}
			else if(color.R < 50 && color.G < 50 && color.B < 50){
				colorR = 0; // 검정
				colorG = 0;
				colorB = 0;	
			}
			else if(color.R > color.G && color.R > color.B){
				if(color.R > color.G + color.B){
					colorR = 255; // 빨강
				}
				else{
					colorR = 255;
					colorG = 100;
					colorB = 100;
				}
			}
			else if(color.G > color.R && color.G > color.B){
				if(color.G > color.R + color.B){
					colorR = 255; // 초록
				}
				else{
					colorG = 255;
					colorB = 100;
					colorR = 100;
				}
			}
			else{//if(colorB > colorG && colorB > colorR){
				if(color.B > color.G + color.R){
					colorB = 255; // 빨강
				}
				else{
					colorB = 255;
					colorG = 100;
					colorR = 100;
				}
			}
			Color output = Color.FromArgb(colorR, colorG, colorB);
			return output;
		}
		
		int ReSize(int size){ //해상도를 낮추는 함수
			if(size > 2000){
				return (size / (20 * comparisonNum));
			}
			else if(size > 1500){
				return (size / (15 * comparisonNum));
			}
			else if(size > 1000){
				return (size / (10 * comparisonNum));
			}
			else if(size > 500){
				return (size / (5 * comparisonNum));
			}
			else if(size > 250){
				return (size / (3 * comparisonNum));
			}
			else if(size > 100){
				return (size / (1 * comparisonNum));
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
	
	class TrashCan{ //접으려고 만들어놓음
		//bool colorComparison(int[] Image1Color, int[] Image2Color){
		//	int image1 = Image1Color[0] + Image1Color[1] + Image1Color[2];
		//	int image2 = Image2Color[0] + Image2Color[1] + Image2Color[2];
		//	if(image1 - image2 > (3 * comparisonNum) || image2 - image1 > (3 * comparisonNum)){
		//		return false;
		//	}
		//	for(int a = 0; a < 3; a++){
		//		if(Image1Color[a] - Image2Color[a] > (2 * comparisonNum) || Image2Color[a] - Image1Color[a] > (2 * comparisonNum)){
		//			Console.WriteLine((Image1Color[a] - Image2Color[a]) +" "+ (Image2Color[a] - Image1Color[a]));
		//			return false;
		//		}
		//	}
		//	return true;
		//}
		
		//int[] findColors(string input){ // Color.R, Color.G, Color.B로 표현할 수 있었네 아 ㅋㅋ
		//	//색깔값은 Color [A=255, R=114, G=93, B=8] 이런식으로 되어있음
		//	//얼마나 투명한지 표현하는 A값은 보지 않음 왜냐면 귀찮거든
		//	int RPoint = input.IndexOf("R");
		//	int GPoint = input.IndexOf("G");
		//	int BPoint = input.IndexOf("B");
		//	int R = int.Parse(input.Substring(RPoint+2, input.IndexOf(",", RPoint)-RPoint-2));
		//	Console.WriteLine(R);
		//	int G = int.Parse(input.Substring(GPoint+2, input.IndexOf(",", GPoint)-GPoint-2));
		//	Console.WriteLine(G);
		//	int B = int.Parse(input.Substring(BPoint+2, input.IndexOf("]", BPoint)-BPoint-2));
		//	Console.WriteLine(B);
		//	int[] Color = {R, G, B};
		//	return Color;
		//}
	}
}
