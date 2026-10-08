using Raylib_cs;

namespace SpaceWars;

public class Tiro : Nave
{
    private Nave _alvo;
    private int _direcaoX;
    private int _dano;
    private bool _perfurante; // atravessa sem ser removido ao acertar

    public float OffsetY = 0; // deriva vertical (usado na rafaga do Helius)
    public int DirecaoX => _direcaoX;

    public Tiro(string caminhoImagem, int direcaoX, Nave alvo,
                int dano = 15, bool perfurante = false)
        : base(caminhoImagem, 40, 30)
    {
        Speed = 18;
        _direcaoX = direcaoX;
        _alvo = alvo;
        _dano = dano;
        _perfurante = perfurante;
        Width = 100;
        Height = 60;
    }

    public bool Atualizar()
    {
        X += Speed * _direcaoX;
        Y += OffsetY;

        if (X > Raylib.GetScreenWidth() || X < 0)
            return false;

        return true;
    }

    public bool ChecarColisao(Nave alvo)
{
    Rectangle retTiro = new Rectangle(X, Y, Width, Height);
    Rectangle retAlvo = new Rectangle(alvo.X, alvo.Y, alvo.Width, alvo.Height);

    if (Raylib.CheckCollisionRecs(retTiro, retAlvo))
    {
        alvo.Dano(_dano);
        return true;
    }
    return false;
}
}