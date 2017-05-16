using System;

namespace FSMLibrary.NFSMBuild
{
    public enum SymbolType { EmptySymbol,Letter, KleeneStar, Concatenation, Union, OpenParenthesis,CloseParenthesis}
    public class Symbol
    {
        public SymbolType Type { get; set; }
        public char Value { get; set; }

       
        
        public Symbol(char value)
        {
            Value = value;
            switch (value)
            {
                case '*':
                    Type = SymbolType.KleeneStar;
                    return;
                case '+':
                    Type = SymbolType.Concatenation;
                    return;
                case '|':
                    Type = SymbolType.Union;
                    return;
                case '(':
                    Type = SymbolType.OpenParenthesis;
                    return;
                case ')':
                    Type = SymbolType.CloseParenthesis;
                    return;
                case '#':
                    Type = SymbolType.EmptySymbol;
                    return;
                
            }
            if (char.IsLetterOrDigit(value))
            {
                Type=SymbolType.Letter;
                return;
            }
            throw new FormatException("Incorrect symbol");
    
        }

        public static bool operator ==(Symbol left, Symbol right)
        {
            if ((object)left == null && (object)right == null)
                return true;
            if ((object)left == null || (object)right == null)
                return false;
            return (left.Value == right.Value && left.Type == right.Type);
        }

        public static bool operator !=(Symbol left, Symbol right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((Symbol)obj);
        }

        protected bool Equals(Symbol other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool IsEpsilon()
        {
            return Type == SymbolType.EmptySymbol;
        }

        public static implicit operator Symbol(char value)
        {
            return new Symbol(value);
        }
        public override string ToString()
        {
            if (Type == SymbolType.EmptySymbol)
                return "#";
            return Value.ToString();
        }
    }
}
