using CF.Domain.Atributos;
using CF.Domain.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Extensoes
{
    public static class EnumExtensions
    {
        public static string ObterCodigoGlyph(this eStatusPagamento status)
        {
            var type = status.GetType();
            var member = type.GetMember(status.ToString());
            var attribute = member[0].GetCustomAttribute<CodigoGlyph>();
            return attribute?.Glyph ?? string.Empty;
        }
        public static string ObterCodigoGlyph(this eTipoOperacao operacao)
        {
            var type = operacao.GetType();
            var member = type.GetMember(operacao.ToString());
            var attribute = member[0].GetCustomAttribute<CodigoGlyph>();
            return attribute?.Glyph ?? string.Empty;
        }
    }
}
