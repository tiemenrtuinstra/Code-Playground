using Common.Enums;
using System;
using System.Collections.Generic;

namespace Common.Extensions;

public static class LineTypeExtensions
{
    private static readonly Dictionary<LineType, char> LineTypeChars = new()
    {
        { LineType.None, ' ' },
        { LineType.Solid, '█' },
        { LineType.Dashed, '-' },
        { LineType.Dotted, '.' },
        { LineType.Double, '=' },
        { LineType.Circle, '○' },
        { LineType.Square, '■' },
        { LineType.Triangle, '▲' },
        { LineType.Star, '★' },
        { LineType.Plus, '+' },
        { LineType.Cross, '✖' },
        { LineType.Diamond, '♦' },
        { LineType.Arrow, '→' },
        { LineType.Heart, '♥' },
        { LineType.Spade, '♠' },
        { LineType.Club, '♣' },
        { LineType.Clover, '♧' },
        { LineType.Lightning, '⚡' },
        { LineType.Moon, '☽' },
        { LineType.Sun, '☀' },
        { LineType.Cloud, '☁' },
        { LineType.Rain, '☂' },
        { LineType.Snow, '❄' },
        { LineType.Wind, '♫' },
        { LineType.Tornado, '☼' },
        { LineType.Earth, '♁' },
        { LineType.Fire, '♨' },
        { LineType.Water, '♒' },
        { LineType.Air, '♂' },
        { LineType.Metal, '♃' },
        { LineType.Wood, '♄' },
        { LineType.Spirit, '♅' }
    };

    public static char GetLineChar(this LineType lineType)
    {
        if (LineTypeChars.TryGetValue(lineType, out var lineChar))
        {
            return lineChar;
        }
        throw new ArgumentOutOfRangeException(nameof(lineType), lineType, null);
    }
}

