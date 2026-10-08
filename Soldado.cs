using Raylib_cs;

namespace SpaceWars;

public class Soldado : Nave
{
    private int _tiroCont = 0;
    private int _intervaloTiro;
    private static Random _rnd = new Random();

    public Soldado(string sprite, int hp) : base(sprite, 80, 50)
    {
        HP = hp;
        Speed = 0;
        X = Raylib.GetScreenWidth() - Width - 50;
        Y = _rnd.Next(50, Raylib.GetScreenHeight() - 100);
        _intervaloTiro = _rnd.Next(60, 120);
    }

    public bool Atualizar(List<Tiro> tiros, NaveJogador alvo)
    {
        _tiroCont++;
        if (_tiroCont >= _intervaloTiro)
        {
            Tiro tiro = new Tiro("./assets/tiroInimigo.png", -1, alvo, 10, false);
            tiro.X = X;
            tiro.Y = Y + Height / 2;
            tiros.Add(tiro);
            _tiroCont = 0;
        }

        return HP > 0;
    }

    public override void Dano(int valorDano)
    {
        HP -= valorDano;
        if (HP < 0) HP = 0;
    }
}
