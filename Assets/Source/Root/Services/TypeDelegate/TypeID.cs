using ModestTree;
using System;

namespace EVI
{
    public struct TypeID : IEquatable<TypeID>
    {
        Type _type;
        object _identifier;

        public TypeID(Type type, object identifier)
        {
            _type = type;
            _identifier = identifier;
        }

        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public object Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public override string ToString()
        {
            if (_identifier == null)
            {
                return _type.PrettyName();
            }

            return "{0} (ID: {1})".Fmt(_type, _identifier);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + _type.GetHashCode();
                hash = hash * 29 + (_identifier == null ? 0 : _identifier.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (other is TypeID)
            {
                TypeID otherId = (TypeID)other;
                return otherId == this;
            }

            return false;
        }

        public bool Equals(TypeID that)
        {
            return this == that;
        }

        public static bool operator ==(TypeID left, TypeID right)
        {
            return left.Type == right.Type && Equals(left.Identifier, right.Identifier);
        }

        public static bool operator !=(TypeID left, TypeID right)
        {
            return !left.Equals(right);
        }
    }
}