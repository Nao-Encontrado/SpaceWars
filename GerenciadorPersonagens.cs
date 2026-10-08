using Raylib_cs;

namespace SpaceWars;
/// Gerencia os 4 personagens jogáveis.
/// Teclas 1-4 trocam o personagem ativo.
/// Q ativa a habilidade do personagem atual.
public class GerenciadorPersonagens
{
    public List<Personagem> Personagens { get; private set; }
    public Personagem Atual => Personagens[_indiceAtual];

    private int _indiceAtual = 0;

    public GerenciadorPersonagens()
    {
        Personagens = new List<Personagem>
        {
            // Helius — equilibrado, rafaga de 3 tiros
            new Personagem(
                nome:       "Helius",
                caminhoNave: "./assets/naveProtagonista.png",
                hp:         120,
                dano:       15,
                speed:      6,
                cadencia:   0.35f,
                habilidade: TipoHabilidade.TiroRapido,
                cooldown:   8f,
                cor:        Color.Orange
            ),
            // Hares — tanque lento, escudo pessoal
            new Personagem(
                nome:       "Hares",
                caminhoNave: "./assets/nave2.png",
                hp:         200,
                dano:       10,
                speed:      4,
                cadencia:   0.6f,
                habilidade: TipoHabilidade.Escudo,
                cooldown:   12f,
                cor:        Color.SkyBlue
            ),
            // Cipher — rápido, tiro perfurante
            new Personagem(
                nome:       "Cipher",
                caminhoNave: "./assets/nave3.png",
                hp:         80,
                dano:       20,
                speed:      8,
                cadencia:   0.5f,
                habilidade: TipoHabilidade.TiroPerfurante,
                cooldown:   10f,
                cor:        Color.Purple
            ),
            // Serath — fraco no combate, cura o grupo
            new Personagem(
                nome:       "Serath",
                caminhoNave: "./assets/nave4.png",
                hp:         90,
                dano:       8,
                speed:      5,
                cadencia:   0.7f,
                habilidade: TipoHabilidade.CuraAliados,
                cooldown:   15f,
                cor:        Color.Green
            ),
        };
    }

    public void Atualizar(float delta)
    {
        // Atualiza timers de todos (mesmo inativos)
        foreach (var p in Personagens)
            p.Atualizar(delta);

        // Troca de personagem — teclas 1, 2, 3, 4
        if (Input.KeyPressed(KeyboardKey.One))   TrocarPara(0);
        if (Input.KeyPressed(KeyboardKey.Two))   TrocarPara(1);
        if (Input.KeyPressed(KeyboardKey.Three)) TrocarPara(2);
        if (Input.KeyPressed(KeyboardKey.Four))  TrocarPara(3);
    }

    private void TrocarPara(int indice)
    {
        if (indice >= 0 && indice < Personagens.Count && Personagens[indice].EstaVivo)
        {
            _indiceAtual = indice;
        }
    }

    // Retorna a lista de tiros gerados (pode ser mais de 1 com TiroRapido)
    public List<Tiro> TentarAtirar(float posX, float posY, Nave alvo)
    {
        var resultado = new List<Tiro>();
        var p = Atual;

        if (!p.PodeAtirar) return resultado;

        p.RegistrarTiro();

        // ENTENDER O QUE ISSO FAZ E SE É NECESSÁRIO
        // if (p.Habilidade == TipoHabilidade.TiroRapido && p.TiroPerfuranteAtivo)
        // {
        //     // Não combina — só tiro normal
        // }

        // Tiro principal
        var tiro = CriarTiro(posX, posY, alvo, p);
        resultado.Add(tiro);

        // Spread vertical extra pra TiroRapido ativo (rafaga de 3)
        // A habilidade é ativada com Q, então aqui só adicionamos os tiros extras
        // quando a habilidade estiver em efeito — tratado em UsarHabilidade via Program.cs

        return resultado;
    }

    // Cria um tiro com stats do personagem atual
    private Tiro CriarTiro(float x, float y, Nave alvo, Personagem p)
    {
        return new Tiro("./assets/tiro.png", 1, alvo, p.Dano, p.TiroPerfuranteAtivo);
    }

    // Ativa habilidade do personagem atual
    // Retorna tiros extras gerados (ex: rafaga do Helius) e efeitos (Serath cura)
    public List<Tiro> UsarHabilidade(float posX, float posY, Nave alvo)
    {
        var resultado = new List<Tiro>();
        var p = Atual;

        if (!p.UsarHabilidade()) return resultado;

        switch (p.Habilidade)
        {
            case TipoHabilidade.TiroRapido:
                // 3 tiros em leque vertical
                for (int i = -1; i <= 1; i++)
                {
                    var t = new Tiro("./assets/tiro.png", 1, alvo, p.Dano, false);
                    t.X = posX;
                    t.Y = posY + i * 30;
                    t.OffsetY = i * 1.5f; // deriva vertical leve
                    resultado.Add(t);
                }
                break;

            case TipoHabilidade.CuraAliados:
                // Cura todos os personagens vivos em 30 HP
                foreach (var aliado in Personagens)
                    if (aliado.EstaVivo) aliado.Curar(30);
                break;

            // Escudo e TiroPerfurante: efeito já aplicado no Personagem.UsarHabilidade()
        }

        return resultado;
    }

    public int IndiceAtual => _indiceAtual;
}
