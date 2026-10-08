using System.Numerics;
using Raylib_cs;

namespace SpaceWars;

public class Nave
{
    public int HP = 100;
    public int Speed = 5;
    public float X, Y;
    public int Width, Height;
    public Texture2D Imagem;

    public Nave(string caminhoImagem, int width, int height)
    {
        Imagem = Raylib.LoadTexture(caminhoImagem);
        Width = 250;
        Height = 85;
    }
    public void Desenhar()
    {
        Rectangle source = new Rectangle(0, 0, Imagem.Width, Imagem.Height);
        Rectangle dest = new Rectangle(X, Y, Width, Height);
        Raylib.DrawTexturePro(Imagem, source, dest, new Vector2(0, 0), 0, Color.White);
    }

    public virtual void Dano(int valorDano)
    {
        valorDano = Math.Max(valorDano, 0);
        HP -= valorDano;
    }
}