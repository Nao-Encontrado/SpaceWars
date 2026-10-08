using Raylib_cs;

namespace SpaceWars;

public class NaveInimigo : Nave
{
    private int _sentido = 1;
    private int _tiroCont = 0;
    private NaveJogador _alvo;

    public NaveInimigo(NaveJogador alvo, string sprite, int hp) : base(sprite, 120, 50)
    {
        X = Raylib.GetScreenWidth() - 200;
        Y = 50;
        Speed = 3;
        HP = hp;
        _alvo = alvo;
    }

    public void Atualizar(List<Tiro> tiros)
    {
        Y += Speed * _sentido;

        if (Y >= Raylib.GetScreenHeight() - Height || Y <= 0)
            _sentido = -_sentido;

        _tiroCont++;
        if (_tiroCont >= 60)
        {
            Tiro tiro = new Tiro("./assets/tiroInimigo.png", -1, _alvo, 10, false);
            tiro.X = X - 20;
            tiro.Y = Y + Height / 2;
            tiros.Add(tiro);
            _tiroCont = 0;
        }
    }

    public override void Dano(int valorDano)
    {
        HP -= valorDano;
        if (HP <= 0) HP = 0;
    }
}
