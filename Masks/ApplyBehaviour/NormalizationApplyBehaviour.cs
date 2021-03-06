﻿namespace Domain.ApplyBehaviour
{
    public class NormalizationApplyBehaviour : IApplyBehaviour
    {
        public int Apply(int[][] model, int x, int y, Mask mask)
        {
            int sum = 0;
            mask.Pixels.ForEach(it => sum += it.Value * model[x + it.X][y + it.Y]);

            if (sum > 256) return sum / mask.Sum();
            if (sum < 0) return 0;
            else return sum;
        }
    }
}