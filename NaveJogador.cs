using Raylib_cs;

namespace SpaceWars;

public class NaveJogador : Nave
{
    private GerenciadorPersonagens _gp;

    public NaveJogador(GerenciadorPersonagens gp) : base("./assets/naveProtagonista.png", 400, 80)
    {
        X = 100;
        Y = 500;
        Speed = 5;
        _gp = gp;
    }

    public void Atualizar()
    {
        string caminho = _gp.Atual.CaminhoNave;
        if (Imagem.Id == 0 || _caminhoAtual != caminho)
        {
            Raylib.UnloadTexture(Imagem);
            Imagem = Raylib.LoadTexture(caminho);
            _caminhoAtual = caminho;
        }
    }

private string _caminhoAtual = "";

    public override void Dano(int valorDano)
    {
        _gp.Atual.ReceberDano(valorDano);
    }
}