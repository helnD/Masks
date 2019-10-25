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
            for (int i = 0; i < 5; i++)
            {
                SymmetricMask[i] = new Pixel[5];
                for (int j = 0; j < 5; j++)
                {
                    //TODO(Надо поменять на генерацию симметричной маски)
                    SymmetricMask[i][j] = new Pixel() {Value = new Random().Next(0, 25)};
                }
            }
            
            //Инициализация значчений асимметричной маски
            for (int i = 0; i < 2; i++)
            {
                AsymmetricMask[i] = new Pixel[6];
                for (int j = 0; j < 6; j++)
                {
                    //TODO(Надо поменять на генерацию асимметричной маски)
                    AsymmetricMask[i][j] = new Pixel() {Value = new Random().Next(0, 25)};
                }
            }

            //Инициализация значений нерегулярной маски
            for (int i = 0; i < 6; i++)
            {
                RandomMask[i] = new Pixel[3];
            }
            
            //TODO(Надо поменять на генерацию нерегулярной маски)
            RandomMask[0][0] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[0][2] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[1][1] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[2][0] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[2][2] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[3][0] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[3][2] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[4][1] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[5][0] = new Pixel() {Value = new Random().Next(0, 25)};
            RandomMask[5][2] = new Pixel() {Value = new Random().Next(0, 25)};

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

        private MaskPixel ToMaskPixel(int x, int y, int value, int xCenter, int yCenter) =>
            new MaskPixel(x - xCenter, y - yCenter, value);
        
        public RelayCommand ApplyCommand { get; set; }
    }
}