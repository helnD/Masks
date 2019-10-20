///////////////////////////////////////////////////////////
//  ImageModel.cs
//  Implementation of the Class ImageModel
//  Generated by Enterprise Architect
//  Created on:      20-���-2019 10:46:15
//  Original author: Heln
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;


namespace Domain
{

	public class ImageModel
    {
	    public ImageModel(List<Pixel> pixels)
	    {
		    Pixels = pixels;
	    }

	    List<Pixel> Pixels { get; }

		public ImageModel ApplyMask(Mask mask)
		{
			var borders = new Borders(-mask.Pixels.Min(it => it.X),
				-mask.Pixels.Min(it => it.Y),
				mask.Pixels.Max(it => it.X),
				mask.Pixels.Max(it => it.Y));

			int xModelSize = Pixels.Max(it => it.X) + 1;
			int yModelSize = Pixels.Max(it => it.Y) + 1;
			int[][] virtualModel = CreateVirtualModel(borders, xModelSize, yModelSize);

			List<Pixel> result = new List<Pixel>();
			
			for (int x = borders.Left; x <= xModelSize - borders.Right; x++)
			{
				for (int y = borders.Top; y <= yModelSize - borders.Bottom; y++)
				{
					var pixel = new Pixel(x - borders.Left, y - borders.Top,
						ApplyToVirtualModel(virtualModel, x, y, mask));
					
					result.Add(pixel);
				}
			}
			
			return new ImageModel(result);
		}

		private int[][] CreateVirtualModel(Borders borders, int xSize, int ySize)
		{
			
			int xModelSize = xSize + borders.Left + borders.Right;
			int yModelSize = ySize + borders.Top + borders.Bottom;
			
			int [][] model = new int[xModelSize][];

			for (int i = 0; i < xModelSize; i++)
			{
				model[i] = new int[yModelSize];
			}
			
			for (int x = 0; x < xSize; x++)
			{
				for (int y = 0; y < xSize; y++)
				{
					model[x + borders.Left][y + borders.Top] = this[x, y];
				}
			}

			for (int x = 0; x < xSize; x++)
			{
				for (int y = 0; y < borders.Top; y++)
				{
					model[x + borders.Left][y] = this[x, 0];
				}

				for (int y = ySize + borders.Top - 1; y < yModelSize; y++)
				{
					model[x + borders.Left][y] = this[x, ySize - 1];
				}
			}

			for (int y = 0; y < yModelSize; y++)
			{
				for (int x = 0; x < borders.Left; x++)
				{
					model[x][y] = model[borders.Left][y];
				}

				for (int x = xSize + borders.Left; x < xModelSize; x++)
				{
					model[x][y] = model[borders.Left + xSize - 1][y];
				}
			}

			return model;
		}

		private int ApplyToVirtualModel(int[][] model, int x, int y, Mask mask)
		{
			int sum = 0;
			mask.Pixels.ForEach(it => sum += it.Value * model[x + it.X][y + it.Y]);

			if (sum > 256) return sum / mask.Sum();
			if (sum < 0) return 0;
			else return sum;
		}

		public int this[int index1, int index2] =>
			Pixels.Single(it => it.X == index1 && it.Y == index2).Color;

		private class Borders
		{
			public Borders(int left, int top, int right, int bottom)
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			public int Left { get; }
			public int Top { get; }
			public int Right { get; }
			public int Bottom { get; }
		}
		
    }

}