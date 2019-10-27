using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.ApplyBehaviour;
using Pixel = ViewModel.Data.Pixel;
using GalaSoft;
using GalaSoft.MvvmLight.Command;

namespace ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            
            SymmetricMask = CreateStartSymmetricMask();
            AsymmetricMask = CreateStartAsymmetricMask();
            RandomMask = CreateStartRandomMask();

            //инициализация моделей изображений
            for (int i = 0; i < 20; i++)
            {
                SourceModel[i] = new int[20];
                ResultModel[i] = new int[20];

                for (int j = 0; j < 20; j++)
                {
                    SourceModel[i][j] = 255;
                }
            }
            
            RegenerateSymmetricMaskCommand = new RelayCommand(RegenerateSymmetricMask);
            RegenerateAsymmetricMaskCommand = new RelayCommand(RegenerateAsymmetricMask);
            RegenerateRandomMaskCommand = new RelayCommand(RegenerateRandomMask);
            
            ClearSymmetricMaskCommand = new RelayCommand(ClearSymmetricMask);
            ClearAsymmetricMaskCommand = new RelayCommand(ClearAsymmetricMask);
            ClearRandomMaskCommand = new RelayCommand(ClearRandomMask);
            
            RestoreSymmetricMaskCommand  = new RelayCommand(RestoreSymmetricMask);
            RestoreAsymmetricMaskCommand  = new RelayCommand(RestoreAsymmetricMask);
            RestoreRandomMaskCommand  = new RelayCommand(RestoreRandomMask);
        }
        
        private int[][] CurrentModel { get; set; }
        private readonly MaskGenerator _generator = new MaskGenerator();

        public int BrushBrightness { get; set; } = 127;
        public Pixel[][] SymmetricMask { get; set; }
        public Pixel[][] AsymmetricMask { get; set; }
        public Pixel[][] RandomMask { get; set; }

        public int[][] SourceModel { get; set; } = new int[20][];
        public int[][] ResultModel { get; set; } = new int[20][];

        public bool IsSymmetric { get; set; } = true;
        public bool IsAsymmetric { get; set; } = false;
        public bool IsRandom { get; set; } = false;

        public bool IsDivide { get; set; } = true;
        public bool IsLimit { get; set; } = false;

        public void ApplyMask()
        {

            Mask mask;
            if (IsSymmetric) mask = ToMask(SymmetricMask, 2, 2);
            else if (IsAsymmetric) mask = ToMask(AsymmetricMask, 1, 0);
            else mask = ToMask(RandomMask, 1, 1);

            ImageModel sourceModel = ToImageModel(SourceModel); 
            
            if (IsDivide) sourceModel.ApplyMaskBehaviour = new NormalizationApplyBehaviour();
            else sourceModel.ApplyMaskBehaviour = new RangeApplyBehaviour();

            ImageModel resultModel = sourceModel.ApplyMask(mask);
            resultModel.Pixels.ForEach(it => ResultModel[it.Y][it.X] = it.Color);
        }

        public void ApplyMaskToResult()
        {
            Mask mask;
            if (IsSymmetric) mask = ToMask(SymmetricMask, 2, 2);
            else if (IsAsymmetric) mask = ToMask(AsymmetricMask, 1, 0);
            else mask = ToMask(RandomMask, 1, 1);

            ImageModel sourceModel = ToImageModel(ResultModel);
            
            for (var i = 0; i < ResultModel.Length; i++)
            {
                for (var j = 0; j < ResultModel[i].Length; j++)
                {
                    SourceModel[i][j] = ResultModel[i][j];
                }
            }

            if (IsDivide) sourceModel.ApplyMaskBehaviour = new NormalizationApplyBehaviour();
            else sourceModel.ApplyMaskBehaviour = new RangeApplyBehaviour();

            ImageModel resultModel = sourceModel.ApplyMask(mask);
            resultModel.Pixels.ForEach(it => ResultModel[it.Y][it.X] = it.Color);
        }

        public void RegenerateSymmetricMask()
        {
            _generator.GeneratorBehaviour = new SymmetryGeneratorBehaviour();
            var newMask = ToArrayMask(_generator.Generate());
            for (var i = 0; i < SymmetricMask.Length; i++)
            {
                for (var j = 0; j < SymmetricMask[i].Length; j++)
                {
                    SymmetricMask[i][j].Value = newMask[i][j].Value;
                }
            }
        }
        
        public void RegenerateAsymmetricMask()
        {
            _generator.GeneratorBehaviour = new AsymmetryGeneratorBehaviour();
            var newMask = ToArrayMask(_generator.Generate());
            for (var i = 0; i < AsymmetricMask.Length; i++)
            {
                for (var j = 0; j < AsymmetricMask[i].Length; j++)
                {
                    AsymmetricMask[i][j].Value = newMask[i][j].Value;
                }
            }
        }
        
        public void RegenerateRandomMask()
        {
            _generator.GeneratorBehaviour = new RandomGeneratorBehaviour();
            var newMask = ToArrayMask(_generator.Generate());
            for (var i = 0; i < RandomMask.Length; i++)
            {
                for (var j = 0; j < RandomMask[i].Length; j++)
                {
                    if (RandomMask[i][j] != null)
                    {
                        RandomMask[i][j].Value = newMask[i][j].Value;
                    }
                }
            }
        }

        public void ClearSymmetricMask()
        {
            foreach (var row in SymmetricMask)
            {
                foreach (var column in row)
                {
                    column.Value = 0;
                }
            }
        }
        
        public void ClearAsymmetricMask()
        {
            foreach (var row in AsymmetricMask)
            {
                foreach (var column in row)
                {
                    column.Value = 0;
                }
            }
        }
        
        public void ClearRandomMask()
        {
            foreach (var row in RandomMask)
            {
                foreach (var column in row)
                {
                    if (column != null)
                    {
                        column.Value = 0;
                    }
                }
            }
        }

        public void RestoreSymmetricMask()
        {
            var mask = CreateStartSymmetricMask();
            for (var i = 0; i < mask.Length; i++)
            {
                for (var j = 0; j < mask[i].Length; j++)
                {
                    SymmetricMask[i][j].Value = mask[i][j].Value;
                }
            }
        }
        
        public void RestoreAsymmetricMask()
        {
            var mask = CreateStartAsymmetricMask();
            for (var i = 0; i < mask.Length; i++)
            {
                for (var j = 0; j < mask[i].Length; j++)
                {
                    AsymmetricMask[i][j].Value = mask[i][j].Value;
                }
            }
        }
        
        public void RestoreRandomMask()
        {
            var mask = CreateStartRandomMask();
            for (var i = 0; i < mask.Length; i++)
            {
                for (var j = 0; j < mask[i].Length; j++)
                {
                    if (mask[i][j] != null)
                    {
                        RandomMask[i][j].Value = mask[i][j].Value;
                    }
                }
            }
        }
        
        private Mask ToMask(Pixel[][] mask, int xCenter, int yCenter)
        {
            List<MaskPixel> pixels = new List<MaskPixel>();
            for (int y = 0; y < mask.Length; y++)
            {
                for (int x = 0; x < mask[y].Length; x++)
                {
                    if (mask[y][x] != null)
                    {
                        pixels.Add(ToMaskPixel(x, y, mask[y][x].Value, xCenter, yCenter));
                    }
                }
            }
            return new Mask(pixels);
        }

        private ImageModel ToImageModel(int[][] model)
        {
            
            List<Domain.Pixel> pixels = new List<Domain.Pixel>();
            for (int y = 0; y < model.Length; y++)
            {
                for (int x = 0; x < model[y].Length; x++)
                {
                    pixels.Add(new Domain.Pixel(x, y, model[y][x]));
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

        private Pixel[][] CreateStartSymmetricMask()
        {
            Pixel[][] symmetricMask = new Pixel[5][];
            for (var i = 0; i < symmetricMask.Length; i++)
            {
                symmetricMask[i] = new Pixel[5];
                for (var j = 0; j < symmetricMask[i].Length; j++)
                {
                    symmetricMask[i][j] = new Pixel() {Value = 0};
                }
            }

            symmetricMask[0][0].Value = symmetricMask[0][4].Value = symmetricMask[4][0].Value
                = symmetricMask[4][4].Value = 1;
            
            symmetricMask[0][2].Value = symmetricMask[2][4].Value = symmetricMask[4][2].Value
                = symmetricMask[2][0].Value = 2;

            symmetricMask[2][2].Value = 4;

            return symmetricMask;
        }
        
        private Pixel[][] CreateStartAsymmetricMask()
        {
            Pixel[][] asymmetricMask = new Pixel[2][];
            for (var i = 0; i < asymmetricMask.Length; i++)
            {
                asymmetricMask[i] = new Pixel[6];
                for (var j = 0; j < asymmetricMask[i].Length; j++)
                {
                    asymmetricMask[i][j] = new Pixel() {Value = (i + j) % 2 == 0 ? 5 : 0};
                }
            }

            return asymmetricMask;
        }
        
        private Pixel[][] CreateStartRandomMask()
        {
            Pixel[][] randomMask = new Pixel[6][];
            for (var i = 0; i < randomMask.Length; i++)
            {
                randomMask[i] = new Pixel[3];
            }

            randomMask[1][1] = new Pixel() {Value = 5};
            randomMask[4][1] = new Pixel() {Value = 5};

            randomMask[0][0] = new Pixel() {Value = 10};
            randomMask[0][2] = new Pixel() {Value = 10};
            randomMask[5][0] = new Pixel() {Value = 10};
            randomMask[5][2] = new Pixel() { Value = 10};

            randomMask[2][0] = new Pixel() { Value = 5};
            randomMask[2][2] = new Pixel() { Value = 5};
            randomMask[3][0] = new Pixel() { Value = 10};
            randomMask[3][2] = new Pixel() { Value = 10};

            return randomMask;
        }

        public RelayCommand RegenerateSymmetricMaskCommand { get; set; }
        public RelayCommand RegenerateAsymmetricMaskCommand { get; set; }
        public RelayCommand RegenerateRandomMaskCommand { get; set; }
        
        public RelayCommand ClearSymmetricMaskCommand { get; set; }
        public RelayCommand ClearAsymmetricMaskCommand { get; set; }
        public RelayCommand ClearRandomMaskCommand { get; set; }
        
        
        public RelayCommand RestoreSymmetricMaskCommand { get; set; }
        public RelayCommand RestoreAsymmetricMaskCommand { get; set; }
        public RelayCommand RestoreRandomMaskCommand { get; set; }
    }
}