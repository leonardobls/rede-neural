namespace RedeNeural.DataTransferObjects
{
    internal class FutebolDTO
    {

        public int Id { get; set; }
        
        public string? FTR { get; set; } // Full Time Result -> Determina as classes H A D

        // public double HTHG { get; set; }

        // public double HTAG { get; set; }

        public double HS { get; set; } // Home Teams Shots -> Chutes do time da casa

        public double AS { get; set; } // Away Teams Shots -> Chutes do time visitante

        public double HST { get; set; } //Home Teams Shots on Target -> Chutes ao gol do time da casa

        public double AST { get; set; } //Away Teams Shots on Target -> Chutes ao gol do time visitante

        public double HC { get; set; } // Home Team Corners -> Escanteios do time da casa

        public double AC { get; set; } // Away Team Corners -> Escanteios do time visitante   

        public double HF { get; set; } // Away Team Corners -> Escanteios do time visitante   

        public double AF { get; set; } // Away Team Corners -> Escanteios do time visitante   

        public double HY { get; set; } // Away Team Corners -> Escanteios do time visitante   

        public double AY { get; set; } // Away Team Corners -> Escanteios do time visitante   

    }

}


