namespace Domain.ApplyBehaviour
{
    public interface IApplyBehaviour
    {
        int Apply(int[][] model, int x, int y, Mask mask);
    }
}