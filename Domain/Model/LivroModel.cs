﻿using Domain.Entity;

namespace Domain.Model
{
    public class LivroModel
    {
        public int Codl { get; set; }

        public string Titulo { get; set; }

        public string Editora { get; set; }

        public int Edicao { get; set; }

        public string AnoPublicacao { get; set; }

        public int StatusReg { get; set; }

        public LivroAssuntoEntity LivroAssuntoEntity { get; set; }

        public List<LivroAutorEntity> LivroAutores { get; set; }

    }
}
