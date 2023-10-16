namespace ProjetoDeRedes
{
    internal class FutebolDTO
    {
        public string? FTR { get; set; } // Full Time Result -> Determina as classes H A D

        public string? HTHG { get; set; } // Half Time Home Team Goal -> Gols do time da casa no primeiro tempo

        public string? HTAG { get; set; } // Half Time Away Team Goal -> Gols do time visitante no primeiro tempo

        public string? HTR { get; set; } // Half Time Result -> Resultado do primeiro tempo

        public string? HS { get; set; } // Home Teams Shots -> Chutes do time da casa

        public string? AS { get; set; } // Away Teams Shots -> Chutes do time visitante

        public string? HST { get; set; } //Home Teams Shots on Target -> Chutes ao gol do time da casa

        public string? AST { get; set; } //Away Teams Shots on Target -> Chutes ao gol do time visitante

        public string? HC { get; set; } // Home Team Corners -> Escanteios do time da casa

        public string? AC { get; set; } // Away Team Corners -> Escanteios do time visitante    
    }
}
