﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Dto
{
    public class EntidadeValidacao
    {
        public Type TipoEntidade { get; set; }
        public string Nome => TipoEntidade?.Name;
        public bool Existe { get; set; }
    }
}
