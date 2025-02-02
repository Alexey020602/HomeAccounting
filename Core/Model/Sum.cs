namespace Core.Model;

public record Sum(int FullPennySum)
{
    public int Rubles => FullPennySum / 100;
    public int Penny => FullPennySum % 100;

    public override string ToString()
    {
        return $"{Rubles}.{Penny}";
    }
}