using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Pixel = ViewModel.Data.Pixel;
using GalaSoft;
using GalaSoft.MvvmLight.Command;

namespace ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            //Инициализация значений симметричной маски
            _generator.GeneratorBehaviour = new SymmetryGeneratorBehaviour();
            SymmetricMask = ToArrayMask(_generator.Generate());
            
            //Инициализация значчений асимметричной маски
            _generator.GeneratorBehaviour = new AsymmetryGeneratorBehaviour();
            AsymmetricMask = ToArrayMask(_generator.Generate());
            
            //Инициализация значений нерегулярной маски
            _generator.GeneratorBehaviour = new RandomGeneratorBehaviour();
            RandomMask = ToArrayMask(_generator.Generate());

            //инициализация моделей изображений
            for (int i = 0; i < 20; i++)
            {
                SourceModel[i] = new int[20];
                ResultModel[i] = new int[20];
            }
            
            ApplyCommand = new RelayCommand(ApplyMask);

        }
        
        private int[][] CurrentModel { get; set; }
        private MaskGenerator _generator = new MaskGenerator();

        public int BrushBrightness { get; set; } = 127;
        public Pixel[][] SymmetricMask { get; set; } = new Pixel[5][];
        public Pixel[][] AsymmetricMask { get; set; } = new Pixel[2][];
        public Pixel[][] RandomMask { get; set; } = new Pixel[6][];

        public int[][] SourceModel { get; set; } = new int[20][];
        public int[][] ResultModel { get; set; } = new int[20][];

        public bool IsSymmetric { get; set; } = true;
        public bool IsAsymmetric { get; set; } = false;
        public bool IsRandom { get; set; } = false;

        public void ApplyMask()
        {
            Mask mask;
            if (IsSymmetric) mask = ToMask(SymmetricMask, 2, 2);
            else if (IsAsymmetric) mask = ToMask(AsymmetricMask, 1, 0);
            else mask = ToMask(RandomMask, 1, 1);

            ImageModel resultModel = ToImageModel(SourceModel).ApplyMask(mask);
            resultModel.Pixels.ForEach(it => ResultModel[it.Y][it.X] = it.Color);
        }

        private Mask ToMask(Pixel[][] mask, int xCenter, int yCenter)
        {
            List<MaskPixel> pixels = new List<MaskPixel>();
            for (int y = 0; y < mask.Length; y++)
            {
                for (int x = 0; x < mask[y].Length; x++)
                {
                    pixels.Add(ToMaskPixel(x, y, mask[y][x].Value, xCenter, yCenter));
                }
            }
            return new Mask(pixels);
        }

        private ImageModel ToImageModel(int[][] model)
        {
            
            List<Domain.Pixel> pixels = new List<Domain.Pixel>();
            for (int y = 0; y < model.Length; y++)
            {
                for (int x= 0; x < model[y].Length; x++)
                {
                    pixels.Add(new Domain.Pixel(x, y, model[x][y]));
                }
            }
            
            return new ImageModel(pixels);
        }
        
        private Pixel[][] ToArrayMask(Mask mask)
        {
            int startX = mask.Pixels.Min(it => it.X);
            int startY = mask.Pixels.Min(it => it.Y);

            int width = mask.Pixels.Max(it => it.X) - startX + 1;
            int height = mask.Pixels.Max(it => it.Y) - startY + 1;
	        
            Pixel[][] result = new Pixel[height][];
            for (int y = 0; y < height; y++)
            {
                result[y] = new Pixel[width];
            }
	        
            foreach (var pixel in mask.Pixels)
            {
                result[pixel.Y - startY][pixel.X - startX] = new Pixel() { Value = pixel.Value };
            }

            return result;
        }

        private MaskPixel ToMaskPixel(int x, int y, int value, int xCenter, int yCenter) =>
            new MaskPixel(x - xCenter, y - yCenter, value);
        
        public RelayCommand ApplyCommand { get; set; }
    }
}