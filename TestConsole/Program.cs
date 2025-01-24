// See https://aka.ms/new-console-template for more information

var traki = new Traki(10);

Console.WriteLine(traki.ToString());

public class Traki
{
    private readonly int idk;

    public Traki(int idk)
    {
        this.idk = idk;
    }

    public override string ToString()
    {
        return $"{idk} traki";
    }
}
